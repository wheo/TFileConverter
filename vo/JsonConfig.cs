using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFileConverter.vo
{
    class JsonConfig
    {
        public String CurrentInputPath { get; set; }
        public String CurrentOutputPath { get; set; }

        public bool IsRecursive { get; set; }

        public String configFileName = "config.json";

        public static JsonConfig instance;

        public static JsonConfig getInstance()
        {
            if ( instance == null)
            {
                instance = new JsonConfig();
            }
            return instance;
        }
    }
}
