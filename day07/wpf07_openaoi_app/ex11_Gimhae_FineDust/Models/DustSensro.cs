using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ex11_Gimhae_FineDust.Models
{
    public class DustSensro
    {
        public int Id { get; set; }
        public string Dev_id { get; set; }
        public string Name { get; set; }
        public string Loc { get; set; }
        public double Coordx { get; set;}
        public double Coordy { get; set;}
        public bool Ison {  get; set; }
        public int Pm10_after {  get; set; }
        public int Pm25_after {  get; set; }
        public int State { get; set; }
        public DateTime Timestamp { get; set; }
        public string Company_id { get; set; }
        public string Company_name { get; set; }

        public static readonly string INSERT_QUERY = @"INSERT INTO [dbo].[Dustsensor]
                                                                   ([Dev_id]
                                                                   ,[Name]
                                                                   ,[Loc]
                                                                   ,[Coordx]
                                                                   ,[Coordy]
                                                                   ,[Ison]
                                                                   ,[Pm10_after]
                                                                   ,[Pm25_after]
                                                                   ,[State]
                                                                   ,[Timestamp]
                                                                   ,[Company_id]
                                                                   ,[Company_name])
                                                             VALUES
                                                                   (@Dev_id
                                                                   ,@Name
                                                                   ,@Loc
                                                                   ,@Coordx
                                                                   ,@Coordy
                                                                   ,@Ison
                                                                   ,@Pm10_after
                                                                   ,@Pm25_after
                                                                   ,@State
                                                                   ,@Timestamp
                                                                   ,@Company_id
                                                                   ,@Company_name)";
        public static readonly string SELECT_QUERY = @"SELECT [Id]
                                                              ,[Dev_id]
                                                              ,[Name]
                                                              ,[Loc]
                                                              ,[Coordx]
                                                              ,[Coordy]
                                                              ,[Ison]
                                                              ,[Pm10_after]
                                                              ,[Pm25_after]
                                                              ,[State]
                                                              ,[Timestamp]
                                                              ,[Company_id]
                                                              ,[Company_name]
                                                          FROM [dbo].[Dustsensor]
                                                         WHERE CONVERT(CHAR(10), GETDATE(), 23) = @Timestamp";


        public static readonly string GETDATE_QUERY = @"SELECT CONVERT(CHAR(10), Timestamp, 23) AS Save_Date
                                                                  FROM [dbo].[DustSensor]
                                                                 GROUP BY CONVERT(CHAR(10), Timestamp, 23)";
                                                                    }
}
