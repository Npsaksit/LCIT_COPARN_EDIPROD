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

namespace cvfile
{
    public class convertISO
    {
        static sqlcon.Connection_String constr = new sqlcon.Connection_String();
        static writeToFile.writeLogFile savelog = new writeToFile.writeLogFile();


        public string iso_check(string a, string flenm)
        {

            string sizeCNTR = "";
            string log = "";
            // Method for check ISO code for show container size and type           
            OleDbConnection con = new OleDbConnection();
            con.ConnectionString = constr.connectionString;
            OleDbDataAdapter sda = new OleDbDataAdapter("SELECT (SIZETYPE_SIZE_AN||SIZETYPE_TYPE_AN||SIZETYPE_HEIGHT_AN) SITY FROM MIS_OWNER.TMS_CONTAINER_TYPE_SNAP WHERE CONTAINER_TYPE_C ='" + a + "'", con);
            DataTable dt = new DataTable();
            dt.TableName = "TestISO";
            sda.Fill(dt);
            try
            {
                sizeCNTR = dt.Rows[0]["SITY"].ToString();
            }
            catch (Exception ex)
            {
                sizeCNTR = "XXXXXX";
                log = "ONE |"+ DateTime.Now.ToString()+"  --------------------- | ERROR : ISO Not found in DB  " + a.ToString() + "in" + flenm.ToString();

                savelog.toLogfile(log);
            }

            return sizeCNTR;
        }
    }
}




