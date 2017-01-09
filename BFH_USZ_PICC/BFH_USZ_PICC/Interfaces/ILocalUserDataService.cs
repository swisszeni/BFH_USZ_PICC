﻿using BFH_USZ_PICC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFH_USZ_PICC.Interfaces
{
    public interface ILocalUserDataService
    {
        Task ResetLocalUserDataAsync();

        // MasterData
        Task<List<UserMasterData>> GetMasterDataAsync();
        Task<int> SaveMasterDataAsync(UserMasterData masterData);
        Task<int> DeleteAllMasterDataAsync();
        Task<int> DeleteMasterDataAsync(UserMasterData masterData);

        // JournalEntry
        Task<List<JournalEntry>> GetJournalEntriesAsync();
        Task<List<T>> GetJournalEntriesAsync<T>() where T : JournalEntry;
        Task<T> GetJournalEntryAsync<T>(string ID) where T : JournalEntry;
        Task<int> SaveJournalEntryAsync<T>(T entry) where T : JournalEntry;
        Task<int> DeleteJournalEntryAsync<T>(T entry) where T : JournalEntry;

        // PICC
        Task<int> DeltePICCAsync(PICC picc);
        Task<List<PICC>> GetFormerPICCsAsync();
        Task<PICC> GetCurrentPICCAsync();
        Task<int> SaveCurrentPICCAsync(PICC currentPICC);
        
    }
}
