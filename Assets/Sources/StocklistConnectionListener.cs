using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Lightstreamer.DotNet.Client;

class LightstreamerConnectionHandler
{
    public const int DISCONNECTED = 0;
    public const int CONNECTING = 1;
    public const int CONNECTED = 2;
    public const int STREAMING = 3;
    public const int POLLING = 4;
    public const int STALLED = 5;
    public const int ERROR = 6;
}

class StocklistConnectionListener : IConnectionListener
{

	private List<ILightstreamerListener> listeners = new List<ILightstreamerListener>();
    private bool isPolling;
	private ReaderWriterLock rwlock = new ReaderWriterLock();
	private const int lockt = 15000;

    public StocklistConnectionListener()
    {
    }

	public void AppendListener(ILightstreamerListener listener)
	{
		if (listener == null)
		{
			throw new ArgumentNullException("listener");
		}
		rwlock.AcquireWriterLock(lockt);
		try
		{
			listeners.Add(listener);
		}
		finally
		{
			rwlock.ReleaseWriterLock();
		}
	}

    public void OnConnectionEstablished()
    {
		rwlock.AcquireReaderLock(lockt);
		try
		{
			foreach (ILightstreamerListener listener in listeners)
			{
				listener.OnStatusChange(LightstreamerConnectionHandler.CONNECTED,
	            	"Connected to Lightstreamer Server...");
			}
		}
		finally
		{
			rwlock.ReleaseReaderLock();
		}
    }

    public void OnSessionStarted(bool isPolling)
    {
        string message;
        int status;
        this.isPolling = isPolling;
        if (isPolling)
        {
            message = "Lightstreamer is pushing (smart polling mode)...";
            status = LightstreamerConnectionHandler.POLLING;
        }
        else
        {
            message = "Lightstreamer is pushing (streaming mode)...";
            status = LightstreamerConnectionHandler.STREAMING;
        }
		rwlock.AcquireReaderLock(lockt);
		try
		{
			foreach (ILightstreamerListener listener in listeners)
			{
				listener.OnStatusChange(status, message);
			}
		}
		finally
		{
			rwlock.ReleaseReaderLock();
		}
    }

    public void OnNewBytes(long b) { }

    public void OnDataError(PushServerException e)
    {
		rwlock.AcquireReaderLock(lockt);
		try
		{
			foreach (ILightstreamerListener listener in listeners)
			{
				listener.OnStatusChange(LightstreamerConnectionHandler.ERROR,
	            	"Data error");
			}
		}
		finally
		{
			rwlock.ReleaseReaderLock();
		}
    }

    public void OnActivityWarning(bool warningOn)
    {
        if (warningOn)
        {
			rwlock.AcquireReaderLock(lockt);
			try
			{
				foreach (ILightstreamerListener listener in listeners)
				{
					listener.OnStatusChange(LightstreamerConnectionHandler.STALLED,
	                	"Connection stalled");
				}
			}
			finally
			{
				rwlock.ReleaseReaderLock();
			}
        }
        else
        {
            OnSessionStarted(this.isPolling);
        }
    }

    public void OnClose()
    {
		rwlock.AcquireReaderLock(lockt);
		try
		{
			foreach (ILightstreamerListener listener in listeners)
			{
				listener.OnStatusChange(LightstreamerConnectionHandler.DISCONNECTED,
	            	"Connection closed");
			}
		}
		finally
		{
			rwlock.ReleaseReaderLock();
		}
    }

    public void OnEnd(int cause)
    {
		rwlock.AcquireReaderLock(lockt);
		try
		{
			foreach (ILightstreamerListener listener in listeners)
			{
				listener.OnStatusChange(LightstreamerConnectionHandler.DISCONNECTED,
	            	"Connection forcibly closed");
			}
		}
		finally
		{
			rwlock.ReleaseReaderLock();
		}
    }

    public void OnFailure(PushServerException e)
    {
		rwlock.AcquireReaderLock(lockt);
		try
		{
			foreach (ILightstreamerListener listener in listeners)
			{
				listener.OnStatusChange(LightstreamerConnectionHandler.ERROR,
	            	"Server failure" + e.ToString());
			}
		}
		finally
		{
			rwlock.ReleaseReaderLock();
		}
    }

    public void OnFailure(PushConnException e)
    {
		rwlock.AcquireReaderLock(lockt);
		try
		{
			foreach (ILightstreamerListener listener in listeners)
			{
				listener.OnStatusChange(LightstreamerConnectionHandler.ERROR,
	            	"Connection failure " + e.ToString());
			}
		}
		finally
		{
			rwlock.ReleaseReaderLock();
		}
    }
}
