using System;
using System.Data;
using System.Data.SqlClient;

namespace SistemNutrisi
{
    /// <summary>
    /// Data Access Layer - Centralized connection management for SistemNutrisi.
    /// Gunakan DAL.GetConnectionString() di semua form agar mudah diganti saat deploy.
    /// </summary>
    public static class DAL
    {
        public static string GetLocalIPAddress()
        {
            // Mengembalikan hostname server "IZAYAAA" agar client di jaringan lokal
            // dapat terhubung ke database server tanpa perlu tahu IP yang berubah-ubah.
            return "IZAYAAA";
        }

        public static string GetConnectionString()
        {
            string connectionString = $"Data Source={GetLocalIPAddress()}\\IZA;Initial Catalog=DBSistemNutrisi;User ID=mhs;Password=mhs123;";
            return connectionString;
        }
    }
}
