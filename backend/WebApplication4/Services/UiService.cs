using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Services
{
    public class UiService
    {

        public IConfiguration Configuration { get; }

        public UiService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public List<Dictionary<string, object>> GetGridData()
        {
            var Uidata = new List<Dictionary<string, object>>();
            string DefaultConnectionString = Configuration.GetConnectionString("DefaultConnectionString");
            string queryString = @" SELECT DATETIME_KEY,NETYPE,NEALIAS,LINK,
                                    sum(SLOT) AS 'SUM_SLOT',
                                    MAX(MAX_RX_LEVEL) AS 'MAXRXLEVEL',
                                    MAX(MAX_TX_LEVEL) AS 'MAXTXLEVEL',
                                    ABS(""MAXRXLEVEL"") - ABS(""MAXTXLEVEL"") AS 'RSLDEVIATION'
                                    FROM TRANS_MW_AGG_SLOT_HOURLY
                                    GROUP BY DATETIME_KEY,NEALIAS,NETYPE,LINK; ";


            using (OdbcConnection connection = new OdbcConnection(DefaultConnectionString))
            {
                OdbcCommand command = new OdbcCommand(queryString, connection); connection.Open(); // Execute the DataReader and access the data.
                OdbcDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var newRow = new Dictionary<string, object>
                    {
                        { "DATETIME_KEY", (DateTime)reader[0] },
                        { "NETYPE", (string)reader[1] },
                        { "NEALIAS", (string)reader[2] },
                        { "LINK", (string)reader[3] },
                        { "SLOT", (Int64)reader[4] },
                        { "MAX_RX_LEVEL", (double)reader[5] },
                        { "MAX_TX_LEVEL", (double)reader[6] },
                        {"RSL_DEVIATION" ,(double)reader[7]},
                        
                    };
                    Uidata.Add(newRow);

                    
                } // Call Close when done reading.
                reader.Close();
                return Uidata;

            }

        }


    }
}
