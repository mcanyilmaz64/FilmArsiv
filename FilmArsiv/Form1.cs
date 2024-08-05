using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace FilmArsiv
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
      
        SqlConnection baglanti = new SqlConnection(@"Data Source=MCY\SQLEXPRESS;Initial Catalog=DBFILMARSIV;Integrated Security=True");

        void filmler()
        {
            string connectionString = @"Data Source=MCY\SQLEXPRESS;Initial Catalog=DBFILMARSIV;Integrated Security=True";

            using (SqlConnection baglanti = new SqlConnection(connectionString))
            {
                try
                {
                    baglanti.Open();
                    MessageBox.Show("Bağlantı başarıyla açıldı!"); // Bağlantının açıldığını doğrulamak için

                    string query = "SELECT * FROM TBLFILM";
                    SqlDataAdapter da = new SqlDataAdapter(query, baglanti);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show("Bağlantı açılırken bir hata oluştu: " + ex.Message);
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("SQL ile ilgili bir hata oluştu: " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bilinmeyen bir hata oluştu: " + ex.Message);
                }
                finally
                {
                    if (baglanti.State == System.Data.ConnectionState.Open)
                    {
                        baglanti.Close();
                        MessageBox.Show("Bağlantı kapatıldı."); // Bağlantının kapatıldığını doğrulamak için
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            filmler();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand cmd = new SqlCommand("insert into TBLFILM (AD,KATEGORI,LINK) values (@P1,@P2,@P3)",baglanti);
            cmd.Parameters.AddWithValue("@P1",TxtAd.Text);
            cmd.Parameters.AddWithValue("@P2",TxtKategori.Text);
            cmd.Parameters.AddWithValue("@P3",TxtLink.Text);
            cmd.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Film başarıyla Eklendi ", "Bilgi",MessageBoxButtons.OK,MessageBoxIcon.Information);
            

        }

        private void BtnHakkımızda_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Bu demo MAHMUT CAN YILMAZ TARAFINDAN YAZILMIŞTIR", "BİLGİ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;
            string link = dataGridView1.Rows[secilen].Cells[3].Value.ToString();
            webBrowser1.Navigate(link);
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnColor_Click(object sender, EventArgs e)
        {
            
            using (ColorDialog colorDialog = new ColorDialog())
            {
                
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                 
                    this.BackColor = colorDialog.Color;
                }
            }
        }

        private void BtnTamEkran_Click(object sender, EventArgs e)
        {
          
            string currentUrl = webBrowser1.Url?.ToString();

            
            if (!string.IsNullOrEmpty(currentUrl))
            {
                FrmEkran fr = new FrmEkran(currentUrl);
                fr.ShowDialog();
            }
            else
            {
                MessageBox.Show("Geçerli bir URL yok", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
