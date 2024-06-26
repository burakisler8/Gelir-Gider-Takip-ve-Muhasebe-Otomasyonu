using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;

namespace Muhasebe
{
    public partial class Form2 : Form
    {
        
        public Form2()
        {
            InitializeComponent();
        }

        private void kaydet1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.firmaadi = firmadi.Text;
            Properties.Settings.Default.yetkiliadi = yetkiliadi.Text;
            Properties.Settings.Default.eposta1 = eposta1.Text;
            Properties.Settings.Default.eposta2 = eposta2.Text;
            Properties.Settings.Default.eposta3 = eposta3.Text;
            Properties.Settings.Default.Save();
            MessageBox.Show("Ayalarınız Kaydedildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void kaydet_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.gondericieposta = gondericieposta.Text;
            Properties.Settings.Default.gondericipostasifre = gondericipostasifre.Text;
            Properties.Settings.Default.Save();
            MessageBox.Show("Ayalarınız Kaydedildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool postagonder()
        {

            string icerik = Properties.Settings.Default.firmaadi +" - "+  Properties.Settings.Default.yetkiliadi + " - Program Deneme Mesajı";
            MailMessage ePosta = new MailMessage();
            ePosta.From = new MailAddress(Properties.Settings.Default.firmaadi + "@gmail.com");
            if (Properties.Settings.Default.eposta1 != "")
            {
                ePosta.To.Add(Properties.Settings.Default.eposta1);
            }
            if (Properties.Settings.Default.eposta2 != "")
            {
                ePosta.To.Add(Properties.Settings.Default.eposta2);
            }
            
            if (Properties.Settings.Default.eposta3 != "")
            {
                ePosta.To.Add(Properties.Settings.Default.eposta3);
            }
            ePosta.Subject = "Gider Hatırlatma";
            ePosta.Body = icerik;

            SmtpClient smtp = new SmtpClient();
            smtp.Credentials = new System.Net.NetworkCredential(Properties.Settings.Default.gondericieposta, Properties.Settings.Default.gondericipostasifre);
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            object userState = ePosta;
            bool kontrol = true;
            try
            {
                smtp.SendAsync(ePosta, (object)ePosta);
                MessageBox.Show("E-Posta gönderildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SmtpException ex)
            {
                kontrol = false;
                System.Windows.Forms.MessageBox.Show(ex.Message, "Mail Gönderme Hatasi");
            }
            return kontrol;
        }

        private void testet_Click(object sender, EventArgs e)
        {
            postagonder();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            firmadi.Text = Properties.Settings.Default.firmaadi;
            yetkiliadi.Text = Properties.Settings.Default.yetkiliadi;
            eposta1.Text = Properties.Settings.Default.eposta1;
            eposta2.Text = Properties.Settings.Default.eposta2;
            eposta3.Text = Properties.Settings.Default.eposta3;
            gondericieposta.Text = Properties.Settings.Default.gondericieposta;
            gondericipostasifre.Text = Properties.Settings.Default.gondericipostasifre;
            kadi.Text = Properties.Settings.Default.kadi;
            sifre.Text = Properties.Settings.Default.sifre;

            haftalikgonder.Checked = Properties.Settings.Default.haftadagonder;
            acilisbaslat.Checked = Properties.Settings.Default.acilistabaslat;
            sifreiste.Checked = Properties.Settings.Default.sifreiste;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.kadi = kadi.Text;
            Properties.Settings.Default.sifre = sifre.Text;
            Properties.Settings.Default.Save();
        }

        private void haftalikgonder_CheckedChanged(object sender, EventArgs e)
        {
            if (haftalikgonder.Checked == false)
            {
                Properties.Settings.Default.haftadagonder = true;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.haftadagonder = false;
                Properties.Settings.Default.Save();
            }
        }

        private void acilisbaslat_CheckedChanged(object sender, EventArgs e)
        {
            if (acilisbaslat.Checked == false)
            {
                Properties.Settings.Default.acilistabaslat = true;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.acilistabaslat = false;
                Properties.Settings.Default.Save();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (sifreiste.Checked == false)
            {
                Properties.Settings.Default.sifreiste = true;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.sifreiste = false;
                Properties.Settings.Default.Save();
            }
        }
    }
}
