using System; //Temel sistem işlevselliği için kullanılan namespace'tir. Console, Math, DateTime gibi temel sınıfları içerir.
using System.Collections.Generic; //Genel koleksiyonlar için kullanılan namespace'tir. List<T>, Dictionary<TKey, TValue> gibi sınıfları içerir.
using System.ComponentModel; //Bileşen modelleme ve tasarım için kullanılan namespace'tir. INotifyPropertyChanged, BackgroundWorker gibi sınıfları içerir.
using System.Data; //Veri işlemleri için kullanılan namespace'tir. DataTable, DataSet gibi sınıfları içerir.
using System.Data.SqlClient; //SQL Server bağlantısı ve veri işlemleri için kullanılan namespace'tir.
                             //SqlConnection, SqlCommand, SqlDataAdapter gibi sınıfları içerir.
using System.Drawing; //Grafik işlemleri için kullanılan namespace'tir. Color, Font, Bitmap gibi sınıfları içerir.
using System.Linq; //LINQ (Language Integrated Query) işlemleri için kullanılan namespace'tir. IEnumerable<T>, IQueryable<T> gibi sınıfları içerir.
using System.Text; //Metin işlemleri için kullanılan namespace'tir. StringBuilder gibi sınıfları içerir.
using System.Threading.Tasks; //Asenkron programlama için kullanılan namespace'tir. Task, Task<T> gibi sınıfları içerir.
using System.Windows.Forms; //Windows Forms uygulamaları için kullanılan namespace'tir. Form, Button, TextBox gibi sınıfları içerir.

namespace KutuphaneYonetimSistemi
{
    public partial class FormKitaplar : Form
    {
        SqlConnection baglantı = new SqlConnection(@"Data Source= YOUR_SERVER_NAME ;Initial Catalog=DbKutuphane;Integrated Security=True;");
        //SQL Server bağlantısı için kullanılan SqlConnection nesnesidir.                                                                                                                                          
        //Bu nesne, veritabanına bağlanmak ve SQL sorgularını çalıştırmak için kullanılır.                                                                                                                                        
        //Bağlantı dizesi, veritabanının konumunu, adını ve güvenlik bilgilerini içerir.

        public FormKitaplar()
        {
            InitializeComponent(); //Formun bileşenlerini başlatmak için kullanılan bir metottur.

        }



        private void VerileriGoster()
        {
            try
            {
                string query = "SELECT * FROM TableKitaplar"; //TableKitaplar tablosundaki tüm verileri seçen SQL sorgusudur.
                                                              //Bu sorgu, kitapların tüm bilgilerini içeren bir sonuç kümesi döndürecektir.

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, baglantı); //SqlDataAdapter, SQL sorgusunu çalıştırmak ve
                                                                                     //sonuçları bir DataTable'a doldurmak için kullanılır. 
                                                                                     //Bu nesne, veritabanından veri çekmek ve
                                                                                     //bu verileri uygulama içinde kullanmak için bir köprü görevi görür.

                DataTable dt = new DataTable(); //DataTable, SQL sorgusunun sonuçlarını tutmak için kullanılan bir veri yapısıdır. 
                                                //Bu nesne, veritabanından çekilen verileri satır ve sütunlar halinde organize eder.

                sqlDataAdapter.Fill(dt); //SqlDataAdapter'ın Fill() metodu, SQL sorgusunun sonuçlarını DataTable'a doldurur. 
                                         //Bu işlem, veritabanından çekilen verilerin uygulama içinde kullanılabilir hale gelmesini sağlar.

                if (dt.Rows.Count > 0) //Eğer DataTable'da satır varsa, yani SQL sorgusundan veri çekilmişse, bu veriler DataGridView'e bağlanır.
                {
                    dataGridViewKitaplar.DataSource = dt; //DataGridView'e verileri bağlar, böylece kullanıcı verileri görebilir.
                }
            }
            catch (Exception ex) //Herhangi bir hata oluşursa, bu catch bloğu çalışır ve hata mesajını kullanıcıya gösterir.
            {
                MessageBox.Show("Hata : " + ex.ToString()); //Herhangi bir hata oluşursa kullanıcıya mesaj gösterilir.
            }
        }



