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
using System.Runtime.InteropServices;

internal static partial class Interop
{
    internal static partial class CapabilityManager
    {
        internal enum ErrorCode
        {
            None = Tizen.Internals.Errors.ErrorCode.None,
            InvalidParameter = Tizen.Internals.Errors.ErrorCode.InvalidParameter,
            OutOfMemory = Tizen.Internals.Errors.ErrorCode.OutOfMemory,
            IoError = Tizen.Internals.Errors.ErrorCode.IoError,
            PermissionDenied = Tizen.Internals.Errors.ErrorCode.PermissionDenied
        }

        internal static partial class Device
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate int ForeachDeviceCallback(IntPtr handle, IntPtr userData);

            [DllImport(Libraries.CapabilityManager, EntryPoint = "capmgr_device_foreach_device")]
            internal static extern ErrorCode ForeachDevice(ForeachDeviceCallback callback, IntPtr userData);

            [DllImport(Libraries.CapabilityManager, EntryPoint = "capmgr_device_clone")]
            internal static extern ErrorCode Clone(IntPtr handle, out IntPtr handleClone);

            [DllImport(Libraries.CapabilityManager, EntryPoint = "capmgr_device_destroy")]
            internal static extern ErrorCode Destroy(IntPtr handle);

            [DllImport(Libraries.CapabilityManager, EntryPoint = "capmgr_device_get_device_id")]
            internal static extern ErrorCode GetDeviceId(IntPtr handle, out string deviceId);

            [DllImport(Libraries.CapabilityManager, EntryPoint = "capmgr_device_get_model_name")]
            internal static extern ErrorCode GetModelName(IntPtr handle, out string modelName);

            [DllImport(Libraries.CapabilityManager, EntryPoint = "capmgr_device_get_device_name")]
            internal static extern ErrorCode GetDeviceName(IntPtr handle, out string deviceName);

            [DllImport(Libraries.CapabilityManager, EntryPoint = "capmgr_device_get_platform_version")]
            internal static extern ErrorCode GetPlatformVersion(IntPtr handle, out string platformVersion);

            [DllImport(Libraries.CapabilityManager, EntryPoint = "capmgr_device_get_profile")]
            internal static extern ErrorCode GetProfile(IntPtr handle, out string profile);

            [DllImport(Libraries.CapabilityManager, EntryPoint = "capmgr_device_get_sw_version")]
            internal static extern ErrorCode GetSWVersion(IntPtr handle, out string SWVersion);
        }
        
        internal static partial class AppControl
        {
            internal enum Result
            {
                Ok,
            }

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate int ReplyCallback(IntPtr handle, IntPtr replyHandle, AppControl.Result res, IntPtr userData);

            [DllImport(Libraries.CapabilityManager, EntryPoint = "capmgr_app_control_create")]
            internal static extern ErrorCode Create(out IntPtr handle);

            [DllImport(Libraries.CapabilityManager, EntryPoint = "capmgr_app_control_create")]
            internal static extern ErrorCode Clone(IntPtr handle, out IntPtr handleClone);

            [DllImport(Libraries.CapabilityManager, EntryPoint = "capmgr_app_control_destroy")]
            internal static extern ErrorCode Destroy(IntPtr handle);

            [DllImport(Libraries.CapabilityManager, EntryPoint = "capmgr_app_control_get_device")]
            internal static extern ErrorCode GetDevice(IntPtr handle, out IntPtr deviceHandle);

            [DllImport(Libraries.CapabilityManager, EntryPoint = "capmgr_app_control_get_operation")]
            internal static extern ErrorCode GetOperation(IntPtr handle, out string operation);

            [DllImport(Libraries.CapabilityManager, EntryPoint = "capmgr_app_control_get_uri")]
            internal static extern ErrorCode GetUri(IntPtr handle, out string uri);

            [DllImport(Libraries.CapabilityManager, EntryPoint = "capmgr_app_control_get_mime")]
            internal static extern ErrorCode GetMime(IntPtr handle, out string mime);

            [DllImport(Libraries.CapabilityManager, EntryPoint = "capmgr_app_control_get_appid")]
            internal static extern ErrorCode GetAppId(IntPtr handle, out string appId);

            [DllImport(Libraries.CapabilityManager, EntryPoint = "capmgr_app_control_get_extra_data")]
            internal static extern ErrorCode GetExtraData(IntPtr handle, string key, out string value);

            [DllImport(Libraries.CapabilityManager, EntryPoint = "capmgr_app_control_set_device")]
            internal static extern ErrorCode SetDevice(IntPtr handle, IntPtr deviceHandle);

            [DllImport(Libraries.CapabilityManager, EntryPoint = "capmgr_app_control_set_operation")]
            internal static extern ErrorCode SetOperation(IntPtr handle, string operation);

            [DllImport(Libraries.CapabilityManager, EntryPoint = "capmgr_app_control_set_uri")]
            internal static extern ErrorCode SetUri(IntPtr handle, string uri);

            [DllImport(Libraries.CapabilityManager, EntryPoint = "capmgr_app_control_set_mime")]
            internal static extern ErrorCode SetMime(IntPtr handle, string mime);

            [DllImport(Libraries.CapabilityManager, EntryPoint = "capmgr_app_control_set_appid")]
            internal static extern ErrorCode SetAppId(IntPtr handle, string appId);

            [DllImport(Libraries.CapabilityManager, EntryPoint = "capmgr_app_control_add_extra_data")]
            internal static extern ErrorCode AddExtraData(IntPtr handle, string key, string value);

            [DllImport(Libraries.CapabilityManager, EntryPoint = "capmgr_app_control_remove_extra_data")]
            internal static extern ErrorCode RemoveExtraData(IntPtr handle, string key);

            [DllImport(Libraries.CapabilityManager, EntryPoint = "capmgr_app_control_send")]
            internal static extern ErrorCode Send(IntPtr handle, ReplyCallback callback, IntPtr userData);
        }

        internal static partial class ApplicationInfo
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate int ForeachAppCallback(IntPtr handle, IntPtr userData);

            [DllImport(Libraries.CapabilityManager, EntryPoint = "capmgr_app_info_foreach_application")]
            internal static extern ErrorCode ForeachApplication(IntPtr handle, ForeachAppCallback callback, IntPtr userData);

            [DllImport(Libraries.CapabilityManager, EntryPoint = "capmgr_app_info_clone")]
            internal static extern ErrorCode Clone(IntPtr handle, out IntPtr handleClone);

            [DllImport(Libraries.CapabilityManager, EntryPoint = "capmgr_app_info_get_appid")]
            internal static extern ErrorCode GetAppId(IntPtr handle, out string appId);

            [DllImport(Libraries.CapabilityManager, EntryPoint = "capmgr_app_info_get_pkgid")]
            internal static extern ErrorCode GetPkgId(IntPtr handle, out string pkgId);

            [DllImport(Libraries.CapabilityManager, EntryPoint = "capmgr_app_info_get_label")]
            internal static extern ErrorCode GetLabel(IntPtr handle, out string label);

            [DllImport(Libraries.CapabilityManager, EntryPoint = "capmgr_app_info_get_version")]
            internal static extern ErrorCode GetVersion(IntPtr handle, out string version);

            [DllImport(Libraries.CapabilityManager, EntryPoint = "capmgr_app_info_get_device")]
            internal static extern ErrorCode GetDevice(IntPtr handle, out IntPtr deviceHandle);

            [DllImport(Libraries.CapabilityManager, EntryPoint = "capmgr_app_info_destroy")]
            internal static extern ErrorCode Destroy(IntPtr handle);
        }
        
    }

}