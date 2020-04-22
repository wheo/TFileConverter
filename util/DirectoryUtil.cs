using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TFileConverter.util
{
    class DirectoryUtil
    {
        static public void SearchDirectory(String strDir, SearchOption option)
        {
            try
            {
                foreach(String d in Directory.GetDirectories(strDir))
                {
                    foreach(String f in Directory.GetFiles(d))
                    {
                        //파일 목록
                        System.Diagnostics.Debug.WriteLine(f.ToString());
                        // 파일 open
                        // text 파일인지 check
                        // 한줄씩 읽어서 Replace
                        
                    }
                    if ( option == SearchOption.Recursive)
                    {
                        SearchDirectory(d, SearchOption.Recursive);
                    }
                }
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }

        }

        static public void ReplaceText(String srcWord, String dstWord, String srcPath, String gdstPath)
        {

        }

        public enum SearchOption
        {
            Recursive,
            NonRecursive
        }
    }
}
