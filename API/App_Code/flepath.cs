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
using System.Web.Script.Serialization;
using System.Reflection;

namespace keepfile
{
    public class flepath
    {
        public DirectoryInfo getedi_ONE = new DirectoryInfo(@"D:\FTP_CUSTOMERS\ONELINE\");
        public DirectoryInfo moveedi_ONE = new DirectoryInfo(@"D:\FTP_BACKUP\ONELINE\IncorrectVersion\");
        public DirectoryInfo backupedi_ONE = new DirectoryInfo(@"D:\FTP_BACKUP\ONELINE\backup\");
        public DirectoryInfo log = new DirectoryInfo(@"D:\LCIT_EDI_COPARN\LOG\");
    }

}