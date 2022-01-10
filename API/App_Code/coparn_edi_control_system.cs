using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Web;
using System.Web.Services;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Text;
using System.IO;
using System.Web.Script.Serialization;
using System.Web.Services.Protocols;
using System.Reflection;

namespace coparn_edi_system
{
    [WebService(Namespace = "http://lcitedi.lcit.com")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]



    public class coparn_edi_control_system : System.Web.Services.WebService
    {
        sqlcon.Connection_String constr = new sqlcon.Connection_String();
        convert_json.jsoncv convertFormat = new convert_json.jsoncv();
        cvfile.convertISO cviso = new cvfile.convertISO();

        exportfile.exportor ep = new exportfile.exportor();

        [WebMethod]
        public string translateCoparn(String operatorCode)
        {
            string result;

            Type type = Type.GetType(operatorCode);
            MethodInfo method = type.GetMethod("startUp");
            result = (string)method.Invoke(this, new object[] { operatorCode });


            return result;
        }

        [WebMethod]
        public string isoConvert(String a,String line)
        {
            string result;


            result = (string)cviso.iso_check(a, "",line);



            return result;

            // string sizeCNTR = "";
            // // string log = "";
            // // Method for check ISO code for show container size and type           
            // SqlConnection con = new SqlConnection();
            // con.ConnectionString = constr.connectionLCIT_EDI_COPARN;
            // SqlDataAdapter sda = new SqlDataAdapter("SELECT CONCAT(SIZE,TYPE,HEIGHT) FROM LCIT_EDI_COPARN.DBO.CNTR_TYPE_MAPPING WHERE [ISO_CODE] = '" + a + "' AND LINE = '"+line+"'", con);
            // DataTable dt = new DataTable();
            // dt.TableName = "TestISO";
            // sda.Fill(dt);
            // try
            // {
            //     sizeCNTR = dt.Rows[0]["SITY"].ToString();
            // }
            // catch (Exception ex)
            // {
            //     sizeCNTR = a;
            //     // log = "ONE |"+ DateTime.Now.ToString()+"  --------------------- | ERROR : ISO Not found in DB  " + a.ToString() + "in" + flenm.ToString();

            //     // savelog.toLogfile(log);
            // }

            // return sizeCNTR;
        }

        [WebMethod]
        public string getDT(String operatorCode, String msgDetail)
        {
            string result = (string)ep.getdata(operatorCode, msgDetail);
            return result;
        }


    }


}
