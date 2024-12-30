using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace denemeodev
{
    public partial class Form2 : Form
    {
        public int SecilenOgrenciId { get; set; }
        public Form2()
        {
            InitializeComponent();
        }

        private void btnderssec_Click(object sender, EventArgs e)
        {
            using (var context = new OkulContext())
            {
                // Seçilen dersleri al
                var secilenDersler = dgvDersler.Rows.Cast<DataGridViewRow>()
                                      .Where(row => Convert.ToBoolean(row.Cells["Secim"].Value) == true)
                                      .Select(row => (int)row.Cells["DersId"].Value)
                                      .ToList();

                // Mevcut dersleri sil
                var mevcutDersler = context.OgrenciDersleri
                    .Where(od => od.OgrenciId == SecilenOgrenciId);
                context.OgrenciDersleri.RemoveRange(mevcutDersler);

                // Yeni dersleri ekle
                foreach (var dersId in secilenDersler)
                {
                    context.OgrenciDersleri.Add(new OgrenciDers
                    {
                        OgrenciId = SecilenOgrenciId,
                        DersId = dersId
                    });
                }

                context.SaveChanges();
                MessageBox.Show("Dersler başarıyla güncellendi.");
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            using (var context = new OkulContext())
            {
                // Öğrencinin bilgilerini al
                var ogrenci = context.Ogrenciler
                    .Include(o => o.Sinif)
                    .FirstOrDefault(o => o.OgrenciId == SecilenOgrenciId);

                if (ogrenci != null)
                {
                    lblOgrenciBilgi.Text = $"Adı: {ogrenci.Adi} Soyadı: {ogrenci.Soyadi} Numara: {ogrenci.Numara} Sınıf: {ogrenci.Sinif.Adi}";
                }

                // Tüm dersleri listele
                var tumDersler = context.Dersler
                    .Select(d => new
                    {
                        d.DersId,
                        d.Baslik
                    }).ToList();

                // Öğrencinin seçtiği dersleri al
                var ogrenciDersler = context.OgrenciDersleri
                    .Where(od => od.OgrenciId == SecilenOgrenciId)
                    .Select(od => od.DersId)
                    .ToList();

                // Dersleri DataGridView'e yükle
                dgvDersler.DataSource = tumDersler;

                // Seçim için Checkbox sütunu ekle
                if (!dgvDersler.Columns.Contains("Secim"))
                {
                    var checkboxColumn = new DataGridViewCheckBoxColumn
                    {
                        HeaderText = "Seç",
                        Name = "Secim",
                        Width = 50
                    };
                    dgvDersler.Columns.Add(checkboxColumn);
                }

                // Öğrencinin seçtiği dersleri işaretle
                foreach (DataGridViewRow row in dgvDersler.Rows)
                {
                    int dersId = (int)row.Cells["DersId"].Value;
                    if (ogrenciDersler.Contains(dersId))
                    {
                        row.Cells["Secim"].Value = true;
                    }
                }
            }
        }

       
    }
}
