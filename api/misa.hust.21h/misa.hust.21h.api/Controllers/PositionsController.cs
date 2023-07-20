using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using misa.hust._21h.api.Entities;
using MySqlConnector;

namespace misa.hust._21h.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllPosition()
        {
            try
            {
                //khởi tạo kết nối tới DB MySQL
                string ConnectionString = "Server=localhost;Port=3306;Database=DAOTAO.AI.2023.NTDUONG;Uid=root;Pwd=root;";
                var mySqlConnection = new MySqlConnection(ConnectionString);

                //chuẩn bị câu lệnh Select all employee
                string getAllPositionCommand = "SELECT * FROM possions;";

                //thực hiện chạy câu lệnh
                var position = mySqlConnection.Query<Position>(getAllPositionCommand);

                //xử lí dữ liệu trả về
                if (position != null)
                {
                    return StatusCode(StatusCodes.Status200OK, position);
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
    }
}
