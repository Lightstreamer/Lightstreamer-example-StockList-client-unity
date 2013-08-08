#region License
/*
* Copyright 2013 Weswit Srl
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/
#endregion License

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

    public StocklistHandyTableListener(ILightstreamerListener listener)
    {
        if (listener == null)
        {
            throw new ArgumentNullException("listener");
        }
        listeners.Add(listener);
    }

	public void AppendListener(ILightstreamerListener listener)
	{
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
