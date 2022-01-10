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


        public string iso_check(string a, string flenm,string line)
        {

            string sizeCNTR = "";
             string log = "";
            // Method for check ISO code for show container size and type           
            SqlConnection con = new SqlConnection();
            con.ConnectionString = constr.connectionLCIT_EDI_COPARN;
            SqlDataAdapter sda = new SqlDataAdapter("SELECT CONCAT(SIZE,TYPE,HEIGHT) FROM LCIT_EDI_COPARN.DBO.CNTR_TYPE_MAPPING WHERE [ISO_CODE] = '" + a + "' AND LINE = '"+line+"'", con);
            DataTable dt = new DataTable();
            dt.TableName = "TestISO";
            sda.Fill(dt);
            try
            {
                sizeCNTR = dt.Rows[0]["SITY"].ToString();

                log = "SELECT CONCAT(SIZE,TYPE,HEIGHT) FROM LCIT_EDI_COPARN.DBO.CNTR_TYPE_MAPPING WHERE [ISO_CODE] = '" + a + "' AND LINE = '"+line+"'";
            }
            catch (Exception ex)
            {
                sizeCNTR = a;
                 log = "ONE |"+ DateTime.Now.ToString()+"  --------------------- | ERROR : ISO Not found in DB  " + a.ToString() + "in" + flenm.ToString();

                 savelog.toLogfile(log);
            }

            return sizeCNTR;
        }
    }
}




