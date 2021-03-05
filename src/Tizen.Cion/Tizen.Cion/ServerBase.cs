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

namespace Tizen.Cion
{
    /// <summary>
    /// </summary>
    /// <since_tizen> 9 </since_tizen>
    public abstract class ServerBase : IDisposable
    {
        private const string LogTag = "Tizen.Cion";

        private ServerSafeHandle _handle;
        private Interop.CionServer.CionServerConnectionRequestCb _connectionRequestCb;
        private Interop.CionServer.CionServerConnectionStatusChangedCb _connectionStatusChangedCb;
        private Interop.CionServer.CionServerDataReceivedCb _dataReceivedCb;
        private Interop.CionServer.CionServerErrorReportedCb _errorReportedCb;
        private Interop.CionServer.CionServerPayloadRecievedCb _payloadRecievedCb;

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public string ServiceName { get; }

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        /// <exception cref="OutOfMemoryException">Thrown when there is not enough memory to continue the execution of the method.</exception> 
        public ServerBase(string serviceName)
        {
            ServiceName = serviceName;

            Interop.Cion.ErrorCode ret = Interop.CionServer.CionServerCreate(out _handle, serviceName);
            if (ret != Interop.Cion.ErrorCode.None)
            {
                throw CionErrorFactory.GetException(ret, "Failed to create server handle.");
            }

            _connectionStatusChangedCb = new Interop.CionServer.CionServerConnectionStatusChangedCb(
                (string service, IntPtr peerInfo, Interop.Cion.ConnectionStatus status, IntPtr userData) =>
                {
                    ConnectionStatus connectionStatus;
                    switch (status)
                    {
                        case Interop.Cion.ConnectionStatus.Offline:
                            connectionStatus = ConnectionStatus.Offline;
                            break;
                        case Interop.Cion.ConnectionStatus.Online:
                            connectionStatus = ConnectionStatus.Online;
                            break;
                        default:
                            throw new ArgumentException(string.Format("Invalid connection status received: {0}", status));
                    }
                    OnConnectionStatusChanged(new PeerInfo(new PeerInfoSafeHandle(peerInfo, false)), connectionStatus);
                });
            ret = Interop.CionServer.CionServerAddConnectionStatusChangedCb(_handle, _connectionStatusChangedCb, IntPtr.Zero);
            if (ret != Interop.Cion.ErrorCode.None)
            {
                throw CionErrorFactory.GetException(ret, "Failed to add connection status changed callback.");
            }

            _dataReceivedCb = new Interop.CionServer.CionServerDataReceivedCb(
                (string service, IntPtr peerInfo, byte[] data, int dataSize, out byte[] returnData, out int returnDataSize, IntPtr userData) =>
                {
                    returnData = OnDataReceived(data, new PeerInfo(new PeerInfoSafeHandle(peerInfo, false)));
                    returnDataSize = returnData.Length;
                });
            ret = Interop.CionServer.CionServerSetDataReceivedCb(_handle, _dataReceivedCb, IntPtr.Zero);
            if (ret != Interop.Cion.ErrorCode.None)
            {
                throw CionErrorFactory.GetException(ret, "Failed to set data received callback.");
            }

            _errorReportedCb = new Interop.CionServer.CionServerErrorReportedCb(
                (string service, IntPtr peerInfo, int error, IntPtr userData) =>
                {
                    OnErrorReported(error, new PeerInfo(new PeerInfoSafeHandle(peerInfo, false)));
                });
            ret = Interop.CionServer.CionServerAddErrorReportedCb(_handle, _errorReportedCb, IntPtr.Zero);
            if (ret != Interop.Cion.ErrorCode.None)
            {
                throw CionErrorFactory.GetException(ret, "Failed to add error reported callback.");
            }

            _payloadRecievedCb = new Interop.CionServer.CionServerPayloadRecievedCb(
                (string service, IntPtr peerInfo, IntPtr payload, IntPtr userData) =>
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
            ret = Interop.CionServer.CionServerAddPayloadReceivedCb(_handle, _payloadRecievedCb, IntPtr.Zero);
            if (ret != Interop.Cion.ErrorCode.None)
            {
                throw CionErrorFactory.GetException(ret, "Failed to add payload received callback.");
            }
        }

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        ~ServerBase()
        {
            Dispose(false);
        }

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        /// <exception cref="InvalidOperationException"></exception>
        public void Listen()
        {
            Interop.CionServer.CionServerConnectionRequestCb cb = new Interop.CionServer.CionServerConnectionRequestCb(
                (serviceName, peerInfo, userData) => {
                    // the given peerInfo is const, this should not be released by us
                    OnConnentionRequest(new PeerInfo(new PeerInfoSafeHandle(peerInfo, false)));
                });
            Interop.Cion.ErrorCode ret = Interop.CionServer.CionServerListen(_handle, cb, IntPtr.Zero);
            if (ret != Interop.Cion.ErrorCode.None)
            {
                throw CionErrorFactory.GetException(ret, "Failed to listen server.");
            }
            _connectionRequestCb = cb;
        }

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        /// <exception cref="InvalidOperationException"></exception>
        public void Stop()
        {
            Interop.Cion.ErrorCode ret = Interop.CionServer.CionServerStop(_handle);
            if (ret != Interop.Cion.ErrorCode.None)
            {
                throw CionErrorFactory.GetException(ret, "Failed to stop server.");
            }
            _connectionRequestCb = null;
        }

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        /// <exception cref="InvalidOperationException"></exception>
        public void Disconnect(PeerInfo peer)
        {
            Interop.Cion.ErrorCode ret = Interop.CionServer.CionServerDisconnect(_handle);
            if (ret != Interop.Cion.ErrorCode.None)
            {
                throw CionErrorFactory.GetException(ret, "Failed to stop server.");
            }
        }

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public void SendPayload(IPayload payload, PeerInfo peer)
        {
            Interop.Cion.ErrorCode ret = Interop.CionServer.CionServerSendPayloadAsync(_handle, peer?._handle, payload?._handle);
            if (ret != Interop.Cion.ErrorCode.None)
            {
                throw CionErrorFactory.GetException(ret, "Failed to send payload.");
            }
        }

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public void SendPayload(IPayload payload)
        {
            var peerList = GetConnectedPeerList();
            foreach (var peer in peerList)
            {
                SendPayload(payload, peer);
            }
        }

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        IEnumerable<PeerInfo> GetConnectedPeerList()
        {
            List<PeerInfo> peerInfoList = new List<PeerInfo>();
            Interop.Cion.ErrorCode ret = Interop.CionServer.CionServerForeachConnectedPeerInfo(_handle, (peer, userData) =>
            {
                Interop.Cion.ErrorCode clone_ret = Interop.CionPeerInfo.CionPeerInfoClone(peer, out PeerInfoSafeHandle clone);
                if (clone_ret != Interop.Cion.ErrorCode.None)
                {
                    // Some error log or throw?
                    return;
                }
                peerInfoList.Add(new PeerInfo(clone));
            }, IntPtr.Zero);
            return peerInfoList;
        }

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        protected abstract void OnConnectionStatusChanged(PeerInfo peerInfo, ConnectionStatus status);

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        protected abstract byte[] OnDataReceived(byte[] data, PeerInfo peerInfo);

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        protected abstract void OnPayloadReceived(IPayload data, PeerInfo peerInfo);

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        protected abstract bool OnConnentionRequest(PeerInfo peerInfo);

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        protected abstract void OnErrorReported(int code, PeerInfo peerInfo);

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
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
