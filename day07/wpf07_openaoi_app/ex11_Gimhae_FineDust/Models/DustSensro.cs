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
        public int State { get; set; }
        public DateTime Timestamp { get; set; }
        public string Company_id { get; set; }
        public string Company_Name { get; set; }

    }
}
