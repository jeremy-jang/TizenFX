/*
 * Copyright (c) 2021 Samsung Electronics Co., Ltd All Rights Reserved
 *
 * Licensed under the Apache License, Version 2.0 (the License);
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an AS IS BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Runtime.InteropServices;
using Tizen.Cion;

using ErrorCode = Interop.Cion.ErrorCode;
using ConnectionStatus = Interop.Cion.ConnectionStatus;

internal static partial class Interop
{
    internal static partial class CionServer
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CionServerPeerInfoIterator(IntPtr peerInfo, IntPtr userData);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CionServerConnectionStatusChangedCb(string serviceName, IntPtr peerInfo, ConnectionStatus status, IntPtr userData);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CionServerDataReceivedCb(string serviceName, IntPtr peerInfo, byte[] data, int dataSize, out byte[] returnData, out int returnDataSize, IntPtr userData);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CionServerPayloadRecievedCb(string serviceName, IntPtr peerInfo, IntPtr payload, IntPtr userData);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CionServerConnectionRequestCb(string serviceName, IntPtr peerInfo, IntPtr userData);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CionServerErrorReportedCb(string serviceName, IntPtr peerInfo, int error, IntPtr userData);

        [DllImport(Libraries.Cion, EntryPoint = "cion_server_create")]
        internal static extern ErrorCode CionServerCreate(out ServerSafeHandle server, string serviceName);

        [DllImport(Libraries.Cion, EntryPoint = "cion_server_destory")]
        internal static extern ErrorCode CionServerDestroy(IntPtr server);

        [DllImport(Libraries.Cion, EntryPoint = "cion_server_set_security")]
        internal static extern ErrorCode CionServerSetSecurity(ServerSafeHandle server, IntPtr security);
        
        [DllImport(Libraries.Cion, EntryPoint = "cion_server_listen")]
        internal static extern ErrorCode CionServerListen(ServerSafeHandle server, CionServerConnectionRequestCb cb, IntPtr userData);
        
        [DllImport(Libraries.Cion, EntryPoint = "cion_server_stop")]
        internal static extern ErrorCode CionServerStop(ServerSafeHandle server);
        
        [DllImport(Libraries.Cion, EntryPoint = "cion_client_connect")]
        internal static extern ErrorCode CionClientConnect(ServerSafeHandle server, PeerInfoSafeHandle peerInfo);

        [DllImport(Libraries.Cion, EntryPoint = "cion_server_disconnect")]
        internal static extern ErrorCode CionServerDisconnect(ServerSafeHandle server);

        [DllImport(Libraries.Cion, EntryPoint = "cion_server_send_payload_async")]
        internal static extern ErrorCode CionServerSendPayloadAsync(ServerSafeHandle server, PeerInfoSafeHandle peerInfo, PayloadSafeHandle payload);

        [DllImport(Libraries.Cion, EntryPoint = "cion_server_foreach_connected_peer_info")]
        internal static extern ErrorCode CionServerForeachConnectedPeerInfo(ServerSafeHandle server, CionServerPeerInfoIterator cb, IntPtr userData);

        [DllImport(Libraries.Cion, EntryPoint = "cion_server_add_connection_status_changed_cb")]
        internal static extern ErrorCode CionServerAddConnectionStatusChangedCb(ServerSafeHandle server, CionServerConnectionStatusChangedCb cb, IntPtr userData);

        [DllImport(Libraries.Cion, EntryPoint = "cion_server_add_payload_recieved_cb")]
        internal static extern ErrorCode CionServerAddPayloadReceivedCb(ServerSafeHandle server, CionServerPayloadRecievedCb cb, IntPtr userData);

        [DllImport(Libraries.Cion, EntryPoint = "cion_server_add_error_reported_cb")]
        internal static extern ErrorCode CionServerAddErrorReportedCb(ServerSafeHandle server, CionServerErrorReportedCb cb, IntPtr userData);

        [DllImport(Libraries.Cion, EntryPoint = "cion_server_set_data_recieved_cb")]
        internal static extern ErrorCode CionServerSetDataReceivedCb(ServerSafeHandle server, CionServerDataReceivedCb cb, IntPtr userData);
    }
}
