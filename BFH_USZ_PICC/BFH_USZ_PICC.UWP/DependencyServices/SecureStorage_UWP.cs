﻿using BFH_USZ_PICC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Security.Cryptography.DataProtection;
using Windows.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(BFH_USZ_PICC.UWP.DependencyServices.SecureStorage_UWP))]

namespace BFH_USZ_PICC.UWP.DependencyServices
{
    /// <summary>
    /// Implements <see cref="ISecureStorage"/> for WP using <see cref="IsolatedStorageFile"/> and <see cref="ProtectedData"/>.
    /// </summary>
    public class SecureStorage_UWP : ISecureStorage
    {
        private static ApplicationData AppStorage { get { return ApplicationData.Current; } }

        private static DataProtectionProvider _dataProtectionProvider = new DataProtectionProvider();

        private readonly byte[] _optionalEntropy;

        /// <summary>
        /// Initializes a new instance of <see cref="SecureStorage"/>.
        /// </summary>
        /// <param name="optionalEntropy">Optional password for additional entropy to make encyption more complex.</param>
        public SecureStorage_UWP(byte[] optionalEntropy)
        {
            this._optionalEntropy = optionalEntropy;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SecureStorage"/>.
        /// </summary>
        public SecureStorage_UWP() : this(null)
        {

        }

        #region ISecureStorage Members

        /// <summary>
        /// Stores the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="dataBytes">The data bytes.</param>
        public async void Store(string key, byte[] dataBytes)
        {
            var mutex = new Mutex(false, key);

            try
            {
                mutex.WaitOne();

                //var result = await new Windows.Security.Cryptography.DataProtection.DataProtectionProvider().ProtectAsync()

                var buffer = dataBytes.AsBuffer();

                if (_optionalEntropy != null)
                {
                    buffer = await _dataProtectionProvider.ProtectAsync(buffer);
                }

                var file =
                    await AppStorage.LocalFolder.CreateFileAsync(key, CreationCollisionOption.ReplaceExisting);

                await FileIO.WriteBufferAsync(file, buffer);
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Retrieves the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.Byte[].</returns>
        /// <exception cref="System.Exception"></exception>
        public byte[] Retrieve(string key)
        {
            var mutex = new Mutex(false, key);

            try
            {
                mutex.WaitOne();

                var file = AppStorage.LocalFolder.GetFileAsync(key);

                var bufferTask = FileIO.ReadBufferAsync(file.GetResults());

                var buffer = bufferTask.GetResults();

                if (_optionalEntropy != null)
                {
                    buffer = _dataProtectionProvider.UnprotectAsync(buffer).GetResults();
                }

                return buffer.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("No entry found for key {0}.", key), ex);
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Deletes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public async void Delete(string key)
        {
            var mutex = new Mutex(false, key);

            try
            {
                mutex.WaitOne();

                var file = await AppStorage.LocalFolder.GetFileAsync(key);

                await file.DeleteAsync();
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Checks if the storage contains a key.
        /// </summary>
        /// <param name="key">The key to search.</param>
        /// <returns>True if the storage has the key, otherwise false.</returns>
        public bool Contains(string key)
        {
            try
            {
                var file = AppStorage.LocalFolder.GetFileAsync(key);
                file.GetResults();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void WipeStorage()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
