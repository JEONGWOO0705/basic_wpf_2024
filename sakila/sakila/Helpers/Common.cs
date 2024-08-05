using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace sakila.Helpers
{
   
    internal class Common
    {

        public static readonly string CONNSTRING = "Server=localhost;"+
                                                    "Port=3306;"+
                                                    "Database=sakila;"+
                                                    "Uid=root;" +
                                                    "Pwd=1234";


    }
}
