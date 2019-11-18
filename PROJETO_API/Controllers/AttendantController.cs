using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PROJETO_API.Requests;
using PROJETO_API.Results;
using MySql.Data.MySqlClient;

namespace PROJETO_API.Controllers
{
    [Route("api/[controller]")]
    public class AttendantController : Controller
    {
        private readonly Appsettings _appSettings;

        public AttendantController(Appsettings appSettings)
        {
            _appSettings = appSettings;
        }

        [HttpGet]
        public IActionResult Get()
        {
            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);
            List<AttendantResult> result = new List<AttendantResult>();

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("SELECT AttendantName, AttendantCpf, AttendantEmail FROM Attendant", conn))
                {
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        result.Add(new AttendantResult
                        {
                            AttendantID = dataReader.GetInt32(0),
                            AttendantName = dataReader.GetString(1),
                            AttendantCpf = dataReader.GetString(2),
                            AttendantEmail = dataReader.GetString(3),

                        });
                    }
                }
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]AttendantRequest request)
        {
            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);
            AttendantResult result = new AttendantResult();
            
            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("INSERT INTO Attendant (AttendantName, AttendantCpf, AttendantEmail, AttendantPass, UbsID) VALUES(@AttendantName, @AttendantCpf, @AttendantEmail, @AttendantPass, @UbsID)", conn))
                {

                    cmd.Parameters.AddWithValue("@AttendantName", request.AttendantName);
                    cmd.Parameters.AddWithValue("@AttendantCpf", request.AttendantCpf);
                    cmd.Parameters.AddWithValue("@AttendantEmail", request.AttendantEmail);
                    cmd.Parameters.AddWithValue("@AttendantPass", request.AttendantPass);
                    cmd.Parameters.AddWithValue("@UbsID", request.UbsID);

                    cmd.ExecuteNonQuery();

                    using (MySqlCommand cmd2 = new MySqlCommand("SELECT last_insert_id()", conn))
                    {
                        result.AttendantID = (int)(ulong)cmd2.ExecuteScalar();
                    }

                }

                return new OkObjectResult(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }

        }

        [HttpGet("{AttendantID}")]
        public IActionResult Get(int AttendantID)
        {
            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);
            AttendantResult result = new AttendantResult();

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("SELECT AttendantID, AttendantName, AttendantCpf, AttendantEmail FROM Attendant WHERE AttendantID =" + AttendantID, conn))
                {
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {

                        result.AttendantID = dataReader.GetInt32(0);
                        result.AttendantName = dataReader.GetString(1);
                        result.AttendantCpf = dataReader.GetString(2);
                        result.AttendantEmail = dataReader.GetString(3);
                                                
                    }
                }
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }
        }
    }
}
