using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using misa.hust._21h.api.Entities;
using misa.hust._21h.api.Entities;
using MySqlConnector;
using System.Linq.Expressions;

namespace misa.hust._21h.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {


        /// <summary>
        /// Lấy danh sách tất cả nhân viên
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            try
            {
                //khởi tạo kết nối tới DB MySQL
                string ConnectionString = "Server=localhost;Port=3306;Database=DAOTAO.AI.2023.NTDUONG;Uid=root;Pwd=root;";
                var mySqlConnection = new MySqlConnection(ConnectionString);

                //chuẩn bị câu lệnh Select all employee
                string getAllEmployeeCommand = "SELECT * FROM employee;";

                //thực hiện chạy câu lệnh
                var employees = mySqlConnection.Query<Employee>(getAllEmployeeCommand);

                //xử lí dữ liệu trả về
                if (employees != null)
                {
                    return StatusCode(StatusCodes.Status200OK, employees);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }


        /// <summary>
        /// API lấy thông tin chi tiết nhân viên
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns>Thông tin chi tiết 1 nhân viên</returns>
        [HttpGet]
        [Route("{employeeID}")]
        public IActionResult GetEmployeeByID([FromRoute] Guid employeeID)
        {
            try
            {
                //khởi tạo kết nối tới DB MySQL
                string ConnectionString = "Server=localhost;Port=3306;Database=DAOTAO.AI.2023.NTDUONG;Uid=root;Pwd=root;";
                var mySqlConnection = new MySqlConnection(ConnectionString);

                //chuẩn bị tên Store Proc
                string storeProName = "PROC_employee_GetEmployeeByID";

                //chuẩn bị tham số đầu vào cho Proc
                var parameters = new DynamicParameters();
                parameters.Add("@employeeID", employeeID);

                var employee = mySqlConnection.Query(storeProName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                //xử lí dữ liệu trả về
                if (employee != null)
                {
                    return StatusCode(StatusCodes.Status200OK, employee);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }


        /// <summary>
        /// lọc danh sách nhân viên
        /// </summary>
        /// <param name="keyword">từ khóa muốn tìm kiếm</param>
        /// <param name="positionID"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("filter")]
        public IActionResult FilterEmployees(
            [FromQuery] string keyword, 
            [FromQuery] int pageSize,
            [FromQuery] int pageNumber)
        {
            try
            {
                //khởi tạo kết nối tới DB MySQL
                string ConnectionString = "Server=localhost;Port=3306;Database=DAOTAO.AI.2023.NTDUONG;Uid=root;Pwd=root;";
                var mySqlConnection = new MySqlConnection(ConnectionString);

                //chuẩn bị tên Store Proc
                string storeProName = "PROC_employee_GetPaging";

                //chuẩn bị tham số đầu vào cho Proc
                var parameters = new DynamicParameters();
                parameters.Add("@v_Offset", (pageNumber - 1) * pageSize);
                parameters.Add("@v_Limit",pageSize);

                //xây dựng ra câu lệnh where
                var orConditions = new List<string>();
                var andConditions = new List<string>();
                string whereClause = "";

                if (keyword != null)
                {
                    orConditions.Add($"EmployeeCode LIKE '%{keyword}%'");
                    orConditions.Add($"EmployeeName LIKE '%{keyword}%'");
                    orConditions.Add($"PhoneNumber LIKE '%{keyword}%'");
                }
                if (orConditions.Count > 0)
                {
                    whereClause = $"({string.Join(" OR ", orConditions)})";
                }
                parameters.Add("v_Where", whereClause);

                //thực hiện gọi lệnh 
                var multipleResults = mySqlConnection.QueryMultiple(storeProName, parameters, commandType: System.Data.CommandType.StoredProcedure); 

                //xử lí kết quả trả về
                if (multipleResults != null)
                {
                    var employees = multipleResults.Read<Department>().ToList();
                    var totalCount = multipleResults.Read<int>().Single();
                    return StatusCode(StatusCodes.Status200OK, new PagingData<Department>()
                    {
                        Data = employees,
                        TotalCount = totalCount
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
            
        }

        /// <summary>
        /// Thêm mới nhân viên
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult InsertEmployee([FromBody] Employee employee)
        {
            try
            {
                //khởi tạo kết nối tới DB MySQL
                string ConnectionString = "Server=localhost;Port=3306;Database=DAOTAO.AI.2023.NTDUONG;Uid=root;Pwd=root;";
                var mySqlConnection = new MySqlConnection(ConnectionString);

                //chuẩn bị câu lệnh Insert Into
                string insertEmployeeCommand = "INSERT INTO employee (EmployeeID, EmployeeCode, EmployeeName, Gender, DateOfBirth, IdentityNumber, IdentityIssuedPlace, IdentityIssuedDate, Email, HomeTelephone, PositionID, PositionName, DepartmentID, DepartmentName, MobilePhone, AccountNumber, Bank, BankBranch, Functions, Address, CreatedDate, CreatedBy, ModifiedDate, ModifiedBy)" +
                    "VALUES(@employeeID, @employeeCode)";

                //chuẩn bị tham số đầu vào cho Insert
                var employeeID = Guid.NewGuid();
                var parameters = new DynamicParameters();
                parameters.Add("@employeeID", employeeID);
                parameters.Add("@eployeeCode", employee.EmployeeCode);

                //thực hiện gọi vào DB để chạy lệnh Insert Into với tham số ở trên
                int numberOfAffectedRows = mySqlConnection.Execute(insertEmployeeCommand, parameters);

                //xử lí kết quả trả về từ DB
                if (numberOfAffectedRows > 0)
                {
                    //trả về dữ liệu cho client
                    return StatusCode(StatusCodes.Status201Created, employeeID);
                }
                
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
            return StatusCode(StatusCodes.Status201Created, Guid.NewGuid());
        }


        /// <summary>
        /// Update thông tin nhân viên
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{employeeID}")]
        public IActionResult UpdateEmployee([FromBody] Employee employee, [FromRoute] Guid employeeID)
        {
            try
            {
                //khởi tạo kết nối tới DB MySQL
                string ConnectionString = "Server=localhost;Port=3306;Database=DAOTAO.AI.2023.NTDUONG;Uid=root;Pwd=root;";
                var mySqlConnection = new MySqlConnection(ConnectionString);

                //chuẩn bị câu lệnh UPDATE
                string updateEmployeeCommand = "UPDATE employee " +
                    "SET EmployeeID = @EmployeeID, " +
                    "EmployeeCode = @EmployeeCode, " +
                    "EmployeeName = @EmployeeName, " +
                    "Gender = @Gender, " +
                    "DateOfBirth = @DateOfBirth, " +
                    "IdentityNumber = @IdentityNumber, " +
                    "IdentityIssuedPlace = @IdentityIssuedPlace, " +
                    "IdentityIssuedDate = @IdentityIssuedDate, " +
                    "Email = @Email, " +
                    "HomeTelephone = @HomeTelephone, " +
                    "PositionID = @PositionID, " +
                    "PositionName = @PositionName, " +
                    "DepartmentID = @DepartmentID, " +
                    "DepartmentName = @DepartmentName, " +
                    "MobilePhone = @MobilePhone, " +
                    "AccountNumber = @AccountNumber, " +
                    "Bank = @Bank, " +
                    "BankBranch = @BankBranch, " +
                    "Functions = @Functions, " +
                    "Address = @Address, " +
                    "CreatedDate = @CreatedDate, " +
                    "CreatedBy = @CreatedBy, " +
                    "ModifiedDate = @ModifiedDate, " +
                    "ModifiedBy = @ModifiedBy " +
                    "WHERE EmployeeID = @EmployeeID;";


                //chuẩn bị tham số đầu vào cho Update
                var parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", employeeID);
                parameters.Add("@EployeeCode", employee.EmployeeCode);
                parameters.Add("@EmployeeName", employee.EmployeeName);
                parameters.Add("@Gender", employee.Gender);
                parameters.Add("@DateOfBirth", employee.DateOfBirth);
                parameters.Add("@IdentityNumber", employee.IdentityNumber);
                parameters.Add("@IdentityIssuedPlace", employee.IdentityIssuedPlace);
                parameters.Add("@IdentityIssuedDate", employee.IdentityIssuedDate);
                parameters.Add("@Email", employee.Email);
                parameters.Add("@HomeTelephone", employee.HomeTelephone);
                parameters.Add("@PositionID", employee.PositionID);
                parameters.Add("@PositionName", employee.PositionName);
                parameters.Add("@DepartmentID", employee.DepartmentID);
                parameters.Add("@DepartmentName", employee.DepartmentName);
                parameters.Add("@MobilePhone", employee.MobilePhone);
                parameters.Add("@AccountNumber", employee.AccountNumber);
                parameters.Add("@Bank", employee.Bank);
                parameters.Add("@BankBranch", employee.BankBranch);
                parameters.Add("@Address", employee.Address);
                parameters.Add("@CreatedDate", employee.CreatedDate);
                parameters.Add("@CreatedBy", employee.CreatedBy);
                parameters.Add("@ModifiedDate", employee.ModifiedDate);
                parameters.Add("@ModifiedBy", employee.ModifiedBy);

                //thực hiện gọi vào DB để chạy lệnh Insert Into với tham số ở trên
                int numberOfAffectedRows = mySqlConnection.Execute(updateEmployeeCommand, parameters);

                //xử lí kết quả trả về từ DB
                if (numberOfAffectedRows > 0)
                {
                    //trả về dữ liệu cho client
                    return StatusCode(StatusCodes.Status200OK, employeeID);
                }

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
            return StatusCode(StatusCodes.Status200OK, employeeID);
        }



        /// <summary>
        /// Xóa 1 nhân viên
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{employeeID}")]
        public IActionResult DeleteEmployee([FromRoute] Guid employeeID)
        {
            return StatusCode(StatusCodes.Status200OK, employeeID);
        }



    }

}
