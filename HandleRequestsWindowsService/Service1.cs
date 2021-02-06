using HandleRequestsWindowsService.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
            timer.Interval = 10000; //number in milisecinds  
            timer.Enabled = true;
        }
        protected override void OnStop()
        {
            WriteToFile("Service is stopped at " + DateTime.Now);
        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            List<Promotion> promotions = GetUnhandledRequests();
            WriteToFile("Unhandeled Requests " + promotions.ToString());
            WriteToFile("Service is recall at " + DateTime.Now);
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
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
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
    }
}
