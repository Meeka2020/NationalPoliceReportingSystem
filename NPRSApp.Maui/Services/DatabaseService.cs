using NPRSApp.Maui.Model;
using SQLite;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using NPRSApp.Maui.Services.Interfaces;


namespace NPRSApp.Maui.Services
{
    // =========================================================
    // 📌 DATABASE SERVICE
    // This file handles ALL database operations for Police Reports
    // Think of it as the "middleman" between the app and SQLite database
    // =========================================================

    public class DatabaseService : IDatabaseService
    {
        // 📌 This is the connection to our SQLite database
        private readonly SQLiteAsyncConnection _database;

        // 📌 This prevents us from creating tables more than once
        private bool _isInitialized = false;

        // =========================================================
        // 📌 CONSTRUCTOR (runs first when app starts this service)
        // =========================================================
        public DatabaseService()
        {
            // 🔧 Initializes SQLite engine for mobile devices
            SQLitePCL.Batteries_V2.Init();

            // 📌 We create the database file inside app storage
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "PoliceReports.db");

            // 📌 Connect to the SQLite database file
            _database = new SQLiteAsyncConnection(dbPath);
        }

        // =========================================================
        // 📌 INITIALIZE DATABASE (runs only once)
        // Creates tables if they do not exist
        // =========================================================
        public async Task InitializeAsync()
        {
            if (_isInitialized)
                return; // Stop if already created

            // 📌 Create PoliceReport table inside database
            await _database.CreateTableAsync<PoliceReport>();

            _isInitialized = true;
        }

        // =========================================================
        // 📌 GET ALL REPORTS
        // This gets everything stored in the PoliceReport table
        // =========================================================
        public async Task<List<PoliceReport>> GetReportsAsync()
        {
            await InitializeAsync();

            return await _database.Table<PoliceReport>().ToListAsync();
        }

        // =========================================================
        // 📌 GET SINGLE REPORT BY ID
        // Used when user clicks "Edit" or "View Details"
        // =========================================================
        public async Task<PoliceReport> GetReportByIdAsync(int id)
        {
            await InitializeAsync();

            return await _database.Table<PoliceReport>()
                                  .Where(r => r.Id == id)
                                  .FirstOrDefaultAsync();
        }

        // =========================================================
        // 📌 ADD NEW REPORT
        // Saves a new police report into the database
        // =========================================================
        public async Task<int> AddReportAsync(PoliceReport report)
        {
            await InitializeAsync();

            return await _database.InsertAsync(report);
        }

        // =========================================================
        // 📌 UPDATE EXISTING REPORT
        // Used when user edits a saved report
        // =========================================================
        public async Task<int> UpdateReportAsync(PoliceReport report)
        {
            await InitializeAsync();

            return await _database.UpdateAsync(report);
        }

        // =========================================================
        // 📌 DELETE ONE REPORT
        // Removes a specific report from database
        // =========================================================
        public async Task<int> DeleteReportAsync(PoliceReport report)
        {
            await InitializeAsync();

            return await _database.DeleteAsync(report);
        }

        // =========================================================
        // 📌 DELETE ALL REPORTS (RESET FUNCTION)
        // ⚠️ Used for testing only - clears entire table
        // =========================================================
        public async Task<int> DeleteAllAsync()
        {
            await InitializeAsync();

            return await _database.DeleteAllAsync<PoliceReport>();
        }
    }
}