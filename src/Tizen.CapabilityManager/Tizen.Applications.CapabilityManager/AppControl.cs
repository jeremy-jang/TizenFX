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
    /// Represents the control message to exchange between applications of remote device.
    /// </summary>
    /// <since_tizen> 6 </since_tizen>
    public class AppControl
    {
        private const string LogTag = "Tizen.CapabilityManager";

        internal IntPtr _handle = IntPtr.Zero;

        private string _operation = string.Empty;
        private string _mime = string.Empty;
        private string _uri = string.Empty;
        private ExtraDataCollection _extraData = null;

        internal AppControl(IntPtr handle)
        {
            IntPtr clone = IntPtr.Zero;

            Interop.CapabilityManager.ErrorCode err = Interop.CapabilityManager.AppControl.Clone(handle, out clone);
            if (err != Interop.CapabilityManager.ErrorCode.None)
            {
                throw new OutOfMemoryException();
            }

            _handle = clone;
        }

        /// <summary>
        /// Initializes the instance of the AppControl class.
        /// </summary>
        /// <exception cref="OutOfMemoryException">Thrown when there is not enough memory to continue the execution of the method.</exception>
        /// <since_tizen> 6 </since_tizen>
        public AppControl()
        {
            IntPtr handle = IntPtr.Zero;

            Interop.CapabilityManager.ErrorCode err = Interop.CapabilityManager.AppControl.Create(out handle);
            if (err != Interop.CapabilityManager.ErrorCode.None)
            {
                throw new OutOfMemoryException();
            }

            _handle = handle;
        }


        /// <summary>
        /// Gets and sets the operation to be performed.
        /// </summary>
        /// <value>
        /// The operation is the mandatory information for the launch request. If the operation is not specified,
        /// AppControlOperations.Default is used for the launch request. If the operation is AppControlOperations.Default,
        /// the package information is mandatory to explicitly launch the application.
        /// (if the operation is null for setter, it clears the previous value.)
        /// </value>
        /// <example>
        /// <code>
        /// AppControl appControl = new AppControl();
        /// appControl.Operation = AppControlOperations.Default;
        /// Log.Debug(LogTag, "Operation: " + appControl.Operation);
        /// </code>
        /// </example>
        /// <since_tizen> 6 </since_tizen>
        public string Operation
        {
            get
            {
                if (String.IsNullOrEmpty(_operation))
                {
                    Interop.CapabilityManager.ErrorCode err = Interop.CapabilityManager.AppControl.GetOperation(_handle, out _operation);
                    if (err != Interop.CapabilityManager.ErrorCode.None)
                    {
                        Log.Warn(LogTag, "Failed to get the operation from the appcontrol. Err = " + err);
                    }
                }
                return _operation;
            }
            set
            {
                Interop.CapabilityManager.ErrorCode err = Interop.CapabilityManager.AppControl.SetOperation(_handle, value);
                if (err == Interop.CapabilityManager.ErrorCode.None)
                {
                    _operation = value;
                }
                else
                {
                    Log.Warn(LogTag, "Failed to set the operation to the appcontrol. Err = " + err);
                }
            }
        }

        /// <summary>
        /// Gets and sets the explicit MIME type of the data.
        /// </summary>
        /// <value>
        /// (if the mime is null for setter, it clears the previous value.)
        /// </value>
        /// <example>
        /// <code>
        /// AppControl appControl = new AppControl();
        /// appControl.Mime = "image/jpg";
        /// Log.Debug(LogTag, "Mime: " + appControl.Mime);
        /// </code>
        /// </example>
        /// <since_tizen> 6 </since_tizen>
        public string Mime
        {
            get
            {
                if (String.IsNullOrEmpty(_mime))
                {
                    Interop.CapabilityManager.ErrorCode err = Interop.CapabilityManager.AppControl.GetMime(_handle, out _mime);
                    if (err != Interop.CapabilityManager.ErrorCode.None)
                    {
                        Log.Warn(LogTag, "Failed to get the mime from the appcontrol. Err = " + err);
                    }
                }
                return _mime;
            }
            set
            {
                Interop.CapabilityManager.ErrorCode err = Interop.CapabilityManager.AppControl.SetMime(_handle, value);
                if (err == Interop.CapabilityManager.ErrorCode.None)
                {
                    _mime = value;
                }
                else
                {
                    Log.Warn(LogTag, "Failed to set the mime to the appcontrol. Err = " + err);
                }
            }
        }

        /// <summary>
        /// Gets and sets the URI of the data.
        /// </summary>
        /// <value>
        /// Since Tizen 2.4, if the parameter 'uri' is started with 'file://' and
        /// it is a regular file in this application's data path, which can be obtained
        /// by property DataPath in ApplicationInfo class,
        /// it will be shared to the callee application.
        /// Framework will grant a temporary permission to the callee application for this file and
        /// revoke it when the callee application is terminated.
        /// The callee application can just read it.
        /// (if the uri is null for setter, it clears the previous value.)
        /// </value>
        /// <example>
        /// <code>
        /// public class AppControlExample : UIApplication
        /// {
        ///     ...
        ///     protected override void OnAppControlReceived(AppControlReceivedEventArgs e)
        ///     {
        ///         ...
        ///         AppControl appControl = new AppControl();
        ///         appContrl.Uri = this.ApplicationInfo.DataPath + "image.jpg";
        ///         Log.Debug(LogTag, "Set Uri: " + appControl.Uri);
        ///     }
        /// }
        /// </code>
        /// </example>
        /// <since_tizen> 6 </since_tizen>
        public string Uri
        {
            get
            {
                if (String.IsNullOrEmpty(_uri))
                {
                    Interop.CapabilityManager.ErrorCode err = Interop.CapabilityManager.AppControl.GetUri(_handle, out _uri);
                    if (err != Interop.CapabilityManager.ErrorCode.None)
                    {
                        Log.Warn(LogTag, "Failed to get the uri from the appcontrol. Err = " + err);
                    }
                }
                return _uri;
            }
            set
            {
                Interop.CapabilityManager.ErrorCode err = Interop.CapabilityManager.AppControl.SetUri(_handle, value);
                if (err == Interop.CapabilityManager.ErrorCode.None)
                {
                    _uri = value;
                }
                else
                {
                    Log.Warn(LogTag, "Failed to set the uri to the appcontrol. Err = " + err);
                }
            }
        }

        /// <summary>
        /// Gets the collection of the extra data.
        /// </summary>
        /// <value>
        /// Extra data for communication between AppControls.
        /// </value>
        /// <example>
        /// <code>
        /// AppControl appControl = new AppControl();
        /// appControl.ExtraData.Add("key", "value");
        /// ...
        /// </code>
        /// </example>
        /// <since_tizen> 6 </since_tizen>
        public ExtraDataCollection ExtraData
        {
            get
            {
                if (_extraData == null)
                    _extraData = new ExtraDataCollection(_handle);
                return _extraData;
            }
        }

        /// <summary>
        /// Class for extra data.
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
        public class ExtraDataCollection
        {
            private readonly IntPtr _handle;

            internal ExtraDataCollection(IntPtr handle)
            {
                _handle = handle;
            }

            /// <summary>
            /// Adds extra data.
            /// </summary>
            /// <remarks>
            /// The function replaces any existing value for the given key.
            /// </remarks>
            /// <param name="key">The name of the extra data.</param>
            /// <param name="value">The value associated with the given key.</param>
            /// <exception cref="ArgumentNullException">Thrown when a key or a value is a zero-length string.</exception>
            /// <exception cref="ArgumentException">Thrown when the application tries to use the same key with the system-defined key.</exception>
            /// <exception cref="OutOfMemoryException">Thrown when there is not enough memory to continue the execution of the method.</exception>
            /// <example>
            /// <code>
            /// AppControl appControl = new AppControl();
            /// appControl.ExtraData.Add("myKey", "myValue");
            /// </code>
            /// </example>
            /// <since_tizen> 6 </since_tizen>
            public void Add(string key, string value)
            {
                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentNullException("key");
                }
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("value");
                }
                Interop.CapabilityManager.ErrorCode err = Interop.CapabilityManager.AppControl.AddExtraData(_handle, key, value);
                if (err != Interop.CapabilityManager.ErrorCode.None)
                {
                    switch (err)
                    {
                        case Interop.CapabilityManager.ErrorCode.InvalidParameter:
                            throw new ArgumentException("Invalid parameter: key or value is a zero-length string");
                        //case Interop.CapabilityManager.ErrorCode.KeyRejected:
                        //    throw new ArgumentException("Key is rejected: the key is system-defined key.");
                        case Interop.CapabilityManager.ErrorCode.OutOfMemory:
                            throw new OutOfMemoryException();
                        default:
                            throw new InvalidOperationException("Error = " + err);
                    }
                }
            }

            /// <summary>
            /// Gets the extra data.
            /// </summary>
            /// <typeparam name="T">Only string and IEnumerable&lt;string&gt;</typeparam>
            /// <param name="key">The name of extra data.</param>
            /// <returns>The value associated with the given key.</returns>
            /// <exception cref="ArgumentNullException">Thrown when the key is an invalid parameter.</exception>
            /// <exception cref="KeyNotFoundException">Thrown when the key is not found.</exception>
            /// <exception cref="ArgumentException">Thrown when the key is rejected.</exception>
            /// <example>
            /// <code>
            /// AppControl appControl = new AppControl();
            /// string myValue = appControl.ExtraData.Get&lt;string&gt;("myKey");
            /// </code>
            /// </example>
            /// <since_tizen> 6 </since_tizen>
            public T Get<T>(string key)
            {
                object ret = Get(key);
                return (T)ret;
            }

            /// <summary>
            /// Gets the extra data.
            /// </summary>
            /// <param name="key">The name of extra data.</param>
            /// <returns>The value associated with the given key.</returns>
            /// <exception cref="ArgumentNullException">Thrown when the key is an invalid parameter.</exception>
            /// <exception cref="KeyNotFoundException">Thrown when the key is not found.</exception>
            /// <exception cref="ArgumentException">Thrown when the key is rejected.</exception>
            /// <example>
            /// <code>
            /// AppControl appControl = new AppControl();
            /// string myValue = appControl.ExtraData.Get("myKey") as string;
            /// if (myValue != null)
            /// {
            ///     // ...
            /// }
            /// </code>
            /// </example>
            /// <since_tizen> 6 </since_tizen>
            public object Get(string key)
            {
                return GetData(key);
            }

            /// <summary>
            /// Tries getting the extra data.
            /// </summary>
            /// <param name="key">The name of extra data.</param>
            /// <param name="value">The value associated with the given key.</param>
            /// <returns>The result whether getting the value is done.</returns>
            /// <exception cref="ArgumentNullException">Thrown when the key is an invalid parameter.</exception>
            /// <exception cref="KeyNotFoundException">Thrown when the key is not found.</exception>
            /// <exception cref="ArgumentException">Thrown when the key is rejected.</exception>
            /// <example>
            /// <code>
            /// AppControl appControl = new AppControl();
            /// string myValue = string.Empty;
            /// bool result = appControl.ExtraData.TryGet("myKey", out myValue);
            /// if (result != null)
            /// {
            ///     // ...
            /// }
            /// </code>
            /// </example>
            /// <since_tizen> 6 </since_tizen>
            public bool TryGet(string key, out string value)
            {
                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentNullException("key");
                }
                Interop.CapabilityManager.AppControl.GetExtraData(_handle, key, out value);
                if (value != null)
                {
                    return true;
                }
                else
                {
                    value = default(string);
                    return false;
                }
            }

            /// <summary>
            /// Removes the extra data.
            /// </summary>
            /// <param name="key">The name of the extra data.</param>
            /// <exception cref="ArgumentNullException">Thrown when the key is a zero-length string.</exception>
            /// <exception cref="KeyNotFoundException">Thrown when the key is not found.</exception>
            /// <exception cref="ArgumentException">Thrown when the key is rejected.</exception>
            /// <example>
            /// <code>
            /// AppControl appControl = new AppControl();
            /// appControl.ExtraData.Remove("myKey");
            /// </code>
            /// </example>
            /// <since_tizen> 6 </since_tizen>
            public void Remove(string key)
            {
                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentNullException("key");
                }
                Interop.CapabilityManager.ErrorCode err = Interop.CapabilityManager.AppControl.RemoveExtraData(_handle, key);
                if (err != Interop.CapabilityManager.ErrorCode.None)
                {
                    switch (err)
                    {
                        case Interop.CapabilityManager.ErrorCode.InvalidParameter:
                            throw new ArgumentException("Invalid parameter: key is a zero-length string");
                        //case Interop.AppControl.ErrorCode.KeyNotFound:
                        //    throw new KeyNotFoundException("Key is not found"); ;
                        //case Interop.AppControl.ErrorCode.KeyRejected:
                        //    throw new ArgumentException("Key is rejected: the key is system-defined key.");
                        default:
                            throw new InvalidOperationException("Error = " + err);
                    }
                }
            }

            private string GetData(string key)
            {
                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentNullException("key");
                }
                string value = string.Empty;
                Interop.CapabilityManager.ErrorCode err = Interop.CapabilityManager.AppControl.GetExtraData(_handle, key, out value);
                if (err != Interop.CapabilityManager.ErrorCode.None)
                {
                    switch (err)
                    {
                        case Interop.CapabilityManager.ErrorCode.InvalidParameter:
                            throw new ArgumentException("Invalid parameter: key is a zero-length string");
                        //case Interop.CapabilityManager.ErrorCode.KeyNotFound:
                        //    throw new KeyNotFoundException("Key is not found"); ;
                        default:
                            throw new InvalidOperationException("Error = " + err);
                    }
                }
                return value;
            }
        }
    }
}
