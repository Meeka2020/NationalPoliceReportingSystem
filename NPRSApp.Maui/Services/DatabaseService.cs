using NPRSApp.Maui.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace NPRSApp.Maui.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;
        private bool _isInitialized = false;

        public DatabaseService()
        {
            // Initialize SQLite
            SQLitePCL.Batteries_V2.Init();

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "PoliceReports.db");
            _database = new SQLiteAsyncConnection(dbPath);
        }

        // ✅ Ensure DB is ready before any operation
        private async Task InitializeAsync()
        {
            if (_isInitialized)
                return;

            await _database.CreateTableAsync<PoliceReport>();
            _isInitialized = true;
        }

        // ✅ GET ALL
        public async Task<List<PoliceReport>> GetReportsAsync()
        {
            await InitializeAsync();
            return await _database.Table<PoliceReport>().ToListAsync();
        }

        // ✅ GET BY ID (important for Edit feature)
        public async Task<PoliceReport> GetReportByIdAsync(int id)
        {
            await InitializeAsync();
            return await _database.Table<PoliceReport>()
                                  .Where(r => r.Id == id)
                                  .FirstOrDefaultAsync();
        }

        // ✅ CREATE
        public async Task<int> AddReportAsync(PoliceReport report)
        {
            await InitializeAsync();
            return await _database.InsertAsync(report);
        }

        // ✅ UPDATE
        public async Task<int> UpdateReportAsync(PoliceReport report)
        {
            await InitializeAsync();
            return await _database.UpdateAsync(report);
        }

        // ✅ DELETE
        public async Task<int> DeleteReportAsync(PoliceReport report)
        {
            await InitializeAsync();
            return await _database.DeleteAsync(report);
        }

        // ✅ OPTIONAL (for testing/reset)
        public async Task<int> DeleteAllAsync()
        {
            await InitializeAsync();
            return await _database.DeleteAllAsync<PoliceReport>();
        }
    }
}