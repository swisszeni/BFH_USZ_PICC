﻿using BFH_USZ_PICC.Interfaces;
using Foundation;
using Security;
using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(BFH_USZ_PICC.iOS.DependencyServices.SecureStorage_iOS))]

namespace BFH_USZ_PICC.iOS.DependencyServices
{
    /// <summary>
    /// Implements <see cref="ISecureStorage"/> for iOS using <see cref="SecKeyChain"/>.
    /// </summary>
    public class SecureStorage_iOS : ISecureStorage
    {
        #region ISecureStorage Members

        /// <summary>
        /// Stores data.
        /// </summary>
        /// <param name="key">Key for the data.</param>
        /// <param name="dataBytes">Data bytes to store.</param>
        public void Store(string key, byte[] dataBytes)
        {
            using (var data = NSData.FromArray(dataBytes))
            using (var newRecord = GetKeyRecord(key, data))
            {
                Delete(key);
                CheckError(SecKeyChain.Add(newRecord));
            }
        }

        /// <summary>
        /// Retrieves stored data.
        /// </summary>
        /// <param name="key">Key for the data.</param>
        /// <returns>Byte array of stored data.</returns>
        public byte[] Retrieve(string key)
        {
            SecStatusCode resultCode;

            using (var existingRecord = GetKeyRecord(key))
            using (var record = SecKeyChain.QueryAsRecord(existingRecord, out resultCode))
            {
                CheckError(resultCode);
                return record.Generic.ToArray();
            }
        }

        /// <summary>
        /// Deletes data.
        /// </summary>
        /// <param name="key">Key for the data to be deleted.</param>
        public void Delete(string key)
        {
            using (var record = GetExistingRecord(key))
            {
                if (record != null) CheckError(SecKeyChain.Remove(record));
            }
        }

        /// <summary>
        /// Deletes all saved data.
        /// If there is a master key used for storage, it is deleted too
        /// </summary>
        public void WipeStorage()
        {
            // Iterate over all possible kinds of record type to erase them
            foreach (var recordKind in new[] {
                SecKind.GenericPassword,
                SecKind.Certificate,
                SecKind.Identity,
                SecKind.InternetPassword,
                SecKind.Key
            })
            {
                SecRecord query = new SecRecord(recordKind);
                SecKeyChain.Remove(query);
            }
        }

        /// <summary>
        /// Checks if the storage contains a key.
        /// </summary>
        /// <param name="key">The key to search.</param>
        /// <returns>True if the storage has the key, otherwise false.</returns>
        public bool Contains(string key)
        {
            using (var existingRecord = GetExistingRecord(key))
            {
                return existingRecord != null;
            }
        }

        #endregion

        #region private static methods

        private static void CheckError(SecStatusCode resultCode, [System.Runtime.CompilerServices.CallerMemberName] string caller = null)
        {
            if (resultCode != SecStatusCode.Success)
            {
                throw new Exception(string.Format("Failed to execute {0}. Result code: {1}", caller, resultCode));
            }
        }

        private static SecRecord GetKeyRecord(string key, NSData data = null)
        {
            var record = new SecRecord(SecKind.GenericPassword)
            {
                Service = NSBundle.MainBundle.BundleIdentifier,
                Account = key
            };

            if (data != null) record.Generic = data;

            return record;
        }

        private static SecRecord GetExistingRecord(string key)
        {
            var existingRecord = GetKeyRecord(key);

            SecStatusCode resultCode;
            SecKeyChain.QueryAsRecord(existingRecord, out resultCode);

            return resultCode == SecStatusCode.Success ? existingRecord : null;
        }

        #endregion
    }
}
