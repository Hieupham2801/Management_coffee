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

namespace QuanLyCuaHangMM
{
    public partial class DangNhap : Form
    {
        SqlConnection conn = new SqlConnection(TaoKetNoi.connectionString);
        public DangNhap()
        {
            InitializeComponent();
        }

        private void ThucHienDangNhap_Click(object sender, EventArgs e)
        {
            if (txtUser.Text == "") // Điều kiện không để trống nhập ô tài khoản
            {
                MessageBox.Show("Vui lòng nhập tài khoản!!!", "Thông báo!!!");
                txtUser.Focus();
            }
            else if (txtPW.Text == "") // Điều kiện không để trống nhập ô mật khẩu
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!!!", "Thông báo");
                txtPW.Focus();
            }
            else
            {
                conn.Open(); // Mở kết nối 
                string tk = txtUser.Text;
                string mk = txtPW.Text;
                string sql = "select * from TaiKhoan WHERE TaiKhoan = '" + tk + "' and MatKhau = '" + mk + "'"; // Tạo câu lệnh truy vấn
                // Câu lệnh thực hiện truy vấn
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dta = cmd.ExecuteReader();
                // Đọc kết quả truy vấn
                if (dta.Read() == true) // Nếu tài khoản, mật khẩu đúng thì làm tiếp còn sai thì trả ra else
                {
                    string kieuDN = (string)dta["KieuTK"].ToString();
                    new Loading(kieuDN).Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Sai tài khoản mật khẩu", "Thông báo!!!");
                    txtUser.Focus();
                }
                conn.Close();
            }

        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


    }
}