        private void FormKitaplar_Load(object sender, EventArgs e)
        {
            VerileriGoster(); //Form yüklendiğinde VerileriGoster() metodu çağrılır, böylece kitaplar DataGridView'de görüntülenir.

        }

        private void buttonKitapEkle_Click(object sender, EventArgs e)
        {

            try //try bloğu, hata oluşabilecek kodları içerir. Eğer bu kodlarda bir hata oluşursa, catch bloğu çalışır ve hatayı yakalar.
            {
                /*
                Parametre	Gelen Veri      Sql Sorgusundaki Karşılığı      TextBox Adı
                @p1	        Kitap adı       KitapAdi                        textBoxKitapAdi
                @p2	        Yazar adı       YazarAdi                        textBoxYazarAdi
                @p3	        Yazar soyadı    YazarSoyadi                     textBoxYazarSoyadi
                @p4	        ISBN            ISBN                            textBoxISBN
                @p5	        Durum           Durum                           
                @p6	        Kitap tür kodu  KitapTurKodu                    textBoxKitapTurKodu
                */

                baglantı.Open(); //Bağlantıyı açmak önemlidir, böylece veritabanına erişim sağlanır. SQL Server bağlantısı aktif hale gelir.
                SqlCommand sqlCommand = new SqlCommand("INSERT INTO TableKitaplar " +
                    "(KitapAdi, YazarAdi, YazarSoyadi, ISBN, Durum , KitapTurKodu)" +
                    " VALUES (@p1, @p2, @p3, @p4, @p5, @p6)", baglantı);  //INSERT işlemi veritabanına gönderilir.
                sqlCommand.Parameters.AddWithValue("@p1", textBoxKitapAdi.Text);
                sqlCommand.Parameters.AddWithValue("@p2", textBoxYazarAdi.Text);
                sqlCommand.Parameters.AddWithValue("@p3", textBoxYazarSoyadi.Text);
                sqlCommand.Parameters.AddWithValue("@p4", textBoxISBN.Text);
                sqlCommand.Parameters.AddWithValue("@p5", true); //Kitap eklendiğinde durum true olarak kaydedilir, yani kitap mevcut olarak işaretlenir.
                sqlCommand.Parameters.AddWithValue("@p6", textBoxKitapTurKodu.Text);

                sqlCommand.ExecuteNonQuery(); //INSERT UPDATE DELETE işlemlerinde kullanılır.

                MessageBox.Show("Kitap eklendi"); //Kitap başarıyla eklendiğinde kullanıcıya mesaj gösterilir.
            }
            catch (Exception ex) //Herhangi bir hata oluşursa, bu catch bloğu çalışır ve hata mesajını kullanıcıya gösterir.
            {
                MessageBox.Show("Kitap Eklenirken Hata Oluştu! " + ex.ToString()); //Herhangi bir hata oluşursa kullanıcıya mesaj gösterilir.
            }
            finally //finally bloğu, try-catch yapısında her durumda çalışacak olan bir bölümdür. 
                    //Bu blok, hata oluşsa da oluşmasa da çalışır ve genellikle kaynakların serbest bırakılması gibi işlemler için kullanılır.
            {
                baglantı.Close(); //Bağlantıyı kapatmak önemlidir, böylece kaynaklar serbest bırakılır ve bağlantı havuzunda yer açılır.
                                  //SQL Server bağlantısı kapatılır.

            }
            VerileriGoster(); //Kitap eklendikten sonra VerileriGoster() metodu çağrılır,
                              //böylece DataGridView güncellenir ve yeni eklenen kitap görüntülenir.

        }

