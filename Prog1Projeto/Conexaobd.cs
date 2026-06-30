using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Prog1Projeto
{
    public static class Conexaobd
    {
        public static MySqlConnection fazerconexao()
        {
            MySqlConnection conectar = new MySqlConnection("server=127.0.0.1; database=biblioteca; uid=root; pwd=;");
            return conectar;
        }
    }
}