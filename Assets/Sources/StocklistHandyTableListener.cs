using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Lightstreamer.DotNet.Client;

class StocklistHandyTableListener : IHandyTableListener
{
    private List<ILightstreamerListener> listeners = new List<ILightstreamerListener>();
	private ReaderWriterLock rwlock = new ReaderWriterLock();
	private const int lockt = 15000;

    public StocklistHandyTableListener()
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

    public void OnUpdate(int itemPos, string itemName, IUpdateInfo update)
    {
		rwlock.AcquireReaderLock(lockt);
		try
		{
			foreach (ILightstreamerListener listener in listeners)
			{
				listener.OnItemUpdate(itemPos, itemName, update);
			}
		}
		finally
		{
			rwlock.ReleaseReaderLock();
		}
    }

    public void OnRawUpdatesLost(int itemPos, string itemName, int lostUpdates)
    {
		rwlock.AcquireReaderLock(lockt);
		try
		{
			foreach (ILightstreamerListener listener in listeners)
			{
				listener.OnLostUpdate(itemPos, itemName, lostUpdates);
			}
		}
		finally
		{
			rwlock.ReleaseReaderLock();
		}
    }

    public void OnSnapshotEnd(int itemPos, string itemName)
    {
    }

    public void OnUnsubscr(int itemPos, string itemName)
    {
    }

    public void OnUnsubscrAll()
    {
    }
}