        private void dataGridViewKitaplar_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            labelGecikmeBedeli.Text = "0";//Gecikme bedelini sıfırlar, böylece her yeni seçimde önceki gecikme bedeli bilgisi temizlenir.
            int secilenSatir = dataGridViewKitaplar.SelectedCells[0].RowIndex; //Kullanıcının tıkladığı hücrenin satır indeksini alır.
                                                                               //Bu indeks, seçilen satırın hangi satır olduğunu belirler.
            labelID.Text = dataGridViewKitaplar.Rows[secilenSatir].Cells[0].Value.ToString(); //Seçilen satırın ilk hücresindeki değeri (ID) labelID'ye atar,
                                                                                              //böylece hangi kitabın seçildiği bilgisi tutulur.
            textBoxKitapAdi.Text = dataGridViewKitaplar.Rows[secilenSatir].Cells[1].Value.ToString();//Seçilen satırın ikinci hücresindeki değeri (Kitap Adı) textBoxKitapAdi'ye atar,
                                                                                                     //böylece kitap adı düzenlenebilir hale gelir.
            textBoxYazarAdi.Text = dataGridViewKitaplar.Rows[secilenSatir].Cells[2].Value.ToString(); //Seçilen satırın üçüncü hücresindeki değeri (Yazar Adı) textBoxYazarAdi'ye atar,
            textBoxYazarSoyadi.Text = dataGridViewKitaplar.Rows[secilenSatir].Cells[3].Value.ToString();//Seçilen satırın dördüncü hücresindeki değeri (Yazar Soyadı) textBoxYazarSoyadi'ye atar,
            textBoxISBN.Text = dataGridViewKitaplar.Rows[secilenSatir].Cells[4].Value.ToString();//Seçilen satırın beşinci hücresindeki değeri (ISBN) textBoxISBN'ye atar,
            textBoxKitapTurKodu.Text = dataGridViewKitaplar.Rows[secilenSatir].Cells[8].Value.ToString();//Seçilen satırın dokuzuncu hücresindeki değeri (Kitap Tür Kodu) textBoxKitapTurKodu'ya atar,
            textBoxOduncAlan.Text = dataGridViewKitaplar.Rows[secilenSatir].Cells[6].Value.ToString();

            /*
                                             C#	                SQL
                                            null                NULL
                     (SQL NULL karşılığı)<--DBNull.Value        NULL  
            */

