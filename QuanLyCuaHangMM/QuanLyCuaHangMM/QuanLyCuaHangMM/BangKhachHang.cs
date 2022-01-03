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
    public partial class BangKhachHang : Form
    {
        SqlConnection conn = new SqlConnection(TaoKetNoi.connectionString);
        public BangKhachHang()
        {
            InitializeComponent();
            BangKhachHang_Load();
        }
        int sodong;
        string makh, tenkh, diachi, sdtkh;
        SqlCommand sql;  // Tạo đối tượng
        DataSet bangphu = null; // Tạo một bảng giả
        SqlDataAdapter chuyenkieu = null; // Chuyển kiểu của Đối tượng truy vấn
        private void DoDuLieu(SqlCommand caulenh)
        {
            bangphu = new DataSet();
            chuyenkieu = new SqlDataAdapter(caulenh);
            sql.CommandType = CommandType.Text;
            SqlCommandBuilder xaydungkieu = new SqlCommandBuilder(chuyenkieu);
            chuyenkieu.Fill(bangphu, "khachhang"); // Lấp đầy dữ liệu cho bảng giả bằng các dữ liệu đã được chuyển kiểu
            conn.Close(); // Không dùng đến kết nối thì đóng lại (giải phóng)
            data.DataSource = bangphu.Tables["khachhang"]; // In dữ liệu lên bằng DataGridView
            data.Columns["MaKH"].ReadOnly = true;
        }
        private void BangKhachHang_Load()
        {
            conn.Open(); // Mở kết nối 
            // Câu lệnh thực hiện truy vấn
            sql = new SqlCommand("Select * from khachhang", conn);
            DoDuLieu(sql);
        }
        private void data_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            sodong = e.RowIndex;
            if (sodong >= 0)
            {
                makh = data.Rows[sodong].Cells["MaKH"].Value.ToString();
                tenkh = data.Rows[sodong].Cells["TenKH"].Value.ToString();
                diachi = data.Rows[sodong].Cells["DiaChi"].Value.ToString();
                sdtkh = data.Rows[sodong].Cells["SĐTKH"].Value.ToString();
            }
        }
        private void txt_timkiem_TextChanged(object sender, EventArgs e)
        {
            conn.Open(); // Mở kết nối 
            // Câu lệnh thực hiện truy vấn
            sql = new SqlCommand("Select * from khachhang where MaKH like '%" + txt_timkiem.Text + "%' or TenKH like N'%" + txt_timkiem.Text + "%' or DiaChi like N'%" + txt_timkiem.Text + "%' or SĐTKH like N'%" + txt_timkiem.Text + "%'", conn);
            DoDuLieu(sql);
        }

        private void NutXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa thông tin của khách hàng?\nTên khách hàng " + tenkh + "?", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
            {
                conn.Open();
                SqlCommand command;
                command = conn.CreateCommand();
                command.CommandText = "Delete from khachhang where makh = '" + makh + "'";
                command.ExecuteNonQuery();
                conn.Close();
                BangKhachHang_Load();
            }
        }
        private void XacNhan_Click(object sender, EventArgs e)
        {
            if (bangphu.HasChanges())
            {
                if (tenkh == "")
                {
                    MessageBox.Show("Vui lòng nhập lên khách hàng", "Thông báo", MessageBoxButtons.OK);
                }
                else if (diachi == "")
                {
                    MessageBox.Show("Vui lòng nhập địa chỉ", "Thông báo", MessageBoxButtons.OK);
                }
                else if (sdtkh == "")
                {
                    MessageBox.Show("Vui lòng nhập số điện thoại khách hàng", "Thông báo", MessageBoxButtons.OK);
                }
                else
                {
                    if (MessageBox.Show("Bạn có muốn xác nhận thông tin?", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
                    {
                        if (makh == "")
                        {
                            conn.Open(); // Mở kết nối 
                            // Câu lệnh truy vấn
                            SqlCommand cmd1 = new SqlCommand("Insert khachhang(TenKH,DiaChi,SĐTKH) Values(@TenKH,@DC,@SDT)", conn); // Tạo đối tượng
                            cmd1.Parameters.AddWithValue("@TenKH", tenkh);// Thiết lập tham số
                            cmd1.Parameters.AddWithValue("@DC", diachi);
                            cmd1.Parameters.AddWithValue("@SDT", sdtkh);
                            cmd1.ExecuteNonQuery(); // Thi hành truy vân
                            cmd1.Parameters.Clear();
                            conn.Close();
                            BangKhachHang_Load();
                        }
                        else
                        {
                            conn.Open();
                            SqlCommand cmd1 = new SqlCommand("Update khachhang set TenKH = @TenKH, DiaChi = @DC, SĐTKH = @SDT where MaKH = @MaKH", conn); // Tạo đối tượng
                            cmd1.Parameters.AddWithValue("@MaKH", makh);
                            cmd1.Parameters.AddWithValue("@TenKH", tenkh);// Thiết lập tham số
                            cmd1.Parameters.AddWithValue("@DC", diachi);
                            cmd1.Parameters.AddWithValue("@SDT", sdtkh);
                            cmd1.ExecuteNonQuery(); // Thi hành truy vân
                            cmd1.Parameters.Clear();
                            conn.Close();
                            BangKhachHang_Load();
                        }
                    }
                }
            }
        }
    }
}
