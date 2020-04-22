using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFileConverter.vo
{
    class LvInfoItem
    {
        public int index { get; set; }
        public String sourceString { get; set; }
        public String destnationString { get; set; }

        public static List<LvInfoItem> instance;

        public static List<LvInfoItem> getInstance()
        {
            if ( instance == null)
            {
                instance = new List<LvInfoItem>();
            }
            return instance;
        }
    }
}
