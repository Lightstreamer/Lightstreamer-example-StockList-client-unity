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
using Lightstreamer.DotNet.Client;

public interface ILightstreamerListener
{
    // Lightstreamer Client connection status changes arrive here
    void OnStatusChange(int status, string message);
    // Lightstreamer Client data updates arrive here
    void OnItemUpdate(int itemPos, string itemName, IUpdateInfo update);
    // Lightstreamer Client lost updates info arrives here
    void OnLostUpdate(int itemPos, string itemName, int lostUpdates);
}


