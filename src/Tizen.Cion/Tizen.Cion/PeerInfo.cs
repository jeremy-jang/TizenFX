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

namespace Tizen.Cion
{
    /// <summary>
    /// </summary>
    /// <since_tizen> 9 </since_tizen>
    public class PeerInfo : IDisposable
    {
        internal PeerInfoSafeHandle _handle;

        internal PeerInfo(PeerInfoSafeHandle handle)
        {
            _handle = handle;
        }

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public string DeviceId
        {
            get
            {
                Interop.Cion.ErrorCode ret = Interop.CionPeerInfo.CionPeerInfoGetDeviceId(_handle, out string deviceId);
                if (ret != Interop.Cion.ErrorCode.None)
                {
                    // error handling ?? property should not throw exception.
                    return "";
                }
                return deviceId;
            }
        }

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public string DeviceName
        {
            get
            {
                Interop.Cion.ErrorCode ret = Interop.CionPeerInfo.CionPeerInfoGetDeviceName(_handle, out string deviceName);
                if (ret != Interop.Cion.ErrorCode.None)
                {
                    // error handling ?? property should not throw exception.
                    return "";
                }
                return deviceName;
            }
        }

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public string DevicePlatform
        {
            get
            {
                Interop.Cion.ErrorCode ret = Interop.CionPeerInfo.CionPeerInfoGetDevicePlatform(_handle, out string devicePlatform);
                if (ret != Interop.Cion.ErrorCode.None)
                {
                    // error handling ?? property should not throw exception.
                    return "";
                }
                return devicePlatform;
            }
        }

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public string DevicePlatformVersion
        {
            get
            {
                Interop.Cion.ErrorCode ret = Interop.CionPeerInfo.CionPeerInfoGetDevicePlatformVersion(_handle, out string devicePlatformVersion);
                if (ret != Interop.Cion.ErrorCode.None)
                {
                    // error handling ?? property should not throw exception.
                    return "";
                }
                return devicePlatformVersion;
            }
        }

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public string DeviceType
        {
            get
            {
                Interop.Cion.ErrorCode ret = Interop.CionPeerInfo.CionPeerInfoGetDeviceType(_handle, out string deviceType);
                if (ret != Interop.Cion.ErrorCode.None)
                {
                    // error handling ?? property should not throw exception.
                    return "";
                }
                return deviceType;
            }
        }

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public string AppId
        {
            get
            {
                Interop.Cion.ErrorCode ret = Interop.CionPeerInfo.CionPeerInfoGetAppId(_handle, out string AppId);
                if (ret != Interop.Cion.ErrorCode.None)
                {
                    // error handling ?? property should not throw exception.
                    return "";
                }
                return AppId;
            }
        }

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public string AppVersion
        {
            get
            {
                Interop.Cion.ErrorCode ret = Interop.CionPeerInfo.CionPeerInfoGetAppVersion(_handle, out string AppVersion);
                if (ret != Interop.Cion.ErrorCode.None)
                {
                    // error handling ?? property should not throw exception.
                    return "";
                }
                return AppVersion;
            }
        }

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public string UUID
        {
            get
            {
                Interop.Cion.ErrorCode ret = Interop.CionPeerInfo.CionPeerInfoGetUuid(_handle, out string uuid);
                if (ret != Interop.Cion.ErrorCode.None)
                {
                    // error handling ?? property should not throw exception.
                    return "";
                }
                return uuid;
            }
        }

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
        ~PeerInfo()
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
