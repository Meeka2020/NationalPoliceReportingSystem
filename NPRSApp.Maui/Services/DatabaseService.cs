using NPRSApp.Maui.Model;
using NPRSApp.Maui.Services.Interfaces;
using SQLite;
using SQLitePCL;
using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace NPRSApp.Maui.Services
{
    // =========================================================
    // 📌 DATABASE SERVICE
    // Handles ALL SQLite operations for Police Reports
    // =========================================================
    public class DatabaseService 
    {
        // 📌 SQLite database connection
        private readonly SQLiteAsyncConnection _database;

        // 📌 Ensures tables are only created once
        private bool _isInitialized = false;

        // =========================================================
        // 📌 CONSTRUCTOR
        // =========================================================
        public DatabaseService()
        {
            // Required for Android/iOS SQLite
            Batteries_V2.Init();

            string dbPath = Path.Combine(
                FileSystem.AppDataDirectory,
                "PoliceReports.db");

            _database = new SQLiteAsyncConnection(dbPath);
        }

        // =========================================================
        // 📌 INITIALIZE DATABASE
        // =========================================================
        private async Task InitializeAsync()
        {
            if (_isInitialized)
                return;

            await _database.CreateTableAsync<PoliceReport>();
            _isInitialized = true;
        }

        // =========================================================
        // 📌 GET ALL REPORTS
        // =========================================================
        public async Task<List<PoliceReport>> GetReportsAsync()
        {
            await InitializeAsync();
            return await _database.Table<PoliceReport>().ToListAsync();
        }

        // =========================================================
        // 📌 GET REPORT BY ID
        // =========================================================
        public async Task<PoliceReport?> GetReportByIdAsync(int id)
        {
            await InitializeAsync();

            return await _database.Table<PoliceReport>()
                                  .Where(r => r.Id == id)
                                  .FirstOrDefaultAsync();
        }

        // =========================================================
        // 📌 ADD REPORT
        // =========================================================
        public async Task<int> AddReportAsync(PoliceReport report)
        {
            await InitializeAsync();
            return await _database.InsertAsync(report);
        }

        // =========================================================
        // 📌 UPDATE REPORT
        // =========================================================
        public async Task<int> UpdateReportAsync(PoliceReport report)
        {
            await InitializeAsync();
            return await _database.UpdateAsync(report);
        }

        // =========================================================
        // ✅ DELETE REPORT **BY ID** (ANDROID‑SAFE ✅)
        // =========================================================
        public async Task DeleteReportAsync(int id)
        {
            await InitializeAsync();
            await _database.DeleteAsync<PoliceReport>(id);
        }

        // =========================================================
        // 📌 DELETE ALL REPORTS (testing / reset)
        // =========================================================
        public async Task<int> DeleteAllAsync()
        {
            await InitializeAsync();
            return await _database.DeleteAllAsync<PoliceReport>();
        }
    }
}

