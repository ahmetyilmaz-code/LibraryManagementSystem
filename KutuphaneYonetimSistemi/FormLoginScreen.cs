using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KutuphaneYonetimSistemi
{
    public partial class LoginScreen : Form
    {
        FormKitaplar formKitaplar;
        public LoginScreen()
        {
            InitializeComponent();
        }

        SqlConnection baglantı = new SqlConnection(@"Data Source= YOUR_SERVER_NAME ;Initial Catalog=DbKutuphane;Integrated Security=True;");

        private void buttonGiris_Click(object sender, EventArgs e)
        {
            string sifre = "";
            try
            {
                baglantı.Open();
                SqlCommand sqlKomut = new SqlCommand("Select Sifre From TableKutuphaneYoneticileri WHERE KullaniciAdi = @p1", baglantı);
                sqlKomut.Parameters.AddWithValue("@p1", textBoxKullaniciAdi.Text);
                SqlDataReader sqlDataReader = sqlKomut.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    sifre = sqlDataReader[0].ToString();

                }


                if (sifre == textBoxSifre.Text)
                {
                    
                    formKitaplar = new FormKitaplar();
                    this.Hide(); // Login ekranını gizler
                    formKitaplar.Show();
                }
                else
                {
                    MessageBox.Show("Kullanıcı Adı veya Şifre Hatalı !");
                    textBoxKullaniciAdi.Text = "";
                    textBoxSifre.Text = "";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Bağlantı Hatası " + ex.Message);
            }
            finally
            {
                baglantı.Close();
            }



        }
    }
}
