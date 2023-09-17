using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

// tìm hiểu kiến trúc
// lớp biểu diễn cơ sở dữ liệu (DataSet)
// lớp biểu diễn bảng (DataTable)

// sử dụng SqlDataAdapter
// là cầu nối
// để tương tác với nguồn dữ liệu
// và ánh xạ dữ liệu trong cơ sở dữ liệu

// DataSet thì nó như cơ sở dữ liệu luôn


/*
    Thật ra, tư duy của DataAdapter
    là thêm sửa xóa trên DataSet
    
    => Sau đó chạy phương thức Update()

    Nếu muốn hiển thị dữ liệu
    thì gọi Clear() để xóa dữ liệu cũ đi

    Sau đó gọi Fill() để đổ dữ liệu mới vào DataSet

    
    Nhưng trong code của tôi
    thì chỗ delete tôi bị lỗi suốt
    => Do đó, tôi xóa mà không dùng DataSet
    => Hơi không đúng tư duy của sử dụng DataAdapter lắm.
*/


namespace MyApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /*
                THÔNG TIN VỀ DATA SET

                DataSet là một cấu trúc phức tạp
                là thành phần cơ bản của ADO.NET
                
                Nó ánh xạ cơ sở dữ liệu
                thành các đối tượng trong bộ nhớ
                
                DataSet chứa trong nó
                là tập hợp các đối tượng DataTable

                Cách tạo đối tượng:
                DataSet dt = new DataSet();
            */


            /*
                THÔNG TIN VỀ DATA TABLE

                DataTable là đối tượng chứa dữ liệu
                
                Nó có tên

                Nó có các dòng, các cột

                Qua đó, nó là ánh xạ của một bảng
                trong cơ sở dữ liệu
            */


            /*
                THÔNG TIN VỀ DATA ADAPTER

                DataAdapter là lớp tạo ra cầu nối giữa DataSet
                với cơ sở dữ liệu

                Từ đó có thể lấy dữ liệu từ cơ sở dữ liệu
                về DataSet

                Dữ liệu được biên tập (INSERT, UPDATE, DELETE)
                trong DataSet
                => Sau đó cập nhật trở lại cơ sở dữ liệu

                Một đối tượng DataAdapter
                có các thuộc tính quan trọng
                để tạo ra các thao tác
                tương tác với cơ sở dữ liệu như:
                
                1. SelectCommand
                2. InsertCommand
                3. UpdateCommand
                4. DeleteCommand
            */


            /***********************************************
                1. THÔNG TIN VỀ SelectCommand
                - thuộc tính chứa đối tượng SqlCommand
                - nó chạy khi lấy dữ liệu
                - bằng cách gọi phương thức Fill()
            ************************************************/


            /***********************************************
                2. THÔNG TIN VỀ InsertCommand
                - thuộc tính chứa đối tượng SqlCommand
                - chạy khi thực hiện thêm bản ghi
            ************************************************/


            /***********************************************
                3. THÔNG TIN VỀ UpdateCommand
                - thuộc tính chứa đối tượng SqlCommand
                - chạy khi thực hiện cập nhật bản ghi
            ************************************************/


            /***********************************************
                4. THÔNG TIN VỀ DeleteCommand
                - thuộc tính chứa đối tượng SqlCommand
                - chạy khi thực hiện xóa dòng dữ liệu
            ************************************************/


            // viết code chuỗi kết nối
            string chuoi_ket_noi = @"
                Server = localhost;
                Database = Database_Demo;
                User ID = sa;
                Password = 123456
            ";


            // tạo đối tượng kết nối
            SqlConnection dt_KetNoi = new SqlConnection(chuoi_ket_noi);


            // mở kết nối
            dt_KetNoi.Open();


            // tạo đối tượng DataAdapter
            SqlDataAdapter dt_DataAdapter = new SqlDataAdapter();


            // thiết lập
            // bảng trong DataSet
            // ánh xạ tương ứng với bảng trong cơ sở dữ liệu
            // có tên là "Table_Demo"
            dt_DataAdapter.TableMappings.Add("Table", "Table_Demo");


            // sử dụng SelectCommand
            // thực thi khi gọi phương thức Fill()
            // để đổ dữ liệu vào DataSet
            dt_DataAdapter.SelectCommand = new SqlCommand(@"
                select  Table_ID,
                        Username,
                        Password
                
                from    Table_Demo;
            ", dt_KetNoi);


            // sử dụng InsertCommand
            // thực thi khi gọi phương thức Update()
            // có tác dụng thêm 1 bản ghi vào cơ sở dữ liệu
            dt_DataAdapter.InsertCommand = new SqlCommand(@"
                insert into Table_Demo(Username, Password)
                values (@bien1, @bien2);
            ", dt_KetNoi);

            // tạo biến 1 và biến 2
            // để truyền vào câu lệnh SQL
            // tham số thứ nhất: tên biến
            // tham số thứ 2: kiểu dữ liệu trong SQL
            // tham số thứ 3: độ dài của giá trị trong SQL
            // tham số thứ 4: tên cột
            // tạo biến 1, biến 2 thì bắt buộc phải viết sau cái InsertCommand ở trên
            // nếu không thì chương trình báo lỗi đấy
            dt_DataAdapter.InsertCommand.Parameters.Add("@bien1", SqlDbType.VarChar, 50, "Username");
            dt_DataAdapter.InsertCommand.Parameters.Add("@bien2", SqlDbType.VarChar, 50, "Password");


            // sử dụng DeleteCommand
            // thực thi khi gọi phương thức Update()
            // có tác dụng xóa bản ghi
            dt_DataAdapter.DeleteCommand = new SqlCommand(@"
                delete from Table_Demo
                where Table_ID = @bien_xoa
            ", dt_KetNoi);

            // tạo biến xóa
            // để truyền vào câu lệnh SQL
            // nếu mở comment thì dòng code bên dưới sẽ gây lỗi
            // dt_DataAdapter.DeleteCommand.Parameters.Add("@bien_xoa", SqlDbType.Int, int.MaxValue, "Table_ID");


            // sử dụng UpdateCommand
            // thực thi khi gọi phương thức Update()
            // có tác dụng cập nhật bản ghi
            dt_DataAdapter.UpdateCommand = new SqlCommand(@"
                update  Table_Demo
                
                set     Username = @bien1,
                        Password = @bien2

                where   Table_ID = @bien_ID;
            ", dt_KetNoi);

            // tạo biến 1, biến 2, biến ID
            // để truyền vào câu lệnh SQL
            dt_DataAdapter.UpdateCommand.Parameters.Add("@bien1", SqlDbType.VarChar, 50, "Username");
            dt_DataAdapter.UpdateCommand.Parameters.Add("@bien2", SqlDbType.VarChar, 50, "Password");
            dt_DataAdapter.UpdateCommand.Parameters.Add("@bien_ID", SqlDbType.Int, int.MaxValue, "Table_ID");


            // tạo đối tượng DataSet
            DataSet dt_DataSet = new DataSet();

            
            // gọi phương thức Fill()
            // để đổ dữ liệu vào đối tượng DataSet
            dt_DataAdapter.Fill(dt_DataSet);

            #region SELECT
                // có 2 cách để in dữ liệu ra màn hình nha
                // cách 1: dùng đối tượng DataSet
                // cách 2: dùng đối tượng DataTable


                // tạo đối tượng DataTable
                // rồi đổ ghi dữ liệu vào đối tượng DataTable
                DataTable dt_DataTable = dt_DataSet.Tables["Table_Demo"];


                // ở đây thì tôi dùng cách 1
                // gọi hàm in ra bảng
                InRa_Bang(dt_DataTable);
            #endregion


            #region INSERT
                // tạo đối tượng bản ghi
                DataRow ban_ghi = dt_DataTable.Rows.Add();


                // chuẩn bị thêm mới 1 bản ghi
                ban_ghi["Username"] = "Vip_01";
                ban_ghi["Password"] = "AAA 9999";


                // gọi phương thức Update()
                // để thêm mới 1 bản ghi
                dt_DataAdapter.Update(dt_DataSet);
                

                // gọi phương thức Clear()
                // để xóa hết dữ liệu cũ trong đối tượng DataSet
                dt_DataSet.Clear();


                // đổ dữ liệu mới vào đối tượng DataSet
                // vì bạn vừa thêm mới 1 bản ghi
                // nên bạn cần phải cập nhật lại đối tượng DataSet
                // để in ra màn hình cho nó hợp lý
                dt_DataAdapter.Fill(dt_DataSet);


                // thông báo sau khi thêm mới
                Console.WriteLine("\n\nSAU KHI THEM MOI 1 BAN GHI:");


                // gọi hàm in ra bảng
                InRa_Bang(dt_DataTable);
            #endregion


            #region UPDATE
                // cập nhật bản ghi thứ nhất
                // có Table_ID = 1

                
                // tạo đối tượng dòng cập nhật
                // thật ra viết như này
                // thì nó trả về bản ghi có index = 0
                // trong đối tượng DataTable
                // nhưng nó không ảnh hưởng đến việc cập nhật
                // nên kệ nó đi cũng được
                
                // đằng nào thì cũng phải có ít nhất 1 bản ghi
                // thì mới cập nhật được
                DataRow dong_CapNhat = dt_DataTable.Rows[0];


                // viết giá trị muốn cập nhật
                dong_CapNhat["Username"] = "admin";
                dong_CapNhat["Password"] = "123456";
                dong_CapNhat["Table_ID"] = 1;


                // gọi phương thức Update()
                dt_DataAdapter.Update(dt_DataSet);


                // gọi phương thức Clear()
                // để xóa hết dữ liệu cũ trong đối tượng DataSet
                dt_DataSet.Clear();


                // đổ dữ liệu mới vào đối tượng DataSet
                dt_DataAdapter.Fill(dt_DataSet);


                // thông báo sau khi cập nhật
                Console.WriteLine("\n\nSAU KHI CAP NHAT 1 BAN GHI:");


                // gọi hàm in ra bảng
                InRa_Bang(dt_DataTable);
            #endregion


            #region DELETE
                // xóa bản ghi thứ 2
                // có Table_ID = 2


                // thêm tham số để xóa
                dt_DataAdapter.DeleteCommand.Parameters.AddWithValue("@bien_xoa", 2);

            
                // gọi phương thức ExecuteNonQuery()
                // để thực thi câu lệnh xóa
                dt_DataAdapter.DeleteCommand.ExecuteNonQuery();


                // gọi phương thức Update()
                dt_DataAdapter.Update(dt_DataSet);


                // gọi phương thức Clear()
                // để xóa hết dữ liệu cũ trong đối tượng DataSet
                dt_DataSet.Clear();


                // đổ dữ liệu mới vào đối tượng DataSet
                dt_DataAdapter.Fill(dt_DataSet);


                // thông báo sau khi xóa
                Console.WriteLine("\n\nSAU KHI XOA 1 BAN GHI:");


                // gọi hàm in ra bảng
                InRa_Bang(dt_DataTable);
            #endregion


            // đóng kết nối
            dt_KetNoi.Close();
        }


        // tạo hàm in ra bảng
        // in ra cột, in ra dòng
        // tham số truyền vào là đối tượng có kiểu DataTable
        // kiểu trả về của hàm là void
        public static void InRa_Bang(DataTable dt)
        {
            // in ra tên bảng
            Console.WriteLine($"---------- Bang: {dt.TableName} ----------");

            // in ra tên cột
            foreach (DataColumn item in dt.Columns)
            {
                Console.Write($"{item.ColumnName, -15}");
            }

            // dòng code này để xuống dòng
            Console.WriteLine();

            // in ra dòng
            foreach (DataRow item in dt.Rows)
            {
                // đếm cột
                // để có được chỉ số (index)
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    Console.Write($"{item[i], -15}");
                }

                // dòng code này để xuống dòng
                Console.WriteLine();
            }
        }
    }
}