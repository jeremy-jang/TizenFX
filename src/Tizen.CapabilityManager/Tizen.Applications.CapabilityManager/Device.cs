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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tizen.CapabilityManager
{
    /// <summary>
    /// This class provides methods and properties to get information of the remote device.
    /// </summary>
    /// <since_tizen> 6 </since_tizen>
    public class Device : IDisposable
    {
        private const string LogTag = "Tizen.CapabilityManager";

        private IntPtr _handle = IntPtr.Zero;

        private string _deviceId = string.Empty;
        private string _modelName = string.Empty;
        private string _deviceName = string.Empty;
        private string _platformVersion = string.Empty;
        private string _profile = string.Empty;
        private string _swVersion = string.Empty;

        internal Device(IntPtr handle)
        {
            IntPtr clone = IntPtr.Zero;

            Interop.CapabilityManager.ErrorCode err = Interop.CapabilityManager.Device.Clone(handle, out clone);
            if (err != Interop.CapabilityManager.ErrorCode.None)
            {
                throw new OutOfMemoryException();
            }

            _handle = clone;
        }

        /// <summary>
        /// Gets the device ID of the device.
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
        public string DeviceId
        {
            get
            {
                if (!String.IsNullOrEmpty(_deviceId))
                    return _deviceId;
                Interop.CapabilityManager.ErrorCode err = Interop.CapabilityManager.Device.GetDeviceId(_handle, out _deviceId);
                if (err != Interop.CapabilityManager.ErrorCode.None)
                { 
                    Log.Warn(LogTag, "Failed to get the device ID. err = " + err);
                }
                return _deviceId;
            }
        }

        /// <summary>
        /// Gets the model name of the device.
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
        public string ModelName
        {
            get
            {
                if (!String.IsNullOrEmpty(_modelName))
                    return _modelName;
                Interop.CapabilityManager.ErrorCode err = Interop.CapabilityManager.Device.GetModelName(_handle, out _modelName);
                if (err != Interop.CapabilityManager.ErrorCode.None)
                {
                    Log.Warn(LogTag, "Failed to get the model name. err = " + err);
                }
                return _modelName;
            }
        }

        /// <summary>
        /// Gets the device name of the device.
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
        public string DeviceName
        {
            get
            {
                if (!String.IsNullOrEmpty(_deviceName))
                    return _deviceName;
                Interop.CapabilityManager.ErrorCode err = Interop.CapabilityManager.Device.GetDeviceName(_handle, out _deviceName);
                if (err != Interop.CapabilityManager.ErrorCode.None)
                {
                    Log.Warn(LogTag, "Failed to get the device name. err = " + err);
                }
                return _deviceName;
            }
        }

        /// <summary>
        /// Gets the platform version of the device.
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
        public string PlatformVersion
        {
            get
            {
                if (!String.IsNullOrEmpty(_platformVersion))
                    return _platformVersion;
                Interop.CapabilityManager.ErrorCode err = Interop.CapabilityManager.Device.GetPlatformVersion(_handle, out _platformVersion);
                if (err != Interop.CapabilityManager.ErrorCode.None)
                {
                    Log.Warn(LogTag, "Failed to get the platform version. err = " + err);
                }
                return _platformVersion;
            }
        }

        /// <summary>
        /// Gets the profile of the device.
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
        public string Profile
        {
            get
            {
                if (!String.IsNullOrEmpty(_profile))
                    return _profile;
                Interop.CapabilityManager.ErrorCode err = Interop.CapabilityManager.Device.GetProfile(_handle, out _profile);
                if (err != Interop.CapabilityManager.ErrorCode.None)
                {
                    Log.Warn(LogTag, "Failed to get the profile. err = " + err);
                }
                return _profile;
            }
        }

        /// <summary>
        /// Gets the SW version of the device.
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
        public string SWVersion
        {
            get
            {
                if (!String.IsNullOrEmpty(_swVersion))
                    return _swVersion;
                Interop.CapabilityManager.ErrorCode err = Interop.CapabilityManager.Device.GetSWVersion(_handle, out _swVersion);
                if (err != Interop.CapabilityManager.ErrorCode.None)
                {
                    Log.Warn(LogTag, "Failed to get the SW version. err = " + err);
                }
                return _swVersion;
            }
        }

        /// <summary>
        /// Retrieves the application information of remote device
        /// </summary>
        /// <returns>Returns the list of devices.</returns>
        /// <exception cref="InvalidIOException">Thrown when an internal IO error occurs.</exception>
        /// <since_tizen> 6 </since_tizen>
        public IEnumerable<ApplicationInfo> GetApplications()
        {
            List<ApplicationInfo> list = new List<ApplicationInfo>();

            Interop.CapabilityManager.ApplicationInfo.ForeachAppCallback cb = (handle, userData) =>
            {
                list.Add(new ApplicationInfo(handle));
                return 0;
            };

            Interop.CapabilityManager.ErrorCode err = Interop.CapabilityManager.ApplicationInfo.ForeachApplication(_handle, cb, IntPtr.Zero);
            if (err != Interop.CapabilityManager.ErrorCode.None)
            {
                throw new InvalidIOException(string.Format("Failed to get device Iist. err = {0}", err));
            }

            return list;
        }

        /// <summary>
        /// Reply callback for the app control request
        /// </summary>
        /// <param name="request">The AppControl of the request that has been sent</param>
        /// <param name="reply">The AppControl in which the results of the callee are contained</param>
        /// <param name="result">The result of the launch request</param>
        /// <since_tizen> 6 </since_tizen>
        public delegate void AppControlReplyCallback(AppControl request, AppControl reply, AppControlReplyResult result);

        /// <summary>
        ///
        /// </summary>
        /// <exception cref="OutOfMemoryException">Thrown when there is not enough memory to continue the execution of the method.</exception>
        /// <exception cref="InvalidIOException">Thrown when an internal IO error occurs.</exception>
        /// <since_tizen> 6 </since_tizen>
        public void SendAppControl(AppControl appControl, AppControlReplyCallback replyCallback)
        {
            Interop.CapabilityManager.ErrorCode err = Interop.CapabilityManager.AppControl.SetDevice(appControl._handle, _handle);
            if (err != Interop.CapabilityManager.ErrorCode.None)
            {
                throw new OutOfMemoryException();
            }

            // callback?
            err = Interop.CapabilityManager.AppControl.Send(appControl._handle, null, IntPtr.Zero);
            if (err != Interop.CapabilityManager.ErrorCode.None)
            {
                throw new InvalidIOException("Send AppControl failed");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
        public Task<AppControlResult> SendAppcontrolAsync(AppControl appControl, AppControlReplyCallback replyCallback)
        {
            // Should be implemented?
            var task = new TaskCompletionSource<AppControlResult>();

            return task.Task;
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
                    Interop.CapabilityManager.Device.Destroy(_handle);
                    _handle = IntPtr.Zero;
                }

                _disposed = true;
            }
        }

        /// <summary>
        /// Finalizer of the class Device.
        /// </summary>
        ~Device()
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
