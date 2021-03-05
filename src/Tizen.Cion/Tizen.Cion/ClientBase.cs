using System;
using System.Collections.Generic;
using System.Text;

namespace Tizen.Cion
{
    /// <summary>
    /// </summary>
    /// <since_tizen> 9 </since_tizen>
    public abstract class ClientBase : IDisposable
    {
        private readonly string LogTag = "Tizen.Cion";
        private ClientSafeHandle _handle;

        private SecurityInfo _securityInfo;
        private PeerInfo _peer;

        private Interop.CionClient.CionClientDiscoveredCb _discoveredCb;
        private Interop.CionClient.CionClientConnectionStatusChangedCb _connectionStatusChangedCb;
        private Interop.CionClient.CionClientErrorReportedCb _errorReportedCb;
        private Interop.CionClient.CionClientPayloadRecievedCb _payloadRecievedCb;

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public string ServiceName { get; }

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public ClientBase(string serviceName)
        {
            ServiceName = serviceName;

            Interop.Cion.ErrorCode ret = Interop.CionClient.CionClientCreate(out _handle, serviceName);
            if (ret != Interop.Cion.ErrorCode.None)
            {
                throw CionErrorFactory.GetException(ret, "Failed to create client.");
            }

            _connectionStatusChangedCb = new Interop.CionClient.CionClientConnectionStatusChangedCb(
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
            ret = Interop.CionClient.CionClientAddConnectionStatusChangedCb(_handle, _connectionStatusChangedCb, IntPtr.Zero);
            if (ret != Interop.Cion.ErrorCode.None)
            {
                throw CionErrorFactory.GetException(ret, "Failed to add connection status changed callback.");
            }

            _errorReportedCb = new Interop.CionClient.CionClientErrorReportedCb(
                (string service, IntPtr peerInfo, int error, IntPtr userData) =>
                {
                    OnErrorReported(error, new PeerInfo(new PeerInfoSafeHandle(peerInfo, false)));
                });
            ret = Interop.CionClient.CionClientAddErrorReportedCb(_handle, _errorReportedCb, IntPtr.Zero);
            if (ret != Interop.Cion.ErrorCode.None)
            {
                throw CionErrorFactory.GetException(ret, "Failed to add error reported callback.");
            }
            _payloadRecievedCb = new Interop.CionClient.CionClientPayloadRecievedCb(
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
                    OnPayloadReceived(receivedPayload);
                });
            ret = Interop.CionClient.CionClientAddPayloadReceivedCb(_handle, _payloadRecievedCb, IntPtr.Zero);
            if (ret != Interop.Cion.ErrorCode.None)
            {
                throw CionErrorFactory.GetException(ret, "Failed to add payload received callback.");
            }
        }

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public void TryDiscovery()
        {
            Interop.CionClient.CionClientDiscoveredCb cb = new Interop.CionClient.CionClientDiscoveredCb(
                (string serviceName, IntPtr peerInfo, IntPtr userData) =>
                {
                    PeerInfo peer = new PeerInfo(new PeerInfoSafeHandle(peerInfo, false));
                    OnDiscovered(peer);
                });
            Interop.Cion.ErrorCode ret = Interop.CionClient.CionClientTryDiscovery(_handle, cb, IntPtr.Zero);
            if (ret != Interop.Cion.ErrorCode.None)
            {
                throw CionErrorFactory.GetException(ret, "Failed to try discovery.");
            }
            _discoveredCb = cb;
        }

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public void StopDiscovery()
        {
            Interop.Cion.ErrorCode ret = Interop.CionClient.CionClientStopDiscovery(_handle);
            if (ret != Interop.Cion.ErrorCode.None)
            {
                throw CionErrorFactory.GetException(ret, "Failed to stop discovery.");
            }
            _discoveredCb = null;
        }

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public void Connect(PeerInfo peer)
        {
            Interop.Cion.ErrorCode ret = Interop.CionClient.CionClientConnect(_handle, peer?._handle);
            if (ret != Interop.Cion.ErrorCode.None)
            {
                throw CionErrorFactory.GetException(ret, "Failed to connect.");
            }
            _peer = peer;
        }

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public void Disconnect()
        {
            Interop.Cion.ErrorCode ret = Interop.CionClient.CionClientDisconnect(_handle);
            if (ret != Interop.Cion.ErrorCode.None)
            {
                throw CionErrorFactory.GetException(ret, "Failed to disconnect.");
            }
            _peer = null;
        }

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public byte[] SendData(byte[] data, int timeout)
        {
            Interop.Cion.ErrorCode ret = Interop.CionClient.CionClientSendData(_handle, data, data.Length, timeout, out byte[] returnData, out int returnDataSize);
            if (ret != Interop.Cion.ErrorCode.None)
            {
                throw CionErrorFactory.GetException(ret, "Failed to send data.");
            }

            Log.Info(LogTag, string.Format("Returned data size: {0}", returnDataSize));

            return returnData;
        }

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public void SendPayloadAsync(IPayload payload)
        {
            Interop.Cion.ErrorCode ret = Interop.CionClient.CionClientSendPayloadAsync(_handle, payload?._handle);
            if (ret != Interop.Cion.ErrorCode.None)
            {
                throw CionErrorFactory.GetException(ret, "Failed to send payload.");
            }
        }

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public PeerInfo GetPeerInfo()
        {
            return _peer;
        }

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        protected abstract void OnConnectionStatusChanged(PeerInfo peer, ConnectionStatus status);

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        protected abstract void OnPayloadReceived(IPayload data);

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        protected abstract void OnErrorReported(int code, PeerInfo peer);

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        protected abstract void OnDiscovered(PeerInfo peer);

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
        ~ClientBase()
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
