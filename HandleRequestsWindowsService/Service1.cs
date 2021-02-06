using HandleRequestsWindowsService.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace HandleRequestsWindowsService
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer(); // name space(using System.Timers;)  
        SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Promotions_Entities"].ConnectionString);
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            WriteToFile("Service is started at " + DateTime.Now);
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 100000; //number in milisecinds  
            timer.Enabled = true;
        }
        protected override void OnStop()
        {
            WriteToFile("Service is stopped at " + DateTime.Now);
        }
        private async void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            WriteToFile("Service is recall at " + DateTime.Now);

            //Get the un-handled Request records from Request table
            List<Promotion> promotions = GetUnhandledRequests();
            WriteToFile("Unhandeled Requests count :  " + promotions.Count() + " request.");


            //Call API HandleRequest to handle the Requests 
            var promotionRequests = promotions.Select(item => new HandlePromotionInputDTO
            {
                MobileNumber = item.MobileNumber
            }).ToList();
            if (await HandledRequests(promotionRequests))
                WriteToFile("Successfully handled ... ");
            else
                WriteToFile("failed to handle ... ");

        }
        public void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter streamWriter = File.CreateText(filepath))
                {
                    streamWriter.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter streamWriter = File.AppendText(filepath))
                {
                    streamWriter.WriteLine(Message);
                }
            }
        }
        public List<Promotion> GetUnhandledRequests()
        {
            string SqlString = "select * from Promotion where Promotion.IsHandled = 0";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(SqlString, sqlConnection);
            DataTable dataTable = new DataTable();
            try
            {
                sqlConnection.Open();
                sqlDataAdapter.Fill(dataTable);
            }
            catch (SqlException se)
            {
                WriteToFile("sql error : " + se);
            }
            finally
            {
                sqlConnection.Close();
            }
            //map data table into promotion list
            List<Promotion> promotions = new List<Promotion>();
            promotions = (from DataRow dr in dataTable.Rows
                          select new Promotion()
                          {
                              RequestId = Convert.ToInt32(dr["RequestId"]),
                              Action = dr["Action"].ToString(),
                              MobileNumber = Convert.ToInt32(dr["MobileNumber"]),
                              IsHandled = Convert.ToBoolean(dr["IsHandled"])
                          }).ToList();
            return promotions;
        }
        public async Task<bool> HandledRequests(List<HandlePromotionInputDTO> inputDtos)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:55587/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //post Method 
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(inputDtos));
                HttpResponseMessage response = await client.PostAsync("api/HandleRequests", stringContent);
                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    StreamReader streamReader = new StreamReader(stream);
                    var content = streamReader.ReadToEnd();
                    if (!string.IsNullOrWhiteSpace(content))
                    {
                        GeneralResponseDto responseDto = JsonConvert.DeserializeObject<GeneralResponseDto>(content);
                        if (responseDto.status == 1)
                            return true;
                        else
                            return false;
                    }
                }
                return false;
            }
        }
    }
}