            if (dataGridViewKitaplar.Rows[secilenSatir].Cells[7].Value != DBNull.Value) //Seçilen satırın sekizinci hücresindeki değerin null olup olmadığını kontrol eder.
                                                                                        //DbNull.Value, veritabanında null değerleri temsil eder. Eğer hücrede null değer varsa,
                                                                                        //bu kontrol false döner ve dateTimePickerOduncAlmaTarihi'ye bir değer atanmadan kalır.
            {
                dateTimePickerOduncAlmaTarihi.Value = (DateTime)dataGridViewKitaplar.Rows[secilenSatir].Cells[7].Value; //Seçilen satırın sekizinci hücresindeki değeri (Ödünç Alma Tarihi) dateTimePickerOduncAlmaTarihi'ye atar,
                                                                                                                        //bu hücrede null değer olabilir, bu yüzden kontrol ediyoruz.
            }

        }

        private void buttonKitapBilgileriGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                baglantı.Open();
                SqlCommand sqlCommand = new SqlCommand("UPDATE TableKitaplar SET" +
                    " KitapAdi=@p1, YazarAdi=@p2, YazarSoyadi=@p3, ISBN=@p4, KitapTurKodu=@p5" +
                    " WHERE ID=@p6", baglantı); //UPDATE işlemi veritabanına gönderilir. Belirli bir kitabın bilgilerini güncellemek için kullanılır.
                sqlCommand.Parameters.AddWithValue("@p1", textBoxKitapAdi.Text);
                sqlCommand.Parameters.AddWithValue("@p2", textBoxYazarAdi.Text);
                sqlCommand.Parameters.AddWithValue("@p3", textBoxYazarSoyadi.Text);
                sqlCommand.Parameters.AddWithValue("@p4", textBoxISBN.Text);
                sqlCommand.Parameters.AddWithValue("@p5", textBoxKitapTurKodu.Text);
                sqlCommand.Parameters.AddWithValue("@p6", labelID.Text);
                sqlCommand.ExecuteNonQuery();
                MessageBox.Show("Liste Güncellendi");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kitap Güncellenirken Hata Oluştu: " + ex.Message);
            }
            finally
            {
                baglantı.Close();
            }
            VerileriGoster();
        }

        private void buttonKitabOduncVer_Click(object sender, EventArgs e)
        {
            if (labelID.Text != "--") //Eğer kullanıcı ödünç vermek istediği kitabı seçmişse, yani labelID'nin değeri "--" değilse, ödünç verme işlemi gerçekleştirilir.
            {
                try
                {
                    baglantı.Open();
                    SqlCommand sqlCommand = new SqlCommand("UPDATE TableKitaplar SET " +
                        "Durum=@p1, OduncAlan=@p2, OduncAlmaTarihi=@p3 WHERE ID=@p4", baglantı); //UPDATE işlemi veritabanına gönderilir.
                                                                                                 //Belirli bir kitabın durumunu ödünç verilmiş olarak güncellemek için kullanılır.
                    sqlCommand.Parameters.AddWithValue("@p1", false);
                    sqlCommand.Parameters.AddWithValue("@p2", textBoxOduncAlan.Text);
                    sqlCommand.Parameters.Add("p3", SqlDbType.Date).Value = dateTimePickerOduncAlmaTarihi.Value.Date;
                    //SqlDbType.Date kullanarak parametre eklemek, SQL Server'a sadece tarih bilgisini göndermek için doğru bir yöntemdir.

                    //Ödünç alma tarihini SQL Server'a uygun formatta göndermek için SqlDbType.Date kullanılır.
                    //dateTimePickerOduncAlmaTarihi.Value.Date ifadesi, dateTimePicker'dan sadece tarih bilgisini alır ve saat bilgisini sıfırlar.
                    //Bu şekilde, SQL Server'a sadece tarih bilgisi gönderilir ve saat bilgisi etkilenmez.

                    //sqlCommand.Parameters.AddWithValue("@p3", dateTimePickerOduncAlmaTarihi.Value.Date); 
                    //Bu satır da aynı işlemi yapar, ancak SqlDbType.Date kullanmak daha doğru ve güvenli bir yöntemdir.

                    //sqlCommand.Parameters.AddWithValue("@p3", dateTimePickerOduncAlmaTarihi.Value); 
                    //Bu satır, dateTimePicker'dan alınan değeri doğrudan SQL Server'a göndermeye çalışır.
                    //Ancak, bu durumda saat bilgisi de gönderilir ve SQL Server'da tarih-saat formatında saklanır.
                    //Eğer sadece tarih bilgisi saklanmak isteniyorsa, SqlDbType.Date kullanarak sadece tarih bilgisini göndermek daha doğru bir yöntemdir.

                    sqlCommand.Parameters.AddWithValue("@p4", labelID.Text);
                    sqlCommand.ExecuteNonQuery();
                    MessageBox.Show("Liste Güncellendi");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kitap Ödünç Verilirken Hata Oluştu: " + ex.Message);
                }
                finally
                {
                    baglantı.Close();
                }
                VerileriGoster();
            }
            else
            {
                MessageBox.Show("Lütfen önce ödünç vermek istediğiniz kitabı listeden seçiniz!"); //Eğer kullanıcı ödünç vermek istediği kitabı seçmemişse, bu mesaj gösterilir.
            }

        }

        private void buttonGecikmeBedeliHesapla_Click(object sender, EventArgs e)
        {
            if (labelID.Text != "--")
            {
                DateTime bugununTarihi = DateTime.Now; //Bugünün tarihini alır.
                int gunFarki = (int)(bugununTarihi - dateTimePickerOduncAlmaTarihi.Value.Date).TotalDays; //Ödünç alma tarihinden bugünün tarihine kadar geçen gün sayısını hesaplar.
                                                                                                          //TotalDays, iki tarih arasındaki farkı gün cinsinden verir.
                if (gunFarki > 10)
                {
                    int gecikmeBedeli = (gunFarki - 10) * 10;
                    labelGecikmeBedeli.Text = gecikmeBedeli.ToString();
                }

            }
            else
            {
                MessageBox.Show("Lütfen önce geri iade edilecek kitabı listeden seçiniz!");
            }
        }

        private void buttonKitabiIadeEt_Click(object sender, EventArgs e)
        {
            if (labelID.Text != "--") //Eğer kullanıcı ödünç vermek istediği kitabı seçmişse, yani labelID'nin değeri "--" değilse, ödünç verme işlemi gerçekleştirilir.
            {
                try
                {
                    baglantı.Open();
                    SqlCommand sqlCommand = new SqlCommand("UPDATE TableKitaplar SET " +
                        "Durum=@p1, OduncAlan=@p2, OduncAlmaTarihi=@p3 WHERE ID=@p4", baglantı);
                    sqlCommand.Parameters.AddWithValue("@p1", true);
                    sqlCommand.Parameters.AddWithValue("@p2", DBNull.Value);
                    sqlCommand.Parameters.Add("p3", SqlDbType.Date).Value = DBNull.Value;
                    sqlCommand.Parameters.AddWithValue("@p4", labelID.Text);
                    sqlCommand.ExecuteNonQuery();
                    dateTimePickerOduncAlmaTarihi.Value = DateTime.Now.Date;
                    textBoxOduncAlan.Text = null;
                    MessageBox.Show("İade Güncellendi");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kitap iade edilirken Hata Oluştu: " + ex.Message);
                }
                finally
                {
                    baglantı.Close();
                }
                VerileriGoster();
            }
            else
            {
                MessageBox.Show("Lütfen önce iade edilecek kitabı listeden seçiniz!"); //Eğer kullanıcı ödünç vermek istediği kitabı seçmemişse, bu mesaj gösterilir.
            }

        }

        private void buttonYazilariTemizle_Click(object sender, EventArgs e)
        {
            textBoxTemizle();
        }
        public void textBoxTemizle()
        {
            labelID.Text = "--";
            textBoxKitapAdi.Text = "";
            textBoxYazarAdi.Text = "";
            textBoxYazarSoyadi.Text = "";
            textBoxISBN.Text = "";
            textBoxOduncAlan.Text = "";
            textBoxKitapTurKodu.Text = "";
            VerileriGoster();
        }
        private void buttonAra_Click(object sender, EventArgs e)
        {

            aramaSonuclariniGoster();

        }



        private void aramaSonuclariniGoster()
        {
            MessageBox.Show("ÇAlıştı");


            try
            {
                baglantı.Open();
                string query = " SELECT * FROM TableKitaplar WHERE " +
                    "KitapAdi LIKE '" + textBoxKitapAdi.Text + "%' " +
                    "AND YazarAdi LIKE '" + textBoxYazarAdi.Text + "%' " +
                    "AND YazarSoyadi LIKE '" + textBoxYazarSoyadi.Text + "%' " +
                    "AND ISBN LIKE '" + textBoxISBN.Text + "%' " +
                    "AND KitapTurKodu LIKE '" + textBoxKitapTurKodu.Text + "%' " +
                    "AND OduncAlan LIKE '" + textBoxOduncAlan.Text + "%' ";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, baglantı);
                DataTable dt = new DataTable();
                sqlDataAdapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    dataGridViewKitaplar.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata : " + ex.ToString());
            }
            finally
            {
                baglantı.Close();
            }





        }

        private void buttonKitapKayitSil_Click(object sender, EventArgs e)
        {
            if(labelID.Text == "--" || labelID.Text=="")
            {
                MessageBox.Show("Lütfen önce silmek istediğiniz kitabı listeden seçiniz!");
               
            }
            else
            {
                try
                {
                    baglantı.Open();
                    SqlCommand sqlCommand = new SqlCommand("DELETE FROM TableKitaplar WHERE ID = @p1", baglantı);
                    sqlCommand.Parameters.AddWithValue("@p1", labelID.Text);
                    sqlCommand.ExecuteNonQuery();
                    MessageBox.Show("Kitap başarıyla silindi.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kitap Silinirken   Hata Oluştu: " + ex.Message);
                }
                finally
                {
                    baglantı.Close();
                }
                textBoxTemizle();

            }
            



        }

        private void FormKitaplar_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit(); //FormKitaplar kapatıldığında uygulamanın tamamen kapanmasını sağlar.
                                //Bu, tüm açık formların ve kaynakların serbest bırakılmasını sağlar.
        }
    }
}
