# Log Update Fitur Aplikasi SistemNutrisi (Nutri Life) - UCP 3

Dokumen ini berisi catatan perubahan rinci mengenai penambahan fitur baru pada aplikasi **SistemNutrisi (Nutri Life)** untuk memenuhi persyaratan UCP 3. Dokumen ini juga menjelaskan dasar teori dan alasan pemilihan arsitektur serta tabel basis data yang digunakan.

---

## 1. Daftar Komponen yang Bertambah (Added Features)

Berikut adalah daftar file, basis data, dan antarmuka (UI) yang ditambahkan atau diperbarui dalam proyek SistemNutrisi:

### A. Komponen Database (SQL Server)
1. **Tabel `LogKonsumsi`**:
   * Tabel penampung riwayat aktivitas transaksi yang bertindak sebagai *audit trail*.
   * Menyimpan informasi: `id_log`, `id_konsumsi`, `action_type` (INSERT/UPDATE/DELETE), `id_user`, `id_makanan`, `tanggal_konsumsi`, `jumlah_sebelumnya`, `jumlah_baru`, `waktu_log`, dan `user_database`.
2. **Trigger `trg_LogKonsumsi`**:
   * Dibuat pada tabel `KonsumsiMakanan`.
   * Otomatis memproses log riwayat aktivitas secara real-time setelah terjadi proses manipulasi data (AFTER INSERT, UPDATE, DELETE).
3. **Stored Procedure `sp_GetDashboardSummary`**:
   * Prosedur untuk mengambil asupan gizi harian pengguna (Kalori, Protein, Lemak, Karbohidrat) beserta batas target harian default (2000.0 kcal kalori, 60.0g protein, 70.0g lemak, 300.0g karbohidrat).
4. **Pembaruan Stored Procedure `sp_DeleteNutrisi`**:
   * Ditambahkan logika cerdas pembersihan makanan (*garbage collection*) agar makanan yang dihapus nutrisinya ikut terhapus dari tabel `Makanan` apabila makanan tersebut belum pernah tercatat dalam transaksi makan user.

### B. Komponen Kode Program (C# WinForms)
1. **`FormDashboard.cs` & `FormDashboard.Designer.cs`**:
   * Halaman visualisasi data yang menampilkan ringkasan gizi harian secara real-time.
   * Menggunakan **GDI+** untuk menggambar *Pie Chart* komposisi makronutrisi (Protein, Lemak, Karbohidrat) serta panel berbayang (*shadow panels/cards*).
2. **`FormRekap.cs` & `FormRekap.Designer.cs`**:
   * Halaman untuk menyaring data riwayat konsumsi makanan berdasarkan rentang tanggal (*Date Range Picker*).
   * Menampilkan hasil pencarian di dalam `DataGridView`.
3. **`FormCetak.cs` & `FormCetak.Designer.cs`**:
   * Halaman cetak laporan (*Report Viewer*) yang terintegrasi dengan komponen `PrintPreviewControl` dan `PrintDocument`.
   * Dilengkapi fitur untuk mencetak fisik, mencetak ke format **PDF** (*Microsoft Print to PDF*), serta mengekspor data mentah ke format **CSV**.
4. **Modifikasi `FormUser.cs` & `FormUser.Designer.cs`**:
   * Menambahkan tombol menu **"Dashboard"** pada panel navigasi kiri.
   * Mengatur `FormDashboard` agar tampil secara default saat form user dimuat pertama kali.
5. **Modifikasi `FormRiwayat.cs` & `FormRiwayat.Designer.cs`**:
   * Menambahkan tombol **"Rekap & Cetak Laporan"** pada menu utama riwayat.
   * Mengintegrasikan tombol tersebut di dalam navigasi kontrol `BindingNavigator` agar tetap dapat diakses saat form dibuka dalam mode *embedded* (child control).

---

## 2. Alasan Pemilihan Tabel Transaksi (`KonsumsiMakanan`) sebagai Fokus Utama

Dalam pengembangan UCP 3, tabel **`KonsumsiMakanan`** dipilih sebagai objek audit trail (trigger), agregasi data dashboard, serta pembuatan laporan rekapitulasi. Berikut adalah analisis rincinya:

