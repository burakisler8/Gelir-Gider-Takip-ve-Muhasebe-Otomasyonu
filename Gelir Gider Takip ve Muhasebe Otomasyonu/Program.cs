using System;
using System.Windows.Forms;

namespace Muhasebe
{
    static class Program
    {
        /// <summary>
        /// Uygulamanın ana girdi noktası.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (Properties.Settings.Default.firmaadi != "")
            {
                if (Properties.Settings.Default.sifreiste == true)
                {
                    Application.Run(new giris());
                }
                else
                {
                    Application.Run(new Form1());
                }
                
            }
            else
            {
                Application.Run(new Form2());
            }
            
        }
    }
}
