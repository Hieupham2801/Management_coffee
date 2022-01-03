using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyCuaHangMM
{
    public partial class Loading : Form
    {
        private string kieuDN;
        public Loading(string KieuDN)
        {
            InitializeComponent();
            kieuDN = KieuDN;
        }

        private void Loading_Load(object sender, EventArgs e)
        {
            guna2ShadowForm1.SetShadowForm(this);
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (guna2CircleProgressBar1.Value == 100)
            {
                timer1.Stop();
                if (kieuDN == "1")
                {
                    new GiaoDienNhanVien().Show();
                    this.Hide();
                }
                else
                {
                    new GiaoDienQuanLy().Show();
                    this.Hide();
                }
            }
            else
            {
                guna2CircleProgressBar1.Value += 1;
                label_val.Text = (Convert.ToInt32(label_val.Text) + 1).ToString();
            }
        }
    }
}
