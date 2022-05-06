using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using WebApplication4.Models;
using System.Windows;
using Xceed.Wpf.Toolkit;

namespace WebApplication4.Services
{
    public class ParserService
    {
        public IConfiguration Configuration { get; }

        public ParserService(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // Parser
        public void ReadData()
        {
            string sourceFolder = Configuration.GetValue<string>("InFolderPath");
            string parsedFolder = Configuration.GetValue<string>("PreviouslyParsed");

            DirectoryInfo directoryInfo = new DirectoryInfo(sourceFolder);
            FileInfo[] files = directoryInfo.GetFiles("*.txt");

            foreach (FileInfo file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file.Name);
                string sourceName = Path.GetFileName(file.Name);
                DateTime dateTime = getDate(fileName);

                // value holding the result of select from log table where filename = file.name id  and action = parsed
                string query2 = "SELECT Count(*) FROM LOGGER WHERE FILE_Name = '" + fileName + "'";

                QueryExecute(query2);
                string value = QueryExecuteCheck(query2);
                int returnedValue = int.Parse(value);

                if(returnedValue == 0)
                {

                    // if value has any data -> continue to the next file inside the directory folder 
                    // else (means that the query did not return any value) so the file is not parsed before
                    // parse this file and insert it's unique id inside my log table


                    string[] lines = File.ReadAllLines(file.FullName);
                    lines = lines.Skip(1).ToArray();

                    var List = new List<FileList>();

                    foreach (var line in lines)
                    {
                        var element = line.Split(',');
                        if (element[2] != "Unreachable Bulk FC"
                            && element[17] == "-")
                        {
                            // creating network_sid, hash should return an integer!

                            string concatenatedvalue = element[2] + element[6];
                            var networkSID = hashedValue(concatenatedvalue);

                            // Link Column
                            string objectValue = element[2];
                            string sub = objectValue.Substring(0, objectValue.IndexOf("_"));
                            char dot = '.';
                            char plusSign = '+';
                            string link;

                            if (sub.Contains(dot) && !sub.Contains(plusSign))
                            {
                                string substring1;
                                string substring2;

                                int efrom = sub.IndexOf("/") + "/".Length;
                                int eto = sub.LastIndexOf(".");
                                substring1 = sub.Substring(efrom, eto - efrom);
                                int sfrom = sub.IndexOf(".") + ".".Length;
                                int sto = sub.LastIndexOf("/");
                                substring2 = sub.Substring(sfrom, sto - sfrom);

                                link = substring1 + "/" + substring2;
                            }

                            else if (sub.Contains(dot) && sub.Contains(plusSign))
                            {
                                string substring1;
                                string substring2;
                                string substring3;
                                int efrom = sub.IndexOf("/") + "/".Length;
                                int eto = sub.IndexOf(".");
                                substring1 = sub.Substring(efrom, eto - efrom);
                                int sfrom = sub.IndexOf("+") + "+".Length;
                                int sto = sub.LastIndexOf(".");
                                substring2 = sub.Substring(sfrom, sto - sfrom);
                                substring3 = sub.Substring(sub.LastIndexOf("/") + "/".Length);

                                link = substring1 + "+" + substring2 + "/" + substring3;
                            }

                            else
                            {
                                int efrom = sub.IndexOf("/") + "/".Length;
                                link = sub.Substring(efrom);

                            }

                            //TID
                            string tid;
                            int pfrom = objectValue.IndexOf("_") + 1 + "_".Length;
                            int pto = objectValue.LastIndexOf("_") - 1;
                            tid = objectValue.Substring(pfrom, pto - pfrom);

                            //FARENDTID
                            string farendtid;
                            farendtid = objectValue.Substring(objectValue.LastIndexOf("_") + "_".Length);

                            // SLOT/PORT


                            string substring11;
                            string substring22;
                            string[] arrayofitems = new string[2];
                            string port = "1";

                            if (sub.Contains(plusSign))
                            {

                                int eefrom = sub.IndexOf("/") + "/".Length;
                                int eeto = sub.LastIndexOf("+");
                                substring11 = sub.Substring(eefrom, eeto - eefrom);
                                int ssfrom = sub.IndexOf("+") + "+".Length;
                                int ssto = sub.LastIndexOf("/");
                                substring22 = sub.Substring(ssfrom, ssto - ssfrom);
                                arrayofitems[0] = substring11;
                                arrayofitems[1] = substring22;

                                for (int t = 0; t < 2; t++)
                                {
                                    var ParsedList = new FileList()
                                    {
                                        Network_SID = networkSID,
                                        DateTime_Key = dateTime,
                                        NEID = element[1],
                                        Object = element[2],
                                        Time = DateTime.Parse(element[3]),
                                        Interval = int.Parse(element[4]),
                                        Direction = element[5],
                                        NeAlias = element[6],
                                        NeType = element[7],
                                        RxLevelBelowTS1 = element[9],
                                        RxLevelBelowTS2 = element[10],
                                        MinRxLevel = float.Parse(element[11]),
                                        MaxRxLevel = float.Parse(element[12]),
                                        TxLevelAboveTS1 = element[13],
                                        MinTxLevel = float.Parse(element[14]),
                                        MaxTxLevel = float.Parse(element[15]),
                                        FailureDescription = element[17],
                                        Link = link,
                                        TID = tid,
                                        FARENDTID = farendtid,
                                        Slot = arrayofitems[t],
                                        Port = port,
                                        FileName = fileName
                                    };

                                    List.Add(ParsedList);
                                }

                            }

                            else
                            {
                                int p1 = sub.IndexOf("/") + "/".Length;
                                int p2 = sub.LastIndexOf("/");
                                string slot = sub.Substring(p1, p2 - p1);

                                var ParsedList = new FileList()
                                {
                                    Network_SID = networkSID,
                                    DateTime_Key = dateTime,
                                    NEID = element[1],
                                    Object = element[2],
                                    Time = DateTime.Parse(element[3]),
                                    Interval = int.Parse(element[4]),
                                    Direction = element[5],
                                    NeAlias = element[6],
                                    NeType = element[7],
                                    RxLevelBelowTS1 = element[9],
                                    RxLevelBelowTS2 = element[10],
                                    MinRxLevel = float.Parse(element[11]),
                                    MaxRxLevel = float.Parse(element[12]),
                                    TxLevelAboveTS1 = element[13],
                                    MinTxLevel = float.Parse(element[14]),
                                    MaxTxLevel = float.Parse(element[15]),
                                    FailureDescription = element[17],
                                    Link = link,
                                    TID = tid,
                                    FARENDTID = farendtid,
                                    Slot = slot,
                                    Port = port,
                                    FileName = fileName
                                };

                                List.Add(ParsedList);
                            }
                        }

                    }
                    using (StreamWriter sw = new StreamWriter($"C:\\Users\\User\\Desktop\\ParsedData\\{fileName}.txt"))
                    {
                        foreach (var ln in List)
                        {
                            sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22}", ln.Network_SID, ln.DateTime_Key, ln.NEID, ln.Object, ln.Time, ln.Interval, ln.Direction, ln.NeAlias, ln.NeType, ln.RxLevelBelowTS1, ln.RxLevelBelowTS2, ln.MinRxLevel, ln.MaxRxLevel, ln.TxLevelAboveTS1, ln.MinTxLevel, ln.MaxTxLevel, ln.FailureDescription, ln.Link, ln.TID, ln.FARENDTID, ln.Slot, ln.Port, ln.FileName));
                        }
                    }

                    string query = @"insert into logger (FILE_NAME, ACTION_NAME, ACTION_DATE)
                               values('" + fileName + "', 'parsed', getdate()) ";

                    QueryExecute(query);

                    File.Move(file.FullName, $"{parsedFolder}{file.Name}");

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

        public DateTime getDate(string fileName)
        {
            string[] splittedEl = fileName.Split("_");

            string hour = splittedEl[splittedEl.Length - 1];
            string date = splittedEl[splittedEl.Length - 2];

            string dateTime = $"{date} {hour}";
            string dateFormat = "yyyyMMdd hhmmss";

            DateTime dateTime_Key = DateTime.ParseExact(dateTime, dateFormat, null);

            return dateTime_Key;
        }

        public int hashedValue(string toBeHashed)
        {
            using (var md5hash = MD5.Create())
            {
                var hashedvalue = Encoding.UTF8.GetBytes(toBeHashed);

                var hashbytes = md5hash.ComputeHash(hashedvalue);

                var hash = BitConverter.ToInt32(hashbytes);

                return hash;

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