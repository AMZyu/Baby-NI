using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Services
{
    public class AggregatorService
    {
        public IConfiguration Configuration { get; }

        public AggregatorService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void AggregateData()
        {
           
            string loadedFolder = Configuration.GetValue<string>("LoadedFolder");
            DirectoryInfo directoryInfo = new DirectoryInfo(loadedFolder);
            FileInfo[] files = directoryInfo.GetFiles("*.txt");

            foreach(FileInfo file in files){

                string fileName = Path.GetFileNameWithoutExtension(file.Name);

                string query = @"INSERT INTO TRANS_MW_AGG_SLOT_HOURLY (
                            DATETIME_KEY, NETYPE, NEALIAS, LINK, SLOT,
                            MAX_RX_LEVEL,
                            MAX_TX_LEVEL,
                            RSL_DEVIATION,
                            FILE_NAME
                            )
                            SELECT DATE_TRUNC('HOUR', TIME_),
                            NETYPE, NEALIAS, LINK, SLOT,
                            MAX(MAXRXLEVEL) AS 'RX LEVEL', 
                            MAX(MAXTXLEVEL) AS 'TX LEVEL',
                            ABS(""RX LEVEL"") - ABS(""TX LEVEL"") AS 'RSL DEVIATION',
                            FILE_NAME
                            FROM TRANS_MW_ERC_PM_TN_RADIO_LINK_POWER
                            WHERE FILE_NAME NOT IN ( SELECT FILE_NAME FROM TRANS_MW_AGG_SLOT_HOURLY)
                            GROUP BY 1, 2, 3, 4,5, FILE_NAME;";
                QueryExecute(query);

                string query2 = @"INSERT INTO TRANS_MW_AGG_SLOT_DAILY (
                            DATETIME_KEY, NETYPE, NEALIAS, LINK, SLOT,
                            MAX_RX_LEVEL,
                            MAX_TX_LEVEL,
                            RSL_DEVIATION,
                            FILE_NAME
                            )
                            SELECT DATE_TRUNC('DAY', DATETIME_KEY), NETYPE, NEALIAS, LINK, SLOT,
                            MAX(MAX_RX_LEVEL) AS 'RX LEVEL', 
                            MAX(MAX_TX_LEVEL) AS 'TX LEVEL',
                            ABS(""RX LEVEL"") - ABS(""TX LEVEL"") AS 'RSL DEVIATION',
                            FILE_NAME
                            FROM TRANS_MW_AGG_SLOT_HOURLY
                            WHERE FILE_NAME NOT IN (SELECT FILE_NAME FROM TRANS_MW_AGG_SLOT_DAILY)
                            GROUP BY 1, 2, 3, 4, 5, FILE_NAME; ";
                QueryExecute(query2);

                string query3 = @"INSERT INTO LOGGER(FILE_NAME, ACTION_NAME, ACTION_DATE)
                              VALUES('" + fileName + "', 'Aggregated', GETDATE())";
                QueryExecute(query3);
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

    }
}
