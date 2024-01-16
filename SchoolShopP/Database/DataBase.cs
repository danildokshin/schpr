using SchoolShopP.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolShopP.DB
{
    public class DataBase
    {
        /// <summary>
        /// Строка обращения к БД
        /// </summary>
        public static ClassWorkEntities entities = new ClassWorkEntities();
    }
}
