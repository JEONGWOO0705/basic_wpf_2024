using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_MiniProject.Models
{
    class WarningMessage
    {
        //public int Id { get; set; }

        //public double total {  get; set; }
        //public string gugunNm {  get; set; }
        //public double gugunWithWalk {  get; set; }
        //public string checkDate { get; set; }
        //public string endSpot { get; set; }
        //public string startSpot {  get; set; }
        //public int Id { get; set; }
        public DateTime create_date {  get; set; }
        public string location_id {  get; set; }
        public string location_name {  get; set; }
        public int md101_sn {  get; set; }
        public string msg {  get; set; }


        public static readonly string INSERT_QUERY = @"INSERT INTO [dbo].[warningMessage]
                                                                       (
                                                                       [create_date]
                                                                       ,[location_id]
                                                                       ,[location_name]
                                                                       ,[md101_sn]
                                                                       ,[msg])
                                                                 VALUES
                                                                       (
                                                                       @create_date
                                                                       ,@location_id
                                                                       ,@location_name
                                                                       ,@md101_sn
                                                                       ,@msg)";

        public static readonly string SELECT_QUERY = @"SELECT [create_date]
                                                              ,[location_id]
                                                              ,[location_name]
                                                              ,[md101_sn]
                                                              ,[msg]
                                                          FROM [dbo].[warningMessage]"
                                                          ;

        public static readonly string DELETE_QUERY = @"DELETE FROM [dbo].[warningMessage] WHERE md101_sn = @md101_sn";

        //public static readonly string GETDATE_QUERY = @"SELECT CONVERT(CHAR(10), create_date, 23) AS Save_Date
        //                                                          FROM [dbo].[warningMessage]
        //                                                         GROUP BY CONVERT(CHAR(10), create_date, 23)";


    }
}
