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

internal static partial class Interop
{
    internal static partial class CionBroadcast
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CionBroadcastPayloadReceivedCb(IntPtr broadcast, IntPtr peerInfo, IntPtr payload, IntPtr userData);

        [DllImport(Libraries.Cion, EntryPoint = "cion_broadcast_create")]
        internal static extern ErrorCode CionBroadcastCreate(out BroadcastSafeHandle broadcast, string topicName);

        [DllImport(Libraries.Cion, EntryPoint = "cion_broadcast_destroy")]
        internal static extern ErrorCode CionBroadcastDestroy(IntPtr broadcast);

        [DllImport(Libraries.Cion, EntryPoint = "cion_broadcast_subscribe")]
        internal static extern ErrorCode CionBroadcastSubscribe(BroadcastSafeHandle broadcast);

        [DllImport(Libraries.Cion, EntryPoint = "cion_broadcast_unsubscribe")]
        internal static extern ErrorCode CionBroadcastUnsubscribe(BroadcastSafeHandle broadcast);

        [DllImport(Libraries.Cion, EntryPoint = "cion_broadcast_publish")]
        internal static extern ErrorCode CionBroadcastPublish(BroadcastSafeHandle broadcast, PayloadSafeHandle data);

        [DllImport(Libraries.Cion, EntryPoint = "cion_broadcast_add_payload_received_cb")]
        internal static extern ErrorCode CionBroadcastAddPayloadReceivedCb(BroadcastSafeHandle broadcast, CionBroadcastPayloadReceivedCb cb, IntPtr userData);
    }
}
