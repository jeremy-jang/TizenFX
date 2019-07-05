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

namespace Tizen.CapabilityManager
{
    /// <summary>
    /// This class provides methods and properties to get information of the remote device and remote application.
    /// </summary>
    /// <since_tizen> 6 </since_tizen>
    public static class CapabilityManager
    {
        private const string LogTag = "Tizen.CapabilityManager";

        /// <summary>
        /// Retrieves the remote device information of currently discovered.
        /// </summary>
        /// <returns>Returns the list of devices.</returns>
        /// <since_tizen> 6 </since_tizen>
        public static IEnumerable<Device> GetRemoteDevices()
        {
            List<Device> deviceList = new List<Device>();

            Interop.CapabilityManager.Device.ForeachDeviceCallback cb = (handle, userData) =>
            {
                deviceList.Add(new Device(handle));
                return 0;
            };

            Interop.CapabilityManager.ErrorCode err = Interop.CapabilityManager.Device.ForeachDevice(cb, IntPtr.Zero);
            if (err != Interop.CapabilityManager.ErrorCode.None)
            {
                throw new InvalidIOException(string.Format("Failed to get device Iist. err = {0}", err));
            }
            return deviceList;
        }
    }
}
