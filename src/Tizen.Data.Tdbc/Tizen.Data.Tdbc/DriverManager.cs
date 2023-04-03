﻿/*
 * Copyright (c) 2023 Samsung Electronics Co., Ltd All Rights Reserved
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
using System.Reflection;

namespace Tizen.Data.Tdbc
{
    /// <summary>
    /// DriverManager loads TDBC drivers and gets connections of databases.
    /// </summary>
    /// <since_tizen> 11 </since_tizen>
    public class DriverManager
    {
        private static string _driverName;
        private static Assembly _driverAssembly;

        /// <summary>
        /// Get connection of registered database and open the database.
        /// </summary>
        /// <param name="uri">The uri represents database to connect.</param>
        /// <returns>The connection object.</returns>
        /// <exception cref="InvalidOperationException">No driver registered.</exception>
        /// <exception cref="SystemException">Failed to open database connection.</exception>
        /// <since_tizen> 11 </since_tizen>
        public static Connection GetConnection(Uri uri)
        {
            if (_driverAssembly == null)
            {
                throw new InvalidOperationException("No TDBC driver registered.");
            }

            Connection conn;
            try
            {
                conn = (Connection)_driverAssembly.CreateInstance(_driverName + ".Connection");
            }
            catch (Exception ex)
            {
                throw new SystemException("Failed to open connection due to: " + ex);
            }

            conn.Open(uri);
            return conn;
        }

        /// <summary>
        /// Get connection of registered database and open the database.
        /// </summary>
        /// <param name="openStr">The string for connect and open database.</param>
        /// <returns>The connection object.</returns>
        /// <exception cref="InvalidOperationException">No driver registered.</exception>
        /// <exception cref="SystemException">Failed to open database connection.</exception>
        /// <since_tizen> 11 </since_tizen>
        public static Connection GetConnection(String openStr)
        {
            if (_driverAssembly == null)
                throw new InvalidOperationException("No TDBC driver registered.");

            Connection conn;
            try
            {
                conn = (Connection)_driverAssembly.CreateInstance(_driverName + ".Connection");
            }
            catch (Exception ex)
            {
                throw new SystemException("Failed to open connection due to: " + ex);
            }

            conn.Open(openStr);
            return conn;
        }

        /// <summary>
        /// Registers and loads the TDBC driver with the given driver name.
        /// </summary>
        /// <param name="name">The name of TDBC driver.</param>
        /// <exception cref="ArgumentNullException">The given name is null.</exception>
        /// <exception cref="ArgumentException">The given name is zero-length string.</exception>
        /// <exception cref="FileNotFoundException">The given driver name is not found.</exception>
        /// <exception cref="FileLoadException">The given driver could not be loaded.</exception>
        /// <exception cref="BadImageFormatException">The given driver is not a valid assembly.</exception>
        /// <since_tizen> 11 </since_tizen>
        public static void RegisterDriver(string name)
        {
            _driverName = name;
            _driverAssembly = Assembly.Load(_driverName);
        }
    }
}
