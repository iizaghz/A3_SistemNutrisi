using System;

namespace SistemNutrisi
{
    // Class Data sesuai Langkah 8 Modul Praktikum untuk binding Crystal Report
    public class RekapNutrisiData
    {
        public DateTime tanggal { get; set; }
        public string nama_makanan { get; set; }
        public int jumlah { get; set; }
        public double total_kalori { get; set; }
        public double total_protein { get; set; }
        public double total_lemak { get; set; }
        public double total_karbohidrat { get; set; }
    }
}
