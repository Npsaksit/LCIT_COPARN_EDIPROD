using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Text;
using System.IO;

using System.Reflection;

namespace writeToFile
{

    public class writeLogFile
    {
        static keepfile.flepath fpath = new keepfile.flepath();

        public string toLogfile(string Message)
        {
            if (!Directory.Exists(fpath.log.ToString()))
            {
                Directory.CreateDirectory(fpath.log.ToString());
            }

            string filepath = fpath.log.ToString() + "ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }

            return "completed";
        }
    }
}