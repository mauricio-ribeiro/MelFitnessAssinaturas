using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelFitnessAssinaturas.DAL
{
    public class BaseDAL
    {
        protected SqlConnection conn;
        protected SqlDataReader reader = null;
    }
}
