using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using System.IO;

namespace Muhasebe
{
    public partial class Form1 : Form
    {
        OleDbConnection baglanti;
        OleDbDataAdapter giderveri, gelirveri;
        OleDbCommand komut;
        DataSet verikumesi;
        DataTable gidertablo, gelirtablo;

        double kdvdahiltutar, kdv, kdvharictutar;

        double birimfiyat, adet, toplam;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.firmalarTableAdapter.Fill(this.databaseDataSet.firmalar);
            baglanti = new OleDbConnection("Provider=Microsoft.Jet.Oledb.4.0; Data Source=database.mdb");
            VeriCek();
        }

        private void firmakaydet_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("INSERT INTO firmalar (adi, adres, telefon) VALUES ('" + eklefirmaadi.Text.Trim() + "', '" + eklefirmaadres.Text.Trim() + "', '" + eklefirmatelefon.Text.Trim() + "')", baglanti);
            komut.ExecuteNonQuery();
            komut.Dispose();
            baglanti.Close();
            this.firmalarTableAdapter.Fill(this.databaseDataSet.firmalar);
        }

        private void VeriCek()
        {

            giderveri = new OleDbDataAdapter("SELECT * FROM gider", baglanti);
            gelirveri = new OleDbDataAdapter("SELECT * FROM gelir", baglanti);
            gidertablo = new DataTable();
            gelirtablo = new DataTable();
            baglanti.Open();
            giderveri.Fill(gidertablo);
            gelirveri.Fill(gelirtablo);
            baglanti.Close();
            dataGridView1.DataSource = gidertablo;
            dataGridView2.DataSource = gelirtablo;
        }

        private void kdvharic_CheckedChanged(object sender, EventArgs e)
        {
            KdvHaricHesap();
        }

        private void kdvdahil_CheckedChanged(object sender, EventArgs e)
        {
            KdvDahilHesap();
        }

        private void gelirgir_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("INSERT INTO gelir (nerden, neden, kdvdahiltutar, kdvharictutar, tarih, kdv) VALUES ('" + firmaadi.Text.Trim() + "','" + aciklama.Text.Trim() + "','" + kdvdahiltutar + "','" + kdvharictutar + "','" + DateTime.Today.ToString() + "','" + kdv + "')", baglanti);
            komut.ExecuteNonQuery();
            komut.Dispose();
            baglanti.Close();
            VeriCek();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Temizle();
        }

        private void Temizle()
        {
            listelefirmaadi.Text = "";
            buaykibakiye.Text = "";
            toplambakiye.Text = "";
            VeriCek();
        }

        private void firmasil_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            string komut = "DELETE FROM firmalar WHERE adi='" + listelefirmaadi.Text + "'";
            OleDbCommand My_Command = new OleDbCommand(komut, baglanti);
            My_Command.ExecuteNonQuery();
            baglanti.Close();
            this.firmalarTableAdapter.Fill(this.databaseDataSet.firmalar);
            Temizle();
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                seciliid.Text = "ID : "+ row.Cells[0].Value.ToString();

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            string komut = "UPDATE gider set durum='Ödendi' WHERE id='" + seciliid.Text + "'";
            OleDbCommand odendiyap = new OleDbCommand(komut, baglanti);
            odendiyap.ExecuteNonQuery();
            baglanti.Close();
            VeriCek();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.Show();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(Application.StartupPath + "\\database.mdb"))
                {

                    saveFileDialog1.DefaultExt = "mdb";
                    saveFileDialog1.Filter = "MDB Dosyaları (*.mdb)|*.mdb|Tüm Dosyalar(*.*)|*.*";
                    saveFileDialog1.ShowDialog();

                    if (saveFileDialog1.FileName != null)
                    {
                        File.Copy(Application.StartupPath + "\\database.mdb", saveFileDialog1.FileName);
                    }

                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Hata Oluştu : " + Ex.ToString());
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show("Lütfen Programın İçinden Yedeklediğiniz Bir Veritabanı (mdb) Dosyasını Seçiniz", "Uyarı", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                openFileDialog1.DefaultExt = "mdb";
                openFileDialog1.Filter = "MDB Dosyaları (*.mdb)|*.mdb|Tüm Dosyalar(*.*)|*.*";
                openFileDialog1.Title = "Veritabanı Seç";
                openFileDialog1.ShowDialog();
                if (openFileDialog1.FileName != null)
                {
                    File.Delete(Application.StartupPath + "\\database.mdb");
                    File.Copy(openFileDialog1.FileName, Application.StartupPath + "\\database.mdb");
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Hata Oluştu : " + Ex.ToString());
            }
            

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void tarih_ValueChanged(object sender, EventArgs e)
        {

        }

        private void miktar_TextChanged(object sender, EventArgs e)
        {

        }

        private void listelefirmaadi_SelectedIndexChanged(object sender, EventArgs e)
        {
            ////AY BAKİYE
            baglanti.Open();
            string sql = "SELECT * FROM gider Where tarih BETWEEN @tar1 and @tar2";
            DataTable dt = new DataTable();
            OleDbDataAdapter adp = new OleDbDataAdapter(sql, baglanti);
            adp.SelectCommand.Parameters.AddWithValue("@tar1", DateTime.Today.ToShortDateString());
            adp.SelectCommand.Parameters.AddWithValue("@tar2", DateTime.Today.AddDays(-30).ToShortDateString());
            adp.SelectCommand.Parameters.AddWithValue("@isim", listelefirmaadi.Text);
            adp.Fill(dt);

            string sql2 = "SELECT * FROM gelir WHERE nerden = '" + listelefirmaadi.Text + "'";
            DataTable dt2 = new DataTable();
            OleDbDataAdapter adp2 = new OleDbDataAdapter(sql2, baglanti);
            adp2.Fill(dt2);

            string sql3 = "SELECT * FROM gider WHERE nerden = '" + listelefirmaadi.Text + "'";
            DataTable dt3 = new DataTable();
            OleDbDataAdapter adp3 = new OleDbDataAdapter(sql3, baglanti);
            adp3.Fill(dt3);

            dataGridView1.DataSource = dt;
            //dataGridView1.DataSource = dt;

            int toplam = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                toplam += Convert.ToInt32(dataGridView1.Rows[i].Cells[8].Value);
            }

            buaykibakiye.Text = toplam.ToString() + " TL";
            dataGridView2.DataSource = dt2;

            //TOPLAM BAKİYE
            string islem2 = "select sum(kdvdahiltutar) from gider WHERE nerden = '" + listelefirmaadi.Text + "'";
            OleDbCommand komut2 = new OleDbCommand(islem2, baglanti);
            toplambakiye.Text = komut2.ExecuteScalar().ToString() + " TL";
            komut2.ExecuteNonQuery(); // Komutu çalıştırıyoruz
            baglanti.Close(); // Bağlantıyı mutlaka kapatıyoruz
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void birimfiyat_TextChanged(object sender, EventArgs e)
        {
            if (miktar.Text != "" && adetfiyat.Text != "")
            {
                birimfiyat = Double.Parse(adetfiyat.Text);
                adet = Double.Parse(miktar.Text);

                toplam = birimfiyat * adet;

                if (kdvdahil.Checked == true)
                {
                    KdvDahilHesap();
                }
                else
                {
                    KdvHaricHesap();
                }
            }
        }

        private void KdvHaricHesap()
        {
            if (kdvorani.Text == "%1")
            {
                kdvtutari.Text = "KDV Tutarı : " + (toplam * 1 / 100).ToString("#,##0.00") + " TL";
                kdvdahiltutar = toplam + toplam * 1 / 100;
                kdvharictutar = toplam - toplam * 1 / 100;
                kdvharictutar += toplam * 1 / 100;
                kdv = toplam * 18 / 100;
            }
            else if (kdvorani.Text == "%8")
            {
                kdvtutari.Text = "KDV Tutarı : " + (toplam * 8 / 100).ToString("#,##0.00") + " TL";
                kdvdahiltutar = toplam + toplam * 8 / 100;
                kdvharictutar = toplam - toplam * 8 / 100;
                kdvharictutar += toplam * 8 / 100;
                kdv = toplam * 18 / 100;
            }
            else if (kdvorani.Text == "%18")
            {
                kdvtutari.Text = "KDV Tutarı : " + (toplam * 18 / 100).ToString("#,##0.00");
                kdvdahiltutar = toplam + toplam * 18 / 100;
                kdvharictutar = toplam - toplam * 18 / 100;
                kdvharictutar += toplam * 18 / 100;
                kdv = toplam * 18 / 100;
            }

            toplamtutar.Text = "Toplam Tutar : " + kdvharictutar.ToString("#,##0.00") + " TL";
        }



        private void KdvDahilHesap()
        {
            if (kdvorani.Text == "%1")
            {
                kdvtutari.Text = "KDV Tutarı : " + (toplam * 1 / 100).ToString("#,##0.00") + " TL";
                kdvdahiltutar = toplam + toplam * 1 / 100;
                kdvharictutar = toplam - (toplam * 1 / 100);
                kdvharictutar += toplam * 1 / 100;
                kdv = toplam * 1 / 100;
            }
            else if (kdvorani.Text == "%8")
            {
                kdvtutari.Text = "KDV Tutarı : " + (toplam * 8 / 100).ToString("#,##0.00") + " TL";
                kdvdahiltutar = toplam + toplam * 8 / 100;
                kdvharictutar = toplam - (toplam * 8 / 100);
                kdvharictutar += toplam * 8 / 100;
                kdv = toplam * 8 / 100;
            }
            else if (kdvorani.Text == "%18")
            {
                kdvtutari.Text = "KDV Tutarı : " + (toplam * 18 / 100).ToString("#,##0.00");
                kdvdahiltutar = toplam + toplam * 18 / 100;
                kdvharictutar = toplam - (toplam * 18 / 100);
                kdvharictutar += toplam * 18 / 100;
                kdv = toplam * 18 / 100;
            }

            toplamtutar.Text = "Toplam Tutar : " + kdvdahiltutar.ToString("#,##0.00") + " TL";
        }

        private void gidergir_Click(object sender, EventArgs e)
        {
            
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("INSERT INTO gider (nerden, alinanmal, kdvdahiltutar, kdvharictutar, kdv,urunadet, birimfiyat, tarih, durum) VALUES ('" + firmaadi.Text.Trim() + "','" + aciklama.Text.Trim() + "','" + kdvdahiltutar + "','" + kdvharictutar + "','" + kdv + "','" + miktar.Text.Trim() + "','" + adetfiyat.Text.Trim() + "','" + DateTime.Today.ToShortDateString() + "','" + "Ödenmedi" + "')", baglanti);
            komut.ExecuteNonQuery();
            komut.Dispose();
            baglanti.Close();
            VeriCek();
        }
    }
}