### A. Karakteristik Data (Master vs. Transaksi)
Di dalam basis data `DBSistemNutrisi`, tabel-tabel dibagi menjadi dua kategori utama:
1. **Tabel Master** (`User`, `Makanan`, `Nutrisi`, `Kategori`): Datanya relatif statis dan hanya bertindak sebagai referensi. Jarang ada perubahan data harian di sini.
2. **Tabel Transaksi** (`KonsumsiMakanan`): Datanya sangat dinamis karena mencatat setiap kejadian atau aktivitas konsumsi makanan yang dilakukan oleh pengguna dari waktu ke waktu.

Karena UCP 3 berfokus pada **transaksi dan reporting**, tabel `KonsumsiMakanan` is satu-satunya tabel yang menyimpan sejarah aktivitas pengguna yang relevan untuk dianalisis dan dilaporkan.

### B. Rationale Pemilihan untuk Audit Trail (Trigger)
Trigger keamanan (`trg_LogKonsumsi`) wajib diletakkan pada tabel transaksi `KonsumsiMakanan` karena alasan berikut:
* **Faktor Risiko Keamanan (Risk Factor)**: Aktivitas manipulasi data seperti menambah porsi makan, mengubah tanggal makan, atau menghapus riwayat makan adalah aktivitas yang rentan terhadap manipulasi oleh pengguna (misal: menghapus rekap makan secara curang agar tidak ketahuan melebihi batas kalori harian).
* **Pelacakan Perubahan (Audit Trail)**: Dengan memasang trigger di tabel transaksi, sistem dapat merekam riwayat perubahan secara otomatis (siapa yang mengubah, kapan diubah, berapa porsi sebelum dan sesudahnya). Hal ini menjamin integritas data (data integrity) untuk laporan kesehatan.

### C. Rationale Pemilihan untuk Dashboard & Reporting
Laporan rekap dan visualisasi grafik hanya bisa dibuat dari tabel transaksi `KonsumsiMakanan` karena:
* **Penyimpanan Riwayat Konsumsi**: Tabel ini menyimpan relasi antara *siapa* (`id_user`) yang memakan *apa* (`id_makanan`), *kapan* (`tanggal`), dan *seberapa banyak* (`jumlah`).
* **Kalkulasi Nutrisi**: Melalui relasi tersebut, sistem dapat melakukan operasi **JOIN** ke tabel master `Makanan` dan `Nutrisi` untuk menghitung nilai asupan gizi yang sesungguhnya secara matematis:
  $$\text{Total Kalori} = \sum (\text{Nutrisi.Kalori} \times \text{KonsumsiMakanan.Jumlah})$$
* Tanpa tabel transaksi `KonsumsiMakanan`, aplikasi tidak memiliki data dasar historis untuk merender diagram lingkaran gizi di dashboard maupun menyusun tabel fisik pada cetakan kertas laporan.

---

## 3. Query-Query Penting & Penjelasannya

Berikut adalah query-query kritis yang diimplementasikan di database SQL Server beserta analisis mendalam mengenai fungsi dan alasan perancangannya:

