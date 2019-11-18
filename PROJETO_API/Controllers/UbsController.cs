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
    public class UbsController : Controller
    {
        private readonly Appsettings _appSettings;

        public UbsController(Appsettings appSettings)
        {
            _appSettings = appSettings;
        }

        [HttpGet("{UserCpf}")]
        public IActionResult Get(int UserCpf)
        {
            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);
            UserDependentResult result = null;

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("SELECT User.UserID, User.UserCpf, User.UserName, Dependent.DependentID, Dependent.DependentName, Dependent.DependentBirth, Dependent.DependentBlood, Dependent.DependentAllergy, Dependent.DependentSusNo FROM User INNER JOIN Dependent ON (User.UserID = Dependent.UserID) WHERE User.UserCpf = " + UserCpf, conn))
                {
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        if (result == null)
                        {
                            result = new UserDependentResult
                            {
                                UserID = dataReader.GetInt32(0),
                                UserCpf = dataReader.GetString(1),
                                UserName = dataReader.GetString(2)
                            };

                            result.Dependentes = new List<DependentVaccineResult>();
                        }

                        result.Dependentes.Add(new DependentVaccineResult
                        {

                            UserID = dataReader.GetInt32(0),
                            DependentID = dataReader.GetInt32(3),
                            DependentName = dataReader.GetString(4),
                            DependentBirth = dataReader.GetDateTime(5),
                            DependentBlood = dataReader.GetString(6),
                            DependentAllergy = dataReader.GetString(7),
                            DependentSusNo = dataReader.GetString(8)
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
        public IActionResult Post([FromBody]DependentRequest request)
        {
            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);
            int DependentID = 0;

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("INSERT INTO Dependent (DependentName, DependentBirth, DependentBlood, DependentAllergy, DependentSusNo, UserID) VALUES(@DependentName, @DependentBirth, @DependentBlood, @DependentAllergy, @DependentSusNo, @UserID)", conn))
                {

                    cmd.Parameters.AddWithValue("@DependentName", request.DependentName);
                    cmd.Parameters.AddWithValue("@DependentBirth", request.DependentBirth);
                    cmd.Parameters.AddWithValue("@DependentBlood", request.DependentBlood);
                    cmd.Parameters.AddWithValue("@DependentAllergy", request.DependentAllergy);
                    cmd.Parameters.AddWithValue("@DependentSusNo", request.DependentSusNo);
                    cmd.Parameters.AddWithValue("@UserID", request.UserID);

                    cmd.ExecuteNonQuery();

                    using (MySqlCommand cmd2 = new MySqlCommand("SELECT last_insert_id()", conn))
                    {
                        DependentID = (int)(ulong)cmd2.ExecuteScalar();
                    }
                }

                return new OkObjectResult(new DependentResult { DependentID = DependentID, DependentName = request.DependentName, DependentBirth = request.DependentBirth, DependentBlood = request.DependentBlood, DependentAllergy = request.DependentAllergy, DependentSusNo = request.DependentSusNo, UserID = request.UserID });

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

        [HttpPut("{DependentID}")]
        public IActionResult Put(int DependentID, [FromBody]DependentRequest request)
        {
            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);
            DependentResult result = new DependentResult();

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("UPDATE Dependent SET DependentName = @Dependentname, DependentBirth = @DependentBirth, DependentBlood = @DependentBlood, DependentAllergy = @DependentAllergy, DependentSusNo = @DependentSusNo WHERE DependentID = @DependentID", conn))
                {

                    cmd.Parameters.AddWithValue("@DependentName", request.DependentName);
                    cmd.Parameters.AddWithValue("@DependentBirth", request.DependentBirth);
                    cmd.Parameters.AddWithValue("@DependentBlood", request.DependentBlood);
                    cmd.Parameters.AddWithValue("@DependentAllergy", request.DependentAllergy);
                    cmd.Parameters.AddWithValue("@DependentSusNo", request.DependentSusNo);
                    cmd.Parameters.AddWithValue("@DependentID", DependentID);//o label em cima tem o nome deste aqui! Tanto faz o nome pois é local

                    cmd.ExecuteNonQuery();
                    //O que está aqui dentro é só codigo de BD, ou seja, passa os valores para a query!
                }

                result.DependentID = DependentID;
                result.DependentName = request.DependentName;
                result.DependentBirth = request.DependentBirth;
                result.DependentBlood = request.DependentBlood;
                result.DependentAllergy = request.DependentAllergy;
                result.DependentSusNo = request.DependentSusNo;

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
   
        [HttpDelete("{DependentID}")]
        public IActionResult Delete(int DependentID)
        {

            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("DELETE FROM Dependent WHERE DependentID = @DependentID", conn))
                {
                    cmd.Parameters.AddWithValue("@DependentID", DependentID);

                    cmd.ExecuteNonQuery();
                }

                return new OkResult();
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
