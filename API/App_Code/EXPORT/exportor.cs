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


namespace exportfile
{
    public class exportor
    {
        static SqlConnection con = new SqlConnection();
        static sqlcon.Connection_String sqlsrvconnect = new sqlcon.Connection_String();
        static convert_json.jsoncv convertFormat = new convert_json.jsoncv();
        public string getdata(String OperatorCode, String msgDetail)
        {

            con.ConnectionString = sqlsrvconnect.connectionLCIT_EDI_COPARN;
            con.Open();
            SqlDataAdapter sdaQuery = new SqlDataAdapter("SELECT [No], Booking_No, VESSEL_NAME, VOYAGE, OPERATOR_CODE, POL, POD, DST, CMDT, CNTR_NUM, [SIZE], [TYPE], HEIGHT, SS, LADENT_SATAUS, VGM, VGM_UOM, GROSSWEIGHT, GROSSWEIGHT_UOM, SHIPPER_NAME, CONSIGNEE_NAME, SEAL, STOW_CODE, BLOCK_STOWAGE, VENTILATION, TMP, TMPUOM, DG_CLASS, OOG_OH, OOG_OF, OOG_OA, OOG_OL, OOG_OR, GATE_ACTIVITY_CODE, PAYMENT, MODIFY_DATE, RECORD_STATUS FROM LCIT_EDI_COPARN.dbo.COPARN_EDIDATA WHERE RECORD_STATUS IN ('" + msgDetail.ToString() + "') AND OPERATOR_CODE LIKE '" + OperatorCode.ToString() + "'", con);
            DataTable dtQuery = new DataTable();

            dtQuery.TableName = "COPARN " + OperatorCode;
            sdaQuery.Fill(dtQuery);


            con.Close();



            return convertFormat.convertToJson(dtQuery);
        }

        public string exportToFile()
        {
            return "Test";
        }
    }
}