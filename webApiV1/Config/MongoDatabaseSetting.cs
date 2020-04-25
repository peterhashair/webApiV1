using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webApiV1.Config
{
    public class MongoDatabaseSetting :IMongoDatabaseSetting
    {

        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

     
    }

    public interface IMongoDatabaseSetting
    {
         string ConnectionString { get; set; }

         string DatabaseName { get; set; }

    }
}
