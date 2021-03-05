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
using System.Collections.Generic;
using System.Text;

namespace Tizen.Cion
{
    /// <summary>
    /// </summary>
    /// <since_tizen> 9 </since_tizen>
    public abstract class BroadcastBase : IDisposable
    {
        private BroadcastSafeHandle _handle;

        private Interop.CionBroadcast.CionBroadcastPayloadReceivedCb _payloadReceivedCb;

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public string Topic { get; }

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public BroadcastBase(string topicName)
        {
            Topic = topicName;

            Interop.Cion.ErrorCode ret = Interop.CionBroadcast.CionBroadcastCreate(out _handle, topicName);
            if (ret != Interop.Cion.ErrorCode.None)
            {
                throw CionErrorFactory.GetException(ret, "Failed to create broadcast.");
            }

            _payloadReceivedCb = new Interop.CionBroadcast.CionBroadcastPayloadReceivedCb(
                (IntPtr broadcast, IntPtr peerInfo, IntPtr payload, IntPtr userData) =>
                {
                    IPayload receivedPayload;
                    Interop.CionPayload.CionPayloadGetType(payload, out Interop.CionPayload.PayloadType type);
                    switch (type)
                    {
                        case Interop.CionPayload.PayloadType.Data:
                            receivedPayload = new DataPayload(new PayloadSafeHandle(payload, false));
                            break;
                        case Interop.CionPayload.PayloadType.File:
                            receivedPayload = new FilePayload(new PayloadSafeHandle(payload, false));
                            break;
                        default:
                            throw new ArgumentException("Invalid payload type received.");
                    }
                    OnPayloadReceived(receivedPayload, new PeerInfo(new PeerInfoSafeHandle(peerInfo, false)));
                });
            ret = Interop.CionBroadcast.CionBroadcastAddPayloadReceivedCb(_handle, _payloadReceivedCb, IntPtr.Zero);
            if (ret != Interop.Cion.ErrorCode.None)
            {
                throw CionErrorFactory.GetException(ret, "Failed to add payload received callback.");
            }
        }

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public void Subscribe()
        {
            Interop.Cion.ErrorCode ret = Interop.CionBroadcast.CionBroadcastSubscribe(_handle);
            if (ret != Interop.Cion.ErrorCode.None)
            {
                throw CionErrorFactory.GetException(ret, "Failed to subscribe.");
            }
        }

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public void Unsubscribe()
        {
            Interop.Cion.ErrorCode ret = Interop.CionBroadcast.CionBroadcastUnsubscribe(_handle);
            if (ret != Interop.Cion.ErrorCode.None)
            {
                throw CionErrorFactory.GetException(ret, "Failed to unsubscribe.");
            }
        }

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public void Publish(IPayload payload)
        {
            Interop.Cion.ErrorCode ret = Interop.CionBroadcast.CionBroadcastPublish(_handle, payload?._handle);
            if (ret != Interop.Cion.ErrorCode.None)
            {
                throw CionErrorFactory.GetException(ret, "Failed to publish payload.");
            }
        }

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        protected abstract void OnPayloadReceived(IPayload payload, PeerInfo peer);

        #region IDisposable Support
        private bool disposedValue = false;

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _handle.Dispose();
                }
                disposedValue = true;
            }
        }

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        ~BroadcastBase()
        {
            Dispose(false);
        }

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
