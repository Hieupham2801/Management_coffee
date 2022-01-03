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
    public partial class BangNhanVien : Form
    {
        SqlConnection conn = new SqlConnection(TaoKetNoi.connectionString);
        public BangNhanVien()
        {
            InitializeComponent();
            BangNhanVien_Load();
        }
        int sodong;
        string manv, tennv, gioitinh,ngayvaolam, chucvu,diachinv,sdtnv,matkhau;
        DateTime kiemtrangay;
        SqlCommand sql;  // Tạo đối tượng
        DataSet bangphu = null; // Tạo một bảng giả
        SqlDataAdapter chuyenkieu = null; // Chuyển kiểu của Đối tượng truy vấn
        private void DoDuLieu(SqlCommand caulenh)
        {
            bangphu = new DataSet();
            chuyenkieu = new SqlDataAdapter(caulenh);
            sql.CommandType = CommandType.Text;
            SqlCommandBuilder xaydungkieu = new SqlCommandBuilder(chuyenkieu);
            chuyenkieu.Fill(bangphu, "nhanvien"); // Lấp đầy dữ liệu cho bảng giả bằng các dữ liệu đã được chuyển kiểu
            conn.Close(); // Không dùng đến kết nối thì đóng lại (giải phóng)
            data.DataSource = bangphu.Tables["nhanvien"]; // In dữ liệu lên bằng DataGridView
            data.Columns["Mã nhân viên"].ReadOnly = true;
            data.Columns["Ngày vào làm"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }
        private void BangNhanVien_Load()
        {
            conn.Open(); // Mở kết nối 
            // Câu lệnh thực hiện truy vấn
            sql = new SqlCommand("Select MaNV as 'Mã nhân viên',TenNV as 'Tên nhân viên',case when GioiTinh = '1' then 'Nam' Else N'Nữ' End as 'Giới tính', ChucVu as 'Chức vụ', NgayVaoLam as 'Ngày vào làm', DiaChiNV as 'Địa chỉ nhân viên', SĐTNV as 'Số điện thoại', MatKhau as 'Mật khẩu'  from nhanvien", conn);
            DoDuLieu(sql);
        }

        private void NutXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa thông tin của nhân viên ?\nTên khách hàng " + tennv + "?", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
            {
                conn.Open();
                SqlCommand command;
                command = conn.CreateCommand();
                command.CommandText = "Delete from nhanvien where manv = '" + manv + "'";
                command.ExecuteNonQuery();
                conn.Close();
                BangNhanVien_Load();
            }
        }

        private void XacNhan_Click(object sender, EventArgs e)
        {
            if (bangphu.HasChanges())
            {
                if (tennv == "")
                {
                    MessageBox.Show("Vui lòng nhập tên nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (gioitinh == "")
                {
                    MessageBox.Show("Vui lòng nhập giới tính", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (chucvu == "")
                {
                    MessageBox.Show("Vui lòng nhập chức vụ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (chucvu != "Quản lý" && chucvu != "Nhân viên")
                {
                    MessageBox.Show("Nhập sai kiểu chức vụ\nCó 2 hiểu: 'Quản lý' hoặc 'Nhân viên'","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
                else if (ngayvaolam == "")
                {
                    MessageBox.Show("Vui lòng nhập ngày vào làm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (DateTime.TryParse(ngayvaolam,out kiemtrangay) == false)
                {
                    MessageBox.Show("Kiểu nhập ngày sai");
                }
                else if (diachinv == "")
                {
                    MessageBox.Show("Vui lòng nhập địa chỉ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (sdtnv == "")
                {
                    MessageBox.Show("Vui lòng nhập số điện thoại nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (matkhau == "")
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (MessageBox.Show("Bạn có muốn xác nhận thông tin?", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
                    {
                        if (gioitinh == "Nam")
                        {
                            gioitinh = "1";
                        }
                        else
                        {
                            gioitinh = "0";
                        }
                        if (manv == "")
                        {
                            if (MessageBox.Show("Bạn có muốn thêm nhân viên?", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
                            {
                                conn.Open(); // Mở kết nối 
                                // Câu lệnh truy vấn
                                SqlCommand cmd1 = new SqlCommand("Insert nhanvien(TenNV,GioiTinh,ChucVu,NgayVaoLam,DiaChiNV,SĐTNV,MatKhau) Values(@TenNV,@GioiTinh,@ChucVu,@NgayVaoLam,@DiaChiNV,@SĐTNV,@MatKhau)", conn); // Tạo đối tượng
                                cmd1.Parameters.AddWithValue("@TenNV", tennv);// Thiết lập tham số
                                cmd1.Parameters.AddWithValue("@GioiTinh", gioitinh);
                                cmd1.Parameters.AddWithValue("@ChucVu", chucvu);
                                cmd1.Parameters.AddWithValue("@NgayVaoLam", ngayvaolam);
                                cmd1.Parameters.AddWithValue("@DiaChiNV", diachinv);
                                cmd1.Parameters.AddWithValue("@SĐTNV", sdtnv);
                                cmd1.Parameters.AddWithValue("@MatKhau", matkhau);
                                cmd1.ExecuteNonQuery(); // Thi hành truy vân
                                cmd1.Parameters.Clear();
                                conn.Close();
                                BangNhanVien_Load();
                            }
                        }
                        else
                        {
                            if (MessageBox.Show("Bạn có muốn sửa thông tin?", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
                            {
                                conn.Open();
                                SqlCommand cmd1 = new SqlCommand("Update nhanvien set TenNV = @TenNV,GioiTinh = @GioiTinh,ChucVu = @ChucVu,NgayVaoLam = @NgayVaoLam,DiaChiNV = @DiaChiNV,SĐTNV = @SĐTNV,MatKhau = @MatKhau where MaNV = @MaNV", conn); // Tạo đối tượng
                                cmd1.Parameters.AddWithValue("@MaNV", manv);
                                cmd1.Parameters.AddWithValue("@TenNV", tennv);// Thiết lập tham số
                                cmd1.Parameters.AddWithValue("@GioiTinh", gioitinh);
                                cmd1.Parameters.AddWithValue("@ChucVu", chucvu);
                                cmd1.Parameters.AddWithValue("@NgayVaoLam", ngayvaolam);
                                cmd1.Parameters.AddWithValue("@DiaChiNV", diachinv);
                                cmd1.Parameters.AddWithValue("@SĐTNV", sdtnv);
                                cmd1.Parameters.AddWithValue("@MatKhau", matkhau);
                                cmd1.ExecuteNonQuery(); // Thi hành truy vân
                                cmd1.Parameters.Clear();
                                conn.Close();
                                BangNhanVien_Load();
                            }
                        }
                    }
                }
            }
        }
        private void data_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            sodong = e.RowIndex;
            if (sodong >= 0)
            {
                manv = data.Rows[sodong].Cells["Mã nhân viên"].Value.ToString();
                tennv = data.Rows[sodong].Cells["Tên nhân viên"].Value.ToString();
                gioitinh = data.Rows[sodong].Cells["Giới tính"].Value.ToString();
                chucvu = data.Rows[sodong].Cells["Chức vụ"].Value.ToString();
                ngayvaolam = data.Rows[sodong].Cells["Ngày vào làm"].Value.ToString();
                diachinv = data.Rows[sodong].Cells["Địa chỉ nhân viên"].Value.ToString();
                sdtnv = data.Rows[sodong].Cells["Số điện thoại"].Value.ToString();
                matkhau = data.Rows[sodong].Cells["Mật khẩu"].Value.ToString();
            }
        }

        private void txt_timkiem_TextChanged(object sender, EventArgs e)
        {
            conn.Open(); // Mở kết nối 
            // Câu lệnh thực hiện truy vấn
            sql = new SqlCommand("Select MaNV as 'Mã nhân viên',TenNV as 'Tên Nhân Viên',case when GioiTinh = '1' then 'Nam' Else N'Nữ' End as 'Giới tính', ChucVu as 'Chức vụ', NgayVaoLam as 'Ngày vào làm', DiaChiNV as 'Địa chỉ nhân viên', SĐTNV as 'Số điện thoại', MatKhau as 'Mật khẩu'  from nhanvien Where MaNV like '%" + txt_timkiem.Text + "%' or TenNV like N'%" + txt_timkiem.Text + "%' or GioiTinh like N'%" + txt_timkiem.Text+ "%' or NgayVaoLam like '%" + txt_timkiem.Text + "%' or DiaChiNV like N'%" + txt_timkiem.Text + "%' or SĐTNV like '%" +txt_timkiem.Text + "%'", conn);
            DoDuLieu(sql);
        }
        private void data_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if(e.ColumnIndex == data.Columns["Ngày vào làm"].Index)
            {
                e.Cancel = true;
                MessageBox.Show("Bạn đã nhập sai kiểu dữ liệu ngày\nYêu cầu nhập kiểu: mm/DD/yyyy","Thông Báo",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            e.Cancel = false;
        }


    }
}
