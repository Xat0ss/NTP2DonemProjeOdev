using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace denemeodev
{
    public class Ogrenci
    {

        public int OgrenciId { get; set; }  // Öğrenci Kimliği
        public string Adi { get; set; }     // Öğrenci Adı
        public string Soyadi { get; set; }  // Öğrenci Soyadı
        public string Numara { get; set; }  // Öğrenci Numarası
        public int SinifId { get; set; }    // Sınıf Kimliği
        public virtual Sinif Sinif { get; set; } // İlişkili Sınıf
        public virtual ICollection<OgrenciDers> OgrenciDersleri { get; set; }


    }
}
