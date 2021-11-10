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
        public string isoConvert(String iso)
        {
            string result;


            result = (string)cviso.iso_check(iso, null);



            return result;
        }

        [WebMethod]
        public string getDT(String operatorCode, String msgDetail)
        {
            string result = (string)ep.getdata(operatorCode, msgDetail);
            return result;
        }


    }


}
