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
    public class FilePayload : IPayload
    {
        internal FilePayload(PayloadSafeHandle handle)
        {
            _handle = handle;
        }

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public string ReceivedFileName
        {
            get
            {
                Interop.Cion.ErrorCode ret = Interop.CionPayload.CionPayloadGetReceivedFileName(_handle, out string path);
                if (ret != Interop.Cion.ErrorCode.None)
                {
                    // error handling ?? property should not throw exception.
                    return "";
                }
                return path;
            }
        }

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public long ReceivedBytes
        { 
            get
            {
                Interop.Cion.ErrorCode ret = Interop.CionPayload.CionPayloadGetReceivedBytes(_handle, out long bytes);
                if (ret != Interop.Cion.ErrorCode.None)
                {
                    // error handling ?? property should not throw exception.
                    return Byte.MinValue;
                }
                return bytes;
            }
        }

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public long TotalBytes 
        {
            get
            {
                Interop.Cion.ErrorCode ret = Interop.CionPayload.CionPayloadGetTotalBytes(_handle, out long bytes);
                if (ret != Interop.Cion.ErrorCode.None)
                {
                    // error handling ?? property should not throw exception.
                    return Byte.MinValue;
                }
                return bytes;
            }
        }

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public override PayloadType GetPayloadType()
        {
            return PayloadType.FilePayload;
        }

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public void SaveAsFile(string path)
        {
            Interop.Cion.ErrorCode ret = Interop.CionPayload.CionPayloadSaveAsFile(_handle, path);
            if (ret != Interop.Cion.ErrorCode.None)
            {
                throw CionErrorFactory.GetException(ret, "Failed to save as file.");
            }
        }

        /// <summary>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public void SetFilePath(string path)
        {
            Interop.Cion.ErrorCode ret = Interop.CionPayload.CionPayloadSetFilePath(_handle, path);
            if (ret != Interop.Cion.ErrorCode.None)
            {
                throw CionErrorFactory.GetException(ret, "Failed to set file path.");
            }
        }
    }
}
