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
    public class UserController : Controller
    {
        private readonly Appsettings _appSettings;

        public UserController(Appsettings appSettings)
        {
            _appSettings = appSettings;
        }

        [HttpGet]
        public IActionResult Get()
        {
            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);
            List<UserResult> result = new List<UserResult>();

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("SELECT UserName, UserCpf, UserEmail FROM User", conn))
                {
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        result.Add(new UserResult
                        {
                            UserID = dataReader.GetInt32(0),
                            UserName = dataReader.GetString(1),
                            UserCpf = dataReader.GetString(2),
                            UserEmail = dataReader.GetString(3),

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

        [HttpGet("{UserCpf}")]
        public IActionResult Get(string UserCpf)
        {
            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);
            UserDependentResult result = null;
            List<DoseResult> listDoses = new List<DoseResult>();

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("SELECT User.UserID, User.UserName, Dependent.DependentID, Dependent.DependentName, Dependent.DependentBirth, Dependent.DependentBlood, Dependent.DependentAllergy, Dependent.DependentSusNo, Vaccine.VaccineID, Vaccine.VaccineName, Vaccine_Dep.VaccineDate, Dose.DoseId, Dose.DoseType FROM User INNER JOIN dependent ON(User.UserID = dependent.User_UserID) INNER JOIN vaccine_dep on (dependent.DependentID = vaccine_dep.DependentID) INNER JOIN vaccine ON (vaccine_dep.VaccineID = vaccine.VaccineID) INNER JOIN dose on (Vaccine.VaccineID = dose.VaccineID) WHERE User.UserCpf LIKE '%" + UserCpf +"%'", conn))
                {
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        if (result == null)
                        {
                            result = new UserDependentResult
                            {
                                UserID = dataReader.GetInt32(0),
                                UserName = dataReader.GetString(1)
                            };

                            result.Dependentes = new List<DependentVaccineResult>();

                        }
                        
                            List<VaccineDoseResult> listVaccineResult = new List<VaccineDoseResult>();

                            result.Dependentes.Add(new DependentVaccineResult
                            {
                                UserID = dataReader.GetInt32(0),
                                DependentID = dataReader.GetInt32(2),
                                DependentName = dataReader.GetString(3),
                                DependentBirth = dataReader.GetDateTime(4),
                                DependentBlood = dataReader.GetString(5),
                                DependentAllergy = dataReader.GetString(6),
                                DependentSusNo = dataReader.GetString(7),
                                Vacinas = listVaccineResult
                            });

                            listVaccineResult.Add(new VaccineDoseResult
                            {
                                VaccineID = dataReader.GetInt32(8),
                                VaccineName = dataReader.GetString(9),
                                VaccineDate = dataReader.GetDateTime(10),
                                Doses = listDoses
                            });

                            listDoses.Add(new DoseResult
                            {
                                DoseID = dataReader.GetInt32(11),
                                DoseType = dataReader.GetString(12)
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
        
    }
}
