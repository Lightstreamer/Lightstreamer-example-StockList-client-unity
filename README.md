# Lightstreamer - Stock-List Demo - Unity Client #

<!-- END DESCRIPTION lightstreamer-example-stocklist-client-unity -->

This project includes a demo client showing the integration between a modified version of the Lightstreamer .NET Client Library and the Unity 3D Development platform.<br>

At this time, Unity 3D support is experimental and some features may not work. Specifically, the Web Player build (the non-NaCL version) is known to NOT work. Moreover, the Android build fails due to errors in the original Unity 3D code.

## Dig the code ##

The .NET code should be located inside the Assets/Sources directory.<br>
Specifically, Lightstreamer connection information is defined inside LightningBolt.cs (pushServerHost, items, fields).
LightstreamerClient.cs, ILightstreamerListener.cs, StocklistConnectionListener.cs and StocklistHandyTableListener.cs are the Lightstreamer bits in this demo.

* <b>StockListConnectionListener.cs</b>: it's the Lightstreamer ConnectionListener implementation that is passed to LSClient for receiving connection-related events. In this project, this object routes all the events to ILightstreamerListener.
* <b>StocklistHandyTableListener.cs</b>: it's the Lightstreamer HandyTableListener implementation that is passed to LSClient for receiving data-related events. In this project, this object routes all the events to ILightstreamerListener.
* <b>LightstreamerClient.cs</b>: it's a wrapper class that encapsulates the Lightstreamer LSClient object, exposting start() and stop() methods.

Check out the sources for further explanations. The Lightstreamer Documentation is available at: http://www.lightstreamer.com/doc<br>

<i>NOTE: not all the functionalities of the modified Lightstreamer .NET Client Library are exposed by the classes listed above. You can easily expand those functionalities using the [Lightstreamer .NET Client API](http://www.lightstreamer.com/docs/client_dotnet_api/frames.html) as a reference. </i>

For any inquiry, please email support@lightstreamer.com.

<!-- START DESCRIPTION lightstreamer-example-stocklist-client-unity -->

# Build #

The Unity 3D Development platform must be installed in order to build and run this demo. This apply to a modified version of the official Unity 3D demo. In particular, the "Lightning Bolt" demo (Lightning Bolt.unity file) is the one containing the modified Lightstreamer .NET Client library code.

## Getting started, Compile & Run ##

* Download and Install Unity 3D from: [http://unity3d.com/unity/download](http://unity3d.com/unity/download)
* Download the [complete Unity project](http://www.lightstreamer.com/download/1060/) of this demo from the "From the Labs -> Lightstreamer Library for Unity Clients" section of [Lightstreamer download page](http://www.lightstreamer.com/download). 
* Open "Assets/Lightning bolt.unity" double clicking on it. The Unity Development Environment should open.

You can then Build & Run the "Lightning Bolt" project for Windows or MacOSX.

# See Also #

## Lightstreamer Adapters needed by this demo client ##

<!-- START RELATED_ENTRIES -->
* [Lightstreamer - Stock- List Demo - Java Adapter](https://github.com/Weswit/Lightstreamer-example-Stocklist-adapter-java)
* [Lightstreamer - Reusable Metadata Adapters- Java Adapter](https://github.com/Weswit/Lightstreamer-example-ReusableMetadata-adapter-java)

<!-- END RELATED_ENTRIES -->

## Similar demo clients that may interest you ##

* [Lightstreamer - Stock-List Demos - HTML Clients](https://github.com/Weswit/Lightstreamer-example-Stocklist-client-javascript)
* [Lightstreamer - Basic Stock-List Demo - jQuery (jqGrid) Client](https://github.com/Weswit/Lightstreamer-example-StockList-client-jquery)
* [Lightstreamer - Stock-List Demo - Dojo Toolkit Client](https://github.com/Weswit/Lightstreamer-example-StockList-client-dojo)
* [Lightstreamer - Basic Stock-List Demo - Java SE (Swing) Client](https://github.com/Weswit/Lightstreamer-example-StockList-client-java)
* [Lightstreamer - Basic Stock-List Demo - .NET Client](https://github.com/Weswit/Lightstreamer-example-StockList-client-dotnet)
* [Lightstreamer - Stock-List Demos - Flex Clients](https://github.com/Weswit/Lightstreamer-example-StockList-client-flex)
* [Lightstreamer - Basic Stock-List Demo - Silverlight Client](https://github.com/Weswit/Lightstreamer-example-StockList-client-silverlight)


# Lightstreamer Compatibility Notes #

- Compatible with Lightstreamer .NET Client API version 2.1 or newer.
- For Lightstreamer Allegro (+ .NET Client API support), Presto, Vivace.