### A. Trigger Audit Trail (`trg_LogKonsumsi`)
```sql
CREATE OR ALTER TRIGGER trg_LogKonsumsi
ON KonsumsiMakanan
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Case INSERT
    IF EXISTS (SELECT * FROM inserted) AND NOT EXISTS (SELECT * FROM deleted)
    BEGIN
        INSERT INTO LogKonsumsi (id_konsumsi, action_type, id_user, id_makanan, tanggal_konsumsi, jumlah_sebelumnya, jumlah_baru)
        SELECT id_konsumsi, 'INSERT', id_user, id_makanan, tanggal, NULL, jumlah
        FROM inserted;
    END

    -- Case UPDATE
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
    BEGIN
        INSERT INTO LogKonsumsi (id_konsumsi, action_type, id_user, id_makanan, tanggal_konsumsi, jumlah_sebelumnya, jumlah_baru)
        SELECT i.id_konsumsi, 'UPDATE', i.id_user, i.id_makanan, i.tanggal, d.jumlah, i.jumlah
        FROM inserted i
        JOIN deleted d ON i.id_konsumsi = d.id_konsumsi;
    END

    -- Case DELETE
    IF NOT EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
    BEGIN
        INSERT INTO LogKonsumsi (id_konsumsi, action_type, id_user, id_makanan, tanggal_konsumsi, jumlah_sebelumnya, jumlah_baru)
        SELECT id_konsumsi, 'DELETE', id_user, id_makanan, tanggal, jumlah, NULL
        FROM deleted;
    END
END;
```
* **Fungsi**: Otomatis mendeteksi aksi manipulasi data pada tabel transaksi `KonsumsiMakanan` dan menulis riwayat perubahan tersebut ke dalam tabel audit `LogKonsumsi`.
* **Kenapa seperti ini?**: 
  * Menggunakan tabel virtual bawaan SQL Server yaitu `inserted` (menyimpan data baru yang masuk/diupdate) dan `deleted` (menyimpan data lama yang dihapus/digantikan).
  * Dengan melakukan pengecekan `EXISTS` secara silang terhadap kedua tabel tersebut, trigger dapat mengklasifikasikan aksi yang terjadi secara akurat (`INSERT`, `UPDATE`, atau `DELETE`).
  * Menyimpan status nilai porsi makanan sebelum (`jumlah_sebelumnya`) dan sesudah (`jumlah_baru`) perubahan, memudahkan administrator sistem untuk melacak kesalahan input porsi makan oleh user.

---

### B. Stored Procedure Ringkasan Dashboard (`sp_GetDashboardSummary`)
```sql
CREATE OR ALTER PROCEDURE sp_GetDashboardSummary
    @id_user INT,
    @tanggal DATE
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        ISNULL(SUM(n.kalori * k.jumlah), 0) AS total_kalori,
        ISNULL(SUM(n.protein * k.jumlah), 0) AS total_protein,
        ISNULL(SUM(n.lemak * k.jumlah), 0) AS total_lemak,
        ISNULL(SUM(n.karbohidrat * k.jumlah), 0) AS total_karbohidrat,
        2000.0 AS target_kalori,
        60.0 AS target_protein,
        70.0 AS target_lemak,
        300.0 AS target_karbohidrat
    FROM KonsumsiMakanan k
    JOIN Makanan m ON k.id_makanan = m.id_makanan
    LEFT JOIN Nutrisi n ON m.id_makanan = n.id_makanan
    WHERE k.id_user = @id_user AND k.tanggal = @tanggal;
END;
```
* **Fungsi**: Mengambil total akumulasi konsumsi nutrisi harian pengguna yang login berdasarkan tanggal tertentu, lengkap dengan batasan angka target harian.
* **Kenapa seperti ini?**:
  * Menggunakan fungsi agregasi `SUM` untuk mengalikan kandungan gizi per porsi dengan jumlah porsi makanan yang dikonsumsi (`n.kalori * k.jumlah`).
  * `ISNULL(..., 0)` digunakan agar jika user belum makan apa pun pada hari tersebut, query tidak menghasilkan nilai `NULL` melainkan angka `0`, mencegah terjadinya *crash* parsing angka di sisi frontend C#.
  * Menggabungkan (**JOIN**) tabel `KonsumsiMakanan` ke tabel master `Makanan` dan dilanjutkan `LEFT JOIN` ke `Nutrisi` untuk mengambil data gizi makanan tersebut secara real-time.

---

### C. Stored Procedure Penghapusan Nutrisi Cerdas (`sp_DeleteNutrisi`)
```sql
CREATE OR ALTER PROCEDURE sp_DeleteNutrisi
    @id_makanan INT
AS
BEGIN
    SET NOCOUNT ON;

    -- 1. Hapus data gizinya di tabel Nutrisi dahulu
    DELETE FROM Nutrisi WHERE id_makanan = @id_makanan;
    
    -- 2. Cek apakah makanan ini pernah dimakan oleh user (ada di transaksi KonsumsiMakanan)
    -- Jika TIDAK PERNAH digunakan di riwayat makan, hapus sekalian makanannya agar hilang sepenuhnya dari layar
    IF NOT EXISTS (SELECT 1 FROM KonsumsiMakanan WHERE id_makanan = @id_makanan)
    BEGIN
        DELETE FROM Makanan WHERE id_makanan = @id_makanan;
    END
END;
```
* **Fungsi**: Menghapus data nutrisi suatu makanan, dan secara otomatis membersihkan baris makanan tersebut dari tabel master `Makanan` jika makanan tersebut belum pernah tercatat dalam transaksi riwayat makan pengguna.
* **Kenapa seperti ini?**:
  * Menghindari masalah *"Baris Kosong Menggantung"* pada antarmuka *Kelola Nutrisi*. Tanpa langkah kedua, saat gizi dihapus, makanan tersebut akan tetap tertera di Grid sebagai baris kosong karena kueri Grid menggunakan `LEFT JOIN` ke tabel `Makanan`.
  * Menjaga integritas data transaksional: Sebelum menghapus nama makanan dari tabel master `Makanan`, stored procedure memastikan bahwa makanan tersebut tidak terikat dengan data transaksi `KonsumsiMakanan` (`NOT EXISTS (SELECT 1 FROM KonsumsiMakanan...)`). Jika terikat, nama makanan tidak dihapus agar riwayat makan user di masa lalu tidak hilang/error.

---

## 4. Penjelasan Peletakan Logika di T-SQL (Database) vs. Backend (C# Application)

Dalam arsitektur aplikasi **SistemNutrisi**, kami memilih untuk meletakkan logika operasional penting pada sisi **Database menggunakan T-SQL (dalam bentuk Trigger dan Stored Procedure)** ketimbang memprosesnya di sisi **Backend (Aplikasi C#)**. Berikut adalah alasan rasionalnya untuk menjawab pertanyaan ujian:

### A. Kenapa Audit Log Menggunakan T-SQL Trigger di Database?
* **Konsistensi Keamanan (Consistent Security)**: Jika logika pencatatan log audit ditaruh di backend C#, maka log audit hanya akan tercatat apabila modifikasi dilakukan lewat aplikasi C# tersebut. Apabila ada oknum/admin yang mengubah data langsung melalui *SQL Server Management Studio (SSMS)*, script Python, atau tool pihak ketiga, maka aktivitas mencurigakan tersebut tidak akan tercatat dalam log audit.
* **Sentralisasi Logika**: Dengan meletakkan trigger di tingkat tabel database (`KonsumsiMakanan`), mesin SQL Server akan **menjamin secara mutlak** bahwa setiap manipulasi data dari mana pun sumbernya akan selalu mencatat riwayat perubahan secara otomatis.

### B. Kenapa Perhitungan Gizi Dashboard Menggunakan T-SQL Stored Procedure?
* **Efisiensi Lalu Lintas Jaringan (Network Traffic Optimization)**: Jika perhitungan total gizi harian dilakukan di backend C#, aplikasi harus menarik semua data mentah baris konsumsi hari itu beserta data gizi makanan secara berulang, baru melakukan kalkulasi di memori C#. Hal ini memakan bandwidth dan RAM klien.
* **Performa Tinggi**: Stored Procedure memanfaatkan kemampuan mesin SQL Server untuk melakukan operasi JOIN dan agregasi matematika (`SUM`, `MULTIPLY`) secara langsung di server menggunakan memori SQL Server yang sudah terindeks. Hasil kalkulasi yang dikirim ke aplikasi C# hanyalah **1 baris data ringkasan** yang siap pakai.

### C. Kenapa Penghapusan Relasional Menggunakan T-SQL Stored Procedure?
* **Atomisitas Transaksi (Atomicity)**: Pada stored procedure `sp_DeleteNutrisi`, proses pengecekan relasi (`IF NOT EXISTS`) dan penghapusan multi-tabel (`Nutrisi` dan `Makanan`) dibungkus dalam satu kesatuan eksekusi di sisi database. Ini menjamin operasi bersifat atomik (sukses seluruhnya atau dibatalkan seluruhnya), mencegah kondisi inkonsistensi data jika terjadi putus koneksi jaringan di tengah proses penghapusan.
