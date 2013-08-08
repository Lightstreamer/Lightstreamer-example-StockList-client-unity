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
using System.Net;

using Lightstreamer.DotNet.Client;

// This is the class handling the Lightstreamer Client,
// the entry point for Lightstreamer update events.

class LightstreamerClient
{
    private string[] items;
    private string[] fields;

    private ILightstreamerListener listener;
    private LSClient client;
	private StocklistConnectionListener ls;
    private SubscribedTableKey tableKey;
	private StocklistHandyTableListener hl;

    public LightstreamerClient(
        ILightstreamerListener listener,
        string[] items, string[] fields)
    {
        if (listener == null)
        {
            throw new ArgumentNullException("listener is null");
        }
        this.items = items;
        this.fields = fields;
        this.listener = listener;
        client = new LSClient();

    }

    public void Stop()
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
        ls = new StocklistConnectionListener(listener);
        client.OpenConnection(connInfo, ls);

        SimpleTableInfo tableInfo = new ExtendedTableInfo(
            items, "MERGE", fields, true);
        tableInfo.DataAdapter = "QUOTE_ADAPTER";
		
		hl = new StocklistHandyTableListener(listener);
        tableKey = client.SubscribeTable(tableInfo, hl, false);
    }

	public void AppendListener(ILightstreamerListener listener)
	{
		if (ls != null)
			ls.AppendListener(listener);
		if (hl != null)
			hl.AppendListener(listener);
		
	}

}


