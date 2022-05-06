using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Services
{
    public class LoaderService
    {
        public IConfiguration Configuration { get; }

        public LoaderService(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        //loader 
        public void LoadData()
        {
            string toBeLoaded = Configuration.GetValue<string>("ToBeLoaded");
            string loadedFolder = Configuration.GetValue<string>("LoadedFolder");
            DirectoryInfo directoryInfo = new DirectoryInfo(toBeLoaded);
            FileInfo[] files = directoryInfo.GetFiles("*.txt");



                foreach (FileInfo file in files)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file.Name);
                    string sourceName = Path.GetFileName(file.Name);
                    string query1 = "SELECT Count(*) FROM LOGGER WHERE FILE_Name = '" + fileName + "' AND ACTION_NAME = 'Loaded' ";
                    string fullPath = toBeLoaded + sourceName;
                     QueryExecute(query1);
                     
                    string value = QueryExecuteCheck(query1);
                    
                    int returnedValue = int.Parse(value);
                    

                    if (returnedValue == 0)
                    {
                  
                         string queryString =
                        "Copy TRANS_MW_ERC_PM_TN_RADIO_LINK_POWER from local '"+ fullPath + "' delimiter ',' REJECTED DATA 'C:\\Users\\User\\Desktop\\RejectedData.txt direct ";
                         QueryExecute(queryString);

                         string queryString2 =
                                @"INSERT INTO LOGGER (FILE_NAME, ACTION_NAME, ACTION_DATE) 
                              VALUES ('" + fileName + "', 'Loaded', GETDATE() )";

                         // HERE check if the data loaded successfully 
                          string rejectedFolder = "C:\\Users\\User\\Desktop\\RejectedData.txt";
                          string[] data = File.ReadAllLines(rejectedFolder);
                          using (StreamReader sr = new StreamReader(rejectedFolder))
                          {
                              string ln;
                                 if ((ln = sr.ReadLine()) == null)
                                 {
                                       // call the query execution function
                                   QueryExecute(queryString2);
                                 }

                          }

                    File.Move(file.FullName, $"{loadedFolder}{file.Name}");

                    }

                    else
                    {
                         Environment.Exit(0);                    
                    }


                }



        }

        public void QueryExecute(string queryString)
        {
            string DefaultConnectionString = Configuration.GetConnectionString("DefaultConnectionString");
            OdbcCommand command = new OdbcCommand(queryString);

            // to connect to DB
            using (OdbcConnection connection = new OdbcConnection(DefaultConnectionString))
            {
                command.Connection = connection;
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();

                // The connection is automatically closed at
                // the end of the Using block.
            }
        }

        public string QueryExecuteCheck(string queryString)
        {
            string returnedValue = "";

            string connetionString = Configuration.GetConnectionString("DefaultConnectionString");

            OdbcCommand command = new OdbcCommand(queryString);

            using (OdbcConnection connection = new OdbcConnection(connetionString))
            {
                command.Connection = connection;
                connection.Open();

                OdbcCommand databaseCommand = connection.CreateCommand();
                databaseCommand.CommandText = queryString;
                OdbcDataReader dataReader = databaseCommand.ExecuteReader();

                while (dataReader.Read())
                {

                    returnedValue = dataReader.GetString(0);

                }
                dataReader.Close();
                connection.Close();
            }

            return returnedValue;
        }
    }
}
