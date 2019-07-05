/*
 * Copyright (c) 2019 Samsung Electronics Co., Ltd All Rights Reserved
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

namespace Tizen.CapabilityManager
{
    /// <summary>
    /// This class provides methods and properties to get application information of the remote device.
    /// </summary>
    /// <since_tizen> 6 </since_tizen>
    public class ApplicationInfo :IDisposable
    {
        private const string LogTag = "Tizen.CapabilityManager";

        IntPtr _handle;
        private string _appId = string.Empty;
        private string _pkgId = string.Empty;
        private string _label = string.Empty;
        private string _version = string.Empty;
        Device _device = null;

        internal ApplicationInfo(IntPtr handle)
        {
            IntPtr clone = IntPtr.Zero;

            Interop.CapabilityManager.ErrorCode err = Interop.CapabilityManager.ApplicationInfo.Clone(handle, out clone);
            if (err != Interop.CapabilityManager.ErrorCode.None)
            {
                throw new OutOfMemoryException();
            }

            _handle = clone;
        }

        /// <summary>
        /// Gets the application ID of the application
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
        public string AppId
        {
            get
            {
                if (!String.IsNullOrEmpty(_appId))
                    return _appId;
                Interop.CapabilityManager.ErrorCode err = Interop.CapabilityManager.ApplicationInfo.GetAppId(_handle, out _appId);
                if (err != Interop.CapabilityManager.ErrorCode.None)
                {
                    Log.Warn(LogTag, "Failed to get the appId. err = " + err);
                }
                return _appId;
            }
        }

        /// <summary>
        /// Gets the package ID of the application
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
        public string PkgId
        {
            get
            {
                if (!String.IsNullOrEmpty(_pkgId))
                    return _pkgId;
                Interop.CapabilityManager.ErrorCode err = Interop.CapabilityManager.ApplicationInfo.GetPkgId(_handle, out _pkgId);
                if (err != Interop.CapabilityManager.ErrorCode.None)
                {
                    Log.Warn(LogTag, "Failed to get the appId. err = " + err);
                }
                return _pkgId;
            }
        }

        /// <summary>
        /// Gets the label of the application
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
        public string Label
        {
            get
            {
                if (!String.IsNullOrEmpty(_label))
                    return _label;
                Interop.CapabilityManager.ErrorCode err = Interop.CapabilityManager.ApplicationInfo.GetLabel(_handle, out _label);
                if (err != Interop.CapabilityManager.ErrorCode.None)
                {
                    Log.Warn(LogTag, "Failed to get the label. err = " + err);
                }
                return _label;
            }
        }

        /// <summary>
        /// Gets the version of the application
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
        public string Version
        {
            get
            {
                if (!String.IsNullOrEmpty(_version))
                    return _version;
                Interop.CapabilityManager.ErrorCode err = Interop.CapabilityManager.ApplicationInfo.GetVersion(_handle, out _version);
                if (err != Interop.CapabilityManager.ErrorCode.None)
                {
                    Log.Warn(LogTag, "Failed to get the version. err = " + err);
                }
                return _version;
            }
        }

        /// <summary>
        /// Gets the device of the application
        /// </summary>
        /// <exception cref="OutOfMemoryException">Thrown when there is not enough memory to continue the execution of the method.</exception>
        /// <since_tizen> 6 </since_tizen>
        public Device Device
        {
            get
            {
                if (_device != null)
                    return _device;
                // Should Device implements SafeHandle?
                IntPtr handle;
                Interop.CapabilityManager.ErrorCode err = Interop.CapabilityManager.ApplicationInfo.GetDevice(_handle, out handle);
                if (err != Interop.CapabilityManager.ErrorCode.None)
                {
                    Log.Warn(LogTag, "Failed to get the device. err = " + err);
                }
                _device = new Device(handle);
                return _device;
            }
        }

        #region IDisposable Support
        private bool _disposed = false;

        protected void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                }

                if (_handle != IntPtr.Zero)
                {
                    Interop.CapabilityManager.ApplicationInfo.Destroy(_handle);
                    _handle = IntPtr.Zero;
                }

                _disposed = true;
            }
        }

        /// <summary>
        /// Finalizer of the class Device.
        /// </summary>
        ~ApplicationInfo()
        {
            Dispose(false);
        }

        /// <summary>
        /// Release all the resources used by the class Device.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
