using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

using Lightstreamer.DotNet.Client;

// This is the class handling the Lightstreamer Client,
// the entry point for Lightstreamer update events.

class LightstreamerClient
{
    private LSClient client;
	private StocklistConnectionListener ls;
	private StocklistHandyTableListener hl;

    public LightstreamerClient()
    {
        client = new LSClient();
    }

	public void Stop(SubscribedTableKey tableKey)
	{
        if (tableKey != null)
            client.UnsubscribeTable(tableKey);
        tableKey = null;
        client.CloseConnection();
    }

    public void Start(string pushServerUrl)
    {
        ConnectionInfo connInfo = new ConnectionInfo();
        connInfo.PushServerUrl = pushServerUrl;
        connInfo.Adapter = "DEMO";
        ls = new StocklistConnectionListener();
        client.OpenConnection(connInfo, ls);
		hl = new StocklistHandyTableListener();
    }

	public SubscribedTableKey AppendListener(ILightstreamerListener listener, string[] items, string[] fields)
	{
		SubscribedTableKey tableKey = null;
		if (listener == null)
		{
			throw new ArgumentNullException("listener is null");
		}
		if (ls != null)
			ls.AppendListener(listener);
		if (hl != null) {
			hl.AppendListener (listener);
		
			SimpleTableInfo tableInfo = new ExtendedTableInfo (
				items, "MERGE", fields, true);
			tableInfo.DataAdapter = "QUOTE_ADAPTER";
			tableKey = client.SubscribeTable (tableInfo, hl, false);
		}

		return tableKey;
	}

}


