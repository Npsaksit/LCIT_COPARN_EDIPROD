using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Data.OleDb;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.IO;
using System.Reflection;

namespace keepfile
{
    public class flepath
    {
        public DirectoryInfo xlsx = new DirectoryInfo(@"D:\LCIT_EDI_COPARN\EXPORT-XLSX\");
        public DirectoryInfo log = new DirectoryInfo(@"D:\LCIT_EDI_COPARN\LOG\");
        public DirectoryInfo xls = new DirectoryInfo(@"D:\LCIT_EDI_COPARN\EXPORT\");


        // public DirectoryInfo xlsxbackup = new DirectoryInfo(@"D:\LCIT_EDI_COPARN\EXPORT-XLSX\BACKUP\");

    }

}