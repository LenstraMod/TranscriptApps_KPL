using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System;
using System.Windows.Forms;

namespace Frontend
{
    public partial class FormInputJadwal : Form
    {
        // Variabel penampung data utama
        public JadwalModel DataJadwal = new JadwalModel();

        public FormInputJadwal()
        {
            InitializeComponent();

            // Daftarkan event klik tombol Save dan tombol Pilih Gambar baru lewat constructor
            btnSave.Click += new System.EventHandler(this.btnSave_Click);
            btnPilihGambar.Click += new System.EventHandler(this.btnPilihGambar_Click);
        }

        private void FormInputJadwal_Load(object sender, EventArgs e)
        {
            txtHari.ReadOnly = true;
            if (cmbJenisSesi.Items.Count > 0) cmbJenisSesi.SelectedIndex = 0;
        }

        private void dtpTanggal_ValueChanged(object sender, EventArgs e)
        {
            txtHari.Text = dtpTanggal.Value.DayOfWeek.ToString();
        }
        private void btnPilihGambar_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                // Menyaring file agar hanya bisa memilih gambar
                ofd.Filter = "Image Files(*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // Menampilkan nama file di label yang baru kamu buat
                    lblPathGambar.Text = System.IO.Path.GetFileName(ofd.FileName);

                    // Catat alamat lengkap filenya ke dalam objek data
                    DataJadwal.PathGambar = ofd.FileName;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DataJadwal.NamaPsikolog = txtNamaPsikolog.Text;
            DataJadwal.TanggalSesi = dtpTanggal.Value.ToString("yyyy-MM-dd");
            DataJadwal.HariSesi = txtHari.Text;
            DataJadwal.JamMulai = txtMulai.Text;
            DataJadwal.JamAkhir = txtAkhir.Text;

            if (cmbJenisSesi.SelectedItem != null)
            {
                DataJadwal.JenisSesi = cmbJenisSesi.SelectedItem.ToString();
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    } // Penutup class FormInputJadwal

    public class JadwalModel
    {
        public string NamaPsikolog { get; set; } = "";
        public string TanggalSesi { get; set; } = "";
        public string HariSesi { get; set; } = "";
        public string JamMulai { get; set; } = "";
        public string JamAkhir { get; set; } = "";
        public string JenisSesi { get; set; } = "";
        public string PathGambar { get; set; } = "";
    }
}
