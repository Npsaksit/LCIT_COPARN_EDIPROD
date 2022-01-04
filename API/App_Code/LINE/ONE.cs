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

public class ONE
{
    static keepfile.flepath fpath = new keepfile.flepath();
    static convert_json.jsoncv convertFormat = new convert_json.jsoncv();
    static writeToFile.writeLogFile savelog = new writeToFile.writeLogFile();


    static edi_translate.ONE_translate edi_decode = new edi_translate.ONE_translate();

    static SqlConnection con = new SqlConnection();

    static sqlcon.Connection_String sqlsrvconnect = new sqlcon.Connection_String();

    public static string startUp(String operatorCode)
    {
        string result = "";
        string resultPrint = "";
        string edi_version = "";
        string log = "";
        string[] EDIDATA = null;
        string fileName = "";

        string checkFile = "";


        foreach (FileInfo fi in fpath.getedi_ONE.GetFiles("*.*"))
        {
            fileName = fi.Name.ToString();
            try
            {

                EDIDATA = System.IO.File.ReadAllLines(fpath.getedi_ONE.ToString() + fi.Name);

                if (EDIDATA.Length.ToString() == "1")
                {
                    checkFile = File.ReadAllText(fpath.getedi_ONE.ToString() + fi.Name.ToString());
                    checkFile = checkFile.Replace("'", "'\r\n");
                    File.WriteAllText(fpath.getedi_ONE.ToString() + fi.Name.ToString(), checkFile);

                    EDIDATA = null;
                    EDIDATA = System.IO.File.ReadAllLines(fpath.getedi_ONE.ToString() + fi.Name);

                }
                log += operatorCode + " | " + DateTime.Now.ToString() + " --------------------- | Read file : " + fi.Name + " Successed\r\n";
                // result = (string)edi_decode.translateEDI(EDIDATA);

                edi_version = edi_decode.checkEDIVersion(new string[] { EDIDATA[2] });



                if (edi_version == "COPARN:D:95B")
                {
                    result = "Correct Version";
                    result = (string)edi_decode.translateEDI(EDIDATA);

                    resultPrint += result;


                    File.Move(fpath.getedi_ONE.ToString() + fi.Name.ToString(), fpath.backupedi_ONE.ToString() + fi.Name.ToString());

                    string dbStatus = (string)ToDB(result.Replace("\r\n", ""), fi.Name.ToString());
                    log += operatorCode + " | " + DateTime.Now.ToString() + " --------------------- | Insert DB : " + dbStatus.ToString() + " Record " + " " + fileName + "\r\n";
                }
                else
                {
                    result = "Incorrect Version : " + edi_version.ToString();
                    File.Move(fpath.getedi_ONE.ToString() + fi.Name.ToString(), fpath.moveedi_ONE.ToString() + fi.Name.ToString());

                    log += operatorCode + " | " + DateTime.Now.ToString() + " --------------------- | Check file : " + fileName + " " + result;

                }
            }
            catch (Exception ex)
            {
                log += operatorCode + " | " + DateTime.Now.ToString() + " --------------------- | Read file : " + fi.Name + " " + ex;
            }
        }
        savelog.toLogfile(log);
        resultPrint = resultPrint.Replace("][", ",");
        return resultPrint;
    }

    public static string ToDB(String data, string refname)
    {
        string status = "";

        string CMDT = "";
        string VGM = "";
        string VGM_UOM = "";
        string SHIPPER_NAME = "";
        string CONSIGNEE_NAME = "";
        string SEAL = "";
        string STOW_CODE = "";
        string BLOCK_STOWAGE = "";
        string VENTILATION = "";
        string TMP = "";
        string TMPUOM = "";
        string DG_CLASS = "";
        string OOG_OH = "";
        string OOG_OF = "";
        string OOG_OA = "";
        string OOG_OL = "";
        string OOG_OR = "";
        string PAYMENT = "";
        string ReeferType = "";

        DataTable dt = new DataTable();
        DataTable EDI_Table = new DataTable();
        try
        {
            dt = (DataTable)convertFormat.JsonStringToDataTable(data);
            if (dt.Rows.Count > 0)
            {
                con.ConnectionString = sqlsrvconnect.connectionLCIT_EDI_COPARN;
                if (dt.Rows[0]["Message Status"].ToString() == "Original")
                {
                    for (int record = 0; record < dt.Rows.Count; record++)
                    {

                        DataColumnCollection columns = dt.Columns;

                        if (columns.Contains("Commodity Description"))
                        {
                            CMDT = dt.Rows[0]["Commodity Description"].ToString();
                        }
                        if (columns.Contains("VGM"))
                        {
                            VGM = dt.Rows[record]["VGM"].ToString();
                        }
                        if (columns.Contains("VGMUOM"))
                        {
                            VGM_UOM = dt.Rows[record]["VGMUOM"].ToString();
                        }
                        if (columns.Contains("Shipper Name"))
                        {
                            SHIPPER_NAME = dt.Rows[record]["Shipper Name"].ToString();
                        }
                        if (columns.Contains("Consignee Name"))
                        {
                            CONSIGNEE_NAME = dt.Rows[record]["Consignee Name"].ToString();
                        }
                        if (columns.Contains("Seal"))
                        {
                            SEAL = dt.Rows[record]["Seal"].ToString();
                        }
                        if (columns.Contains("Stowage"))
                        {
                            STOW_CODE = dt.Rows[record]["Stowage"].ToString();
                        }
                        if (columns.Contains("Block Stowage"))
                        {
                            BLOCK_STOWAGE = dt.Rows[record]["Block Stowage"].ToString();
                        }
                        if (columns.Contains("Ventilation"))
                        {
                            VENTILATION = dt.Rows[record]["Ventilation"].ToString();
                        }
                        if (columns.Contains("Temp"))
                        {
                            TMP = dt.Rows[record]["Temp"].ToString();
                        }
                        if (columns.Contains("TempUOM"))
                        {
                            TMPUOM = dt.Rows[record]["TempUOM"].ToString();
                        }
                        if (columns.Contains("DG"))
                        {
                            DG_CLASS = dt.Rows[record]["DG"].ToString();
                        }
                        if (columns.Contains("OOG HIGHT"))
                        {
                            OOG_OH = dt.Rows[record]["OOG HIGHT"].ToString();
                        }
                        if (columns.Contains("OOG FRONT"))
                        {
                            OOG_OF = dt.Rows[record]["OOG FRONT"].ToString();
                        }
                        if (columns.Contains("OOG REAR"))
                        {
                            OOG_OA = dt.Rows[record]["OOG REAR"].ToString();
                        }
                        if (columns.Contains("OOG LEFT"))
                        {
                            OOG_OL = dt.Rows[record]["OOG LEFT"].ToString();
                        }
                        if (columns.Contains("OOG RIGHT"))
                        {
                            OOG_OR = dt.Rows[record]["OOG RIGHT"].ToString();
                        }
                        if (columns.Contains("PaymentType"))
                        {
                            PAYMENT = dt.Rows[record]["PaymentType"].ToString();
                        }
                        if(columns.Contains("ReeferAsDry"))
                        {
                            ReeferType = dt.Rows[record]["ReeferAsDry"].ToString();
                        }


                        // status += "INSERT INTO LCIT_EDI_COPARN.dbo.COPARN_EDIDATA (Booking_No, VESSEL_NAME, VOYAGE, OPERATOR_CODE, POL, POD, DST, CMDT, CNTR_NUM, [SIZE], [TYPE], HEIGHT, SS, LADENT_SATAUS, VGM, VGM_UOM, GROSSWEIGHT, GROSSWEIGHT_UOM, SHIPPER_NAME, CONSIGNEE_NAME, SEAL, STOW_CODE, BLOCK_STOWAGE, VENTILATION, TMP, TMPUOM, DG_CLASS, OOG_OH, OOG_OF, OOG_OA, OOG_OL, OOG_OR, GATE_ACTIVITY_CODE, PAYMENT, MODIFY_DATE, RECORD_STATUS) VALUES('" + dt.Rows[0]["Booking Reference"].ToString() + "', '" + dt.Rows[0]["Vessel Name"].ToString() + "', '" + dt.Rows[0]["Voyage"].ToString() + "', '" + dt.Rows[0]["Operator Code"].ToString() + "', '" + dt.Rows[record]["POL"].ToString() + "', '" + dt.Rows[record]["POD"].ToString() + "', '" + dt.Rows[record]["DEL"].ToString() + "', '" + CMDT + "', '" + dt.Rows[record]["Container No"].ToString() + "', '" + dt.Rows[record]["ISO"].ToString().Substring(0, 2) + "', '" + dt.Rows[record]["ISO"].ToString().Substring(0, 2) + "', '" + dt.Rows[record]["ISO"].ToString().Substring(0, 2) + "', '" + dt.Rows[record]["Status"].ToString() + "', '" + dt.Rows[record]["Laden"].ToString() + "', '" + VGM + "', '" + VGM_UOM + "', '" + dt.Rows[record]["GROSSWEIGHT"].ToString() + "', '" + dt.Rows[record]["GROSSWEIGHTUOM"].ToString() + "', '" + SHIPPER_NAME + "', '" + CONSIGNEE_NAME + "', '" + SEAL + "', '" + STOW_CODE + "', '" + BLOCK_STOWAGE + "', '" + VENTILATION + "', '" + TMP + "', '" + TMPUOM + "', '" + DG_CLASS + "', '" + OOG_OH + "', '" + OOG_OF + "', '" + OOG_OA + "','" + OOG_OL + "', '" + OOG_OR + "', 'PRE-ADVISE-" + refname.ToString() + "', '" + PAYMENT + "','" + DateTime.Now + "', 'ACTIVE')";

                        con.Open();

                        try
                        {
                            SqlDataAdapter sda = new SqlDataAdapter("INSERT INTO LCIT_EDI_COPARN.dbo.COPARN_EDIDATA (Booking_No, VESSEL_NAME, VOYAGE, OPERATOR_CODE, POL, POD, DST, CMDT, CNTR_NUM, [SIZE], [TYPE], HEIGHT, SS, LADENT_SATAUS, VGM, VGM_UOM, GROSSWEIGHT, GROSSWEIGHT_UOM, SHIPPER_NAME, CONSIGNEE_NAME, SEAL, STOW_CODE, BLOCK_STOWAGE, VENTILATION, TMP, TMPUOM, DG_CLASS, OOG_OH, OOG_OF, OOG_OA, OOG_OL, OOG_OR, GATE_ACTIVITY_CODE, PAYMENT, MODIFY_DATE, RECORD_STATUS,ISASDRY) VALUES('" + dt.Rows[0]["Booking Reference"].ToString() + "', '" + dt.Rows[0]["Vessel Name"].ToString() + "', '" + dt.Rows[0]["Voyage"].ToString() + "', '" + dt.Rows[0]["Operator Code"].ToString() + "', '" + dt.Rows[record]["POL"].ToString() + "', '" + dt.Rows[record]["POD"].ToString() + "', '" + dt.Rows[record]["DEL"].ToString() + "', '" + CMDT + "', '" + dt.Rows[record]["Container No"].ToString() + "', '" + dt.Rows[record]["ISO"].ToString().Substring(0, 2) + "', '" + dt.Rows[record]["ISO"].ToString().Substring(2, 2) + "', '" + dt.Rows[record]["ISO"].ToString().Substring(4, 2) + "', '" + dt.Rows[record]["Status"].ToString() + "', '" + dt.Rows[record]["Laden"].ToString() + "', '" + VGM + "', '" + VGM_UOM + "', '" + dt.Rows[record]["GROSSWEIGHT"].ToString() + "', '" + dt.Rows[record]["GROSSWEIGHTUOM"].ToString() + "', '" + SHIPPER_NAME + "', '" + CONSIGNEE_NAME + "', '" + SEAL + "', '" + STOW_CODE + "', '" + BLOCK_STOWAGE + "', '" + VENTILATION + "', '" + TMP + "', '" + TMPUOM + "', '" + DG_CLASS + "', '" + OOG_OH + "', '" + OOG_OF + "', '" + OOG_OA + "','" + OOG_OL + "', '" + OOG_OR + "', 'PRE-ADVISE-" + refname.ToString() + "', '" + PAYMENT + "','" + DateTime.Now + "', '" + dt.Rows[0]["Message Status"].ToString() + "','"+ReeferType+"')", con);
                            sda.Fill(EDI_Table);
                        }
                        catch (Exception ex)
                        {
                            status += "INSERT INTO LCIT_EDI_COPARN.dbo.COPARN_EDIDATA (Booking_No, VESSEL_NAME, VOYAGE, OPERATOR_CODE, POL, POD, DST, CMDT, CNTR_NUM, [SIZE], [TYPE], HEIGHT, SS, LADENT_SATAUS, VGM, VGM_UOM, GROSSWEIGHT, GROSSWEIGHT_UOM, SHIPPER_NAME, CONSIGNEE_NAME, SEAL, STOW_CODE, BLOCK_STOWAGE, VENTILATION, TMP, TMPUOM, DG_CLASS, OOG_OH, OOG_OF, OOG_OA, OOG_OL, OOG_OR, GATE_ACTIVITY_CODE, PAYMENT, MODIFY_DATE, RECORD_STATUS,ISASDRY) VALUES('" + dt.Rows[0]["Booking Reference"].ToString() + "', '" + dt.Rows[0]["Vessel Name"].ToString() + "', '" + dt.Rows[0]["Voyage"].ToString() + "', '" + dt.Rows[0]["Operator Code"].ToString() + "', '" + dt.Rows[record]["POL"].ToString() + "', '" + dt.Rows[record]["POD"].ToString() + "', '" + dt.Rows[record]["DEL"].ToString() + "', '" + CMDT + "', '" + dt.Rows[record]["Container No"].ToString() + "', '" + dt.Rows[record]["ISO"].ToString().Substring(0, 2) + "', '" + dt.Rows[record]["ISO"].ToString().Substring(2, 2) + "', '" + dt.Rows[record]["ISO"].ToString().Substring(4, 2) + "', '" + dt.Rows[record]["Status"].ToString() + "', '" + dt.Rows[record]["Laden"].ToString() + "', '" + VGM + "', '" + VGM_UOM + "', '" + dt.Rows[record]["GROSSWEIGHT"].ToString() + "', '" + dt.Rows[record]["GROSSWEIGHTUOM"].ToString() + "', '" + SHIPPER_NAME + "', '" + CONSIGNEE_NAME + "', '" + SEAL + "', '" + STOW_CODE + "', '" + BLOCK_STOWAGE + "', '" + VENTILATION + "', '" + TMP + "', '" + TMPUOM + "', '" + DG_CLASS + "', '" + OOG_OH + "', '" + OOG_OF + "', '" + OOG_OA + "','" + OOG_OL + "', '" + OOG_OR + "', 'PRE-ADVISE-" + refname.ToString() + "', '" + PAYMENT + "','" + DateTime.Now + "', '" + dt.Rows[0]["Message Status"].ToString() + "','"+ReeferType+"')";

                            continue;




                        }


                        con.Close();
                    }
                }

                if (dt.Rows[0]["Message Status"].ToString() == "Replace")
                {

                    for (int record = 0; record < dt.Rows.Count; record++)
                    {

                        DataColumnCollection columns = dt.Columns;

                        if (columns.Contains("Commodity Description"))
                        {
                            CMDT = dt.Rows[0]["Commodity Description"].ToString();
                        }
                        if (columns.Contains("VGM"))
                        {
                            VGM = dt.Rows[record]["VGM"].ToString();
                        }
                        if (columns.Contains("VGMUOM"))
                        {
                            VGM_UOM = dt.Rows[record]["VGMUOM"].ToString();
                        }
                        if (columns.Contains("Shipper Name"))
                        {
                            SHIPPER_NAME = dt.Rows[record]["Shipper Name"].ToString();
                        }
                        if (columns.Contains("Consignee Name"))
                        {
                            CONSIGNEE_NAME = dt.Rows[record]["Consignee Name"].ToString();
                        }
                        if (columns.Contains("Seal"))
                        {
                            SEAL = dt.Rows[record]["Seal"].ToString();
                        }
                        if (columns.Contains("Stowage"))
                        {
                            STOW_CODE = dt.Rows[record]["Stowage"].ToString();
                        }
                        if (columns.Contains("Block Stowage"))
                        {
                            BLOCK_STOWAGE = dt.Rows[record]["Block Stowage"].ToString();
                        }
                        if (columns.Contains("Ventilation"))
                        {
                            VENTILATION = dt.Rows[record]["Ventilation"].ToString();
                        }
                        if (columns.Contains("Temp"))
                        {
                            TMP = dt.Rows[record]["Temp"].ToString();
                        }
                        if (columns.Contains("TempUOM"))
                        {
                            TMPUOM = dt.Rows[record]["TempUOM"].ToString();
                        }
                        if (columns.Contains("DG"))
                        {
                            DG_CLASS = dt.Rows[record]["DG"].ToString();
                        }
                        if (columns.Contains("OOG HIGHT"))
                        {
                            OOG_OH = dt.Rows[record]["OOG HIGHT"].ToString();
                        }
                        if (columns.Contains("OOG FRONT"))
                        {
                            OOG_OF = dt.Rows[record]["OOG FRONT"].ToString();
                        }
                        if (columns.Contains("OOG REAR"))
                        {
                            OOG_OA = dt.Rows[record]["OOG REAR"].ToString();
                        }
                        if (columns.Contains("OOG LEFT"))
                        {
                            OOG_OL = dt.Rows[record]["OOG LEFT"].ToString();
                        }
                        if (columns.Contains("OOG RIGHT"))
                        {
                            OOG_OR = dt.Rows[record]["OOG RIGHT"].ToString();
                        }
                        if (columns.Contains("PaymentType"))
                        {
                            PAYMENT = dt.Rows[record]["PaymentType"].ToString();
                        }
                         if(columns.Contains("ReeferAsDry"))
                        {
                            ReeferType = dt.Rows[record]["ReeferAsDry"].ToString();
                        }


                        // status += "INSERT INTO LCIT_EDI_COPARN.dbo.COPARN_EDIDATA (Booking_No, VESSEL_NAME, VOYAGE, OPERATOR_CODE, POL, POD, DST, CMDT, CNTR_NUM, [SIZE], [TYPE], HEIGHT, SS, LADENT_SATAUS, VGM, VGM_UOM, GROSSWEIGHT, GROSSWEIGHT_UOM, SHIPPER_NAME, CONSIGNEE_NAME, SEAL, STOW_CODE, BLOCK_STOWAGE, VENTILATION, TMP, TMPUOM, DG_CLASS, OOG_OH, OOG_OF, OOG_OA, OOG_OL, OOG_OR, GATE_ACTIVITY_CODE, PAYMENT, MODIFY_DATE, RECORD_STATUS) VALUES('" + dt.Rows[0]["Booking Reference"].ToString() + "', '" + dt.Rows[0]["Vessel Name"].ToString() + "', '" + dt.Rows[0]["Voyage"].ToString() + "', '" + dt.Rows[0]["Operator Code"].ToString() + "', '" + dt.Rows[record]["POL"].ToString() + "', '" + dt.Rows[record]["POD"].ToString() + "', '" + dt.Rows[record]["DEL"].ToString() + "', '" + CMDT + "', '" + dt.Rows[record]["Container No"].ToString() + "', '" + dt.Rows[record]["ISO"].ToString().Substring(0, 2) + "', '" + dt.Rows[record]["ISO"].ToString().Substring(0, 2) + "', '" + dt.Rows[record]["ISO"].ToString().Substring(0, 2) + "', '" + dt.Rows[record]["Status"].ToString() + "', '" + dt.Rows[record]["Laden"].ToString() + "', '" + VGM + "', '" + VGM_UOM + "', '" + dt.Rows[record]["GROSSWEIGHT"].ToString() + "', '" + dt.Rows[record]["GROSSWEIGHTUOM"].ToString() + "', '" + SHIPPER_NAME + "', '" + CONSIGNEE_NAME + "', '" + SEAL + "', '" + STOW_CODE + "', '" + BLOCK_STOWAGE + "', '" + VENTILATION + "', '" + TMP + "', '" + TMPUOM + "', '" + DG_CLASS + "', '" + OOG_OH + "', '" + OOG_OF + "', '" + OOG_OA + "','" + OOG_OL + "', '" + OOG_OR + "', 'PRE-ADVISE-" + refname.ToString() + "', '" + PAYMENT + "','" + DateTime.Now + "', 'ACTIVE')";

                        con.Open();



                        SqlDataAdapter sdaCheckExists = new SqlDataAdapter("SELECT  COUNT(*) FROM LCIT_EDI_COPARN.dbo.COPARN_EDIDATA WHERE Booking_No LIKE '" + dt.Rows[record]["Booking Reference"].ToString() + "' AND CNTR_NUM LIKE '" + dt.Rows[record]["Container No"].ToString() + "'", con);
                        DataTable dtChekc = new DataTable();
                        sdaCheckExists.Fill(dtChekc);
                        con.Close();

                        if (int.Parse(dtChekc.Rows[0][0].ToString()) > 0)
                        {
                            con.Open();

                            SqlDataAdapter SDAupdate = new SqlDataAdapter(" UPDATE LCIT_EDI_COPARN.dbo.COPARN_EDIDATA SET VESSEL_NAME = '" + dt.Rows[0]["Vessel Name"].ToString() + "', VOYAGE = '" + dt.Rows[0]["Voyage"].ToString() + "', OPERATOR_CODE = '" + dt.Rows[0]["Operator Code"].ToString() + "', POL = '" + dt.Rows[record]["POL"].ToString() + "', POD = '" + dt.Rows[record]["POD"].ToString() + "', DST = '" + dt.Rows[record]["DEL"].ToString() + "', CMDT = '" + CMDT + "', CNTR_NUM = '" + dt.Rows[record]["Container No"].ToString() + "', [SIZE] = '" + dt.Rows[record]["ISO"].ToString().Substring(0, 2) + "', [TYPE] = '" + dt.Rows[record]["ISO"].ToString().Substring(2, 2) + "', HEIGHT = '" + dt.Rows[record]["ISO"].ToString().Substring(4, 2) + "', SS = '" + dt.Rows[record]["Status"].ToString() + "', LADENT_SATAUS = '" + dt.Rows[record]["Laden"].ToString() + "', VGM = '" + VGM + "', VGM_UOM = '" + VGM_UOM + "', GROSSWEIGHT = '" + dt.Rows[record]["GROSSWEIGHT"].ToString() + "', GROSSWEIGHT_UOM = '" + dt.Rows[record]["GROSSWEIGHTUOM"].ToString() + "', SHIPPER_NAME = '" + SHIPPER_NAME + "', CONSIGNEE_NAME = '" + CONSIGNEE_NAME + "', SEAL = '" + SEAL + "', STOW_CODE = '" + STOW_CODE + "', BLOCK_STOWAGE = '" + BLOCK_STOWAGE + "', VENTILATION = '" + VENTILATION + "', TMP = '" + TMP + "', TMPUOM = '" + TMPUOM + "', DG_CLASS = '" + DG_CLASS + "', OOG_OH = '" + OOG_OH + "', OOG_OF = '" + OOG_OF + "', OOG_OA = '" + OOG_OA + "', OOG_OL = '" + OOG_OL + "', OOG_OR = '" + OOG_OR + "', GATE_ACTIVITY_CODE = 'PRE-ADVISE-" + refname.ToString() + "', PAYMENT = '" + PAYMENT + "', MODIFY_DATE = '" + DateTime.Now + "', RECORD_STATUS = '" + dt.Rows[0]["Message Status"].ToString() + "', ISASDRY ='"+ReeferType+"' WHERE Booking_No LIKE '" + dt.Rows[0]["Booking Reference"].ToString() + "'AND CNTR_NUM LIKE '" + dt.Rows[record]["Container No"].ToString() + "'", con);
                            DataTable dtupdate = new DataTable();
                            SDAupdate.Fill(dtupdate);

                            status += "UPDATE LCIT_EDI_COPARN.dbo.COPARN_EDIDATA SET VESSEL_NAME = '" + dt.Rows[0]["Vessel Name"].ToString() + "', VOYAGE = '" + dt.Rows[0]["Voyage"].ToString() + "', OPERATOR_CODE = '" + dt.Rows[0]["Operator Code"].ToString() + "', POL = '" + dt.Rows[record]["POL"].ToString() + "', POD = '" + dt.Rows[record]["POD"].ToString() + "', DST = '" + dt.Rows[record]["DEL"].ToString() + "', CMDT = '" + CMDT + "', CNTR_NUM = '" + dt.Rows[record]["Container No"].ToString() + "', [SIZE] = '" + dt.Rows[record]["ISO"].ToString().Substring(0, 2) + "', [TYPE] = '" + dt.Rows[record]["ISO"].ToString().Substring(2, 2) + "', HEIGHT = '" + dt.Rows[record]["ISO"].ToString().Substring(4, 2) + "', SS = '" + dt.Rows[record]["Status"].ToString() + "', LADENT_SATAUS = '" + dt.Rows[record]["Laden"].ToString() + "', VGM = '" + VGM + "', VGM_UOM = '" + VGM_UOM + "', GROSSWEIGHT = '" + dt.Rows[record]["GROSSWEIGHT"].ToString() + "', GROSSWEIGHT_UOM = '" + dt.Rows[record]["GROSSWEIGHTUOM"].ToString() + "', SHIPPER_NAME = '" + SHIPPER_NAME + "', CONSIGNEE_NAME = '" + CONSIGNEE_NAME + "', SEAL = '" + SEAL + "', STOW_CODE = '" + STOW_CODE + "', BLOCK_STOWAGE = '" + BLOCK_STOWAGE + "', VENTILATION = '" + VENTILATION + "', TMP = '" + TMP + "', TMPUOM = '" + TMPUOM + "', DG_CLASS = '" + DG_CLASS + "', OOG_OH = '" + OOG_OH + "', OOG_OF = '" + OOG_OF + "', OOG_OA = '" + OOG_OA + "', OOG_OL = '" + OOG_OL + "', OOG_OR = '" + OOG_OR + "', GATE_ACTIVITY_CODE = 'PRE-ADVISE-" + refname.ToString() + "', PAYMENT = '" + PAYMENT + "', MODIFY_DATE = '" + DateTime.Now + "', RECORD_STATUS = '" + dt.Rows[0]["Message Status"].ToString() + "', ISASDRY ='"+ReeferType+"' WHERE Booking_No LIKE '" + dt.Rows[0]["Booking Reference"].ToString() + "'AND CNTR_NUM LIKE '" + dt.Rows[record]["Container No"].ToString() + "'";




                            con.Close();

                        }
                        else
                        {

                            con.Open();

                            try
                            {
                                SqlDataAdapter sda = new SqlDataAdapter(" INSERT INTO LCIT_EDI_COPARN.dbo.COPARN_EDIDATA (Booking_No, VESSEL_NAME, VOYAGE, OPERATOR_CODE, POL, POD, DST, CMDT, CNTR_NUM, [SIZE], [TYPE], HEIGHT, SS, LADENT_SATAUS, VGM, VGM_UOM, GROSSWEIGHT, GROSSWEIGHT_UOM, SHIPPER_NAME, CONSIGNEE_NAME, SEAL, STOW_CODE, BLOCK_STOWAGE, VENTILATION, TMP, TMPUOM, DG_CLASS, OOG_OH, OOG_OF, OOG_OA, OOG_OL, OOG_OR, GATE_ACTIVITY_CODE, PAYMENT, MODIFY_DATE, RECORD_STATUS) VALUES('" + dt.Rows[0]["Booking Reference"].ToString() + "', '" + dt.Rows[0]["Vessel Name"].ToString() + "', '" + dt.Rows[0]["Voyage"].ToString() + "', '" + dt.Rows[0]["Operator Code"].ToString() + "', '" + dt.Rows[record]["POL"].ToString() + "', '" + dt.Rows[record]["POD"].ToString() + "', '" + dt.Rows[record]["DEL"].ToString() + "', '" + CMDT + "', '" + dt.Rows[record]["Container No"].ToString() + "', '" + dt.Rows[record]["ISO"].ToString().Substring(0, 2) + "', '" + dt.Rows[record]["ISO"].ToString().Substring(2, 2) + "', '" + dt.Rows[record]["ISO"].ToString().Substring(4, 2) + "', '" + dt.Rows[record]["Status"].ToString() + "', '" + dt.Rows[record]["Laden"].ToString() + "', '" + VGM + "', '" + VGM_UOM + "', '" + dt.Rows[record]["GROSSWEIGHT"].ToString() + "', '" + dt.Rows[record]["GROSSWEIGHTUOM"].ToString() + "', '" + SHIPPER_NAME + "', '" + CONSIGNEE_NAME + "', '" + SEAL + "', '" + STOW_CODE + "', '" + BLOCK_STOWAGE + "', '" + VENTILATION + "', '" + TMP + "', '" + TMPUOM + "', '" + DG_CLASS + "', '" + OOG_OH + "', '" + OOG_OF + "', '" + OOG_OA + "', '" + OOG_OL + "', '" + OOG_OR + "', 'PRE-ADVISE-" + refname.ToString() + "', '" + PAYMENT + "', '" + DateTime.Now + "', '" + dt.Rows[0]["Message Status"].ToString() + "')", con);
                                DataTable dtWithoutReplace = new DataTable();
                                sda.Fill(dtWithoutReplace);

                            }
                            catch (Exception ex)
                            {
                                status += "INSERT INTO LCIT_EDI_COPARN.dbo.COPARN_EDIDATA (Booking_No, VESSEL_NAME, VOYAGE, OPERATOR_CODE, POL, POD, DST, CMDT, CNTR_NUM, [SIZE], [TYPE], HEIGHT, SS, LADENT_SATAUS, VGM, VGM_UOM, GROSSWEIGHT, GROSSWEIGHT_UOM, SHIPPER_NAME, CONSIGNEE_NAME, SEAL, STOW_CODE, BLOCK_STOWAGE, VENTILATION, TMP, TMPUOM, DG_CLASS, OOG_OH, OOG_OF, OOG_OA, OOG_OL, OOG_OR, GATE_ACTIVITY_CODE, PAYMENT, MODIFY_DATE, RECORD_STATUS) VALUES('" + dt.Rows[0]["Booking Reference"].ToString() + "', '" + dt.Rows[0]["Vessel Name"].ToString() + "', '" + dt.Rows[0]["Voyage"].ToString() + "', '" + dt.Rows[0]["Operator Code"].ToString() + "', '" + dt.Rows[record]["POL"].ToString() + "', '" + dt.Rows[record]["POD"].ToString() + "', '" + dt.Rows[record]["DEL"].ToString() + "', '" + CMDT + "', '" + dt.Rows[record]["Container No"].ToString() + "', '" + dt.Rows[record]["ISO"].ToString().Substring(0, 2) + "', '" + dt.Rows[record]["ISO"].ToString().Substring(0, 2) + "', '" + dt.Rows[record]["ISO"].ToString().Substring(0, 2) + "', '" + dt.Rows[record]["Status"].ToString() + "', '" + dt.Rows[record]["Laden"].ToString() + "', '" + VGM + "', '" + VGM_UOM + "', '" + dt.Rows[record]["GROSSWEIGHT"].ToString() + "', '" + dt.Rows[record]["GROSSWEIGHTUOM"].ToString() + "', '" + SHIPPER_NAME + "', '" + CONSIGNEE_NAME + "', '" + SEAL + "', '" + STOW_CODE + "', '" + BLOCK_STOWAGE + "', '" + VENTILATION + "', '" + TMP + "', '" + TMPUOM + "', '" + DG_CLASS + "', '" + OOG_OH + "', '" + OOG_OF + "', '" + OOG_OA + "','" + OOG_OL + "', '" + OOG_OR + "', 'PRE-ADVISE-" + refname.ToString() + "', '" + PAYMENT + "','" + DateTime.Now + "', '" + dt.Rows[0]["Message Status"].ToString() + "')";

                                continue;


                            }


                            con.Close();

                        }




                    }




                }

                if (dt.Rows[0]["Message Status"].ToString() == "Cancellation")
                {
                    for (int record = 0; record < dt.Rows.Count; record++)
                    {

                        DataColumnCollection columns = dt.Columns;

                        if (columns.Contains("Commodity Description"))
                        {
                            CMDT = dt.Rows[0]["Commodity Description"].ToString();
                        }
                        if (columns.Contains("VGM"))
                        {
                            VGM = dt.Rows[record]["VGM"].ToString();
                        }
                        if (columns.Contains("VGMUOM"))
                        {
                            VGM_UOM = dt.Rows[record]["VGMUOM"].ToString();
                        }
                        if (columns.Contains("Shipper Name"))
                        {
                            SHIPPER_NAME = dt.Rows[record]["Shipper Name"].ToString();
                        }
                        if (columns.Contains("Consignee Name"))
                        {
                            CONSIGNEE_NAME = dt.Rows[record]["Consignee Name"].ToString();
                        }
                        if (columns.Contains("Seal"))
                        {
                            SEAL = dt.Rows[record]["Seal"].ToString();
                        }
                        if (columns.Contains("Stowage"))
                        {
                            STOW_CODE = dt.Rows[record]["Stowage"].ToString();
                        }
                        if (columns.Contains("Block Stowage"))
                        {
                            BLOCK_STOWAGE = dt.Rows[record]["Block Stowage"].ToString();
                        }
                        if (columns.Contains("Ventilation"))
                        {
                            VENTILATION = dt.Rows[record]["Ventilation"].ToString();
                        }
                        if (columns.Contains("Temp"))
                        {
                            TMP = dt.Rows[record]["Temp"].ToString();
                        }
                        if (columns.Contains("TempUOM"))
                        {
                            TMPUOM = dt.Rows[record]["TempUOM"].ToString();
                        }
                        if (columns.Contains("DG"))
                        {
                            DG_CLASS = dt.Rows[record]["DG"].ToString();
                        }
                        if (columns.Contains("OOG HIGHT"))
                        {
                            OOG_OH = dt.Rows[record]["OOG HIGHT"].ToString();
                        }
                        if (columns.Contains("OOG FRONT"))
                        {
                            OOG_OF = dt.Rows[record]["OOG FRONT"].ToString();
                        }
                        if (columns.Contains("OOG REAR"))
                        {
                            OOG_OA = dt.Rows[record]["OOG REAR"].ToString();
                        }
                        if (columns.Contains("OOG LEFT"))
                        {
                            OOG_OL = dt.Rows[record]["OOG LEFT"].ToString();
                        }
                        if (columns.Contains("OOG RIGHT"))
                        {
                            OOG_OR = dt.Rows[record]["OOG RIGHT"].ToString();
                        }
                        if (columns.Contains("PaymentType"))
                        {
                            PAYMENT = dt.Rows[record]["PaymentType"].ToString();
                        }
                         if(columns.Contains("ReeferAsDry"))
                        {
                            ReeferType = dt.Rows[record]["ReeferAsDry"].ToString();
                        }


                        // status += "INSERT INTO LCIT_EDI_COPARN.dbo.COPARN_EDIDATA (Booking_No, VESSEL_NAME, VOYAGE, OPERATOR_CODE, POL, POD, DST, CMDT, CNTR_NUM, [SIZE], [TYPE], HEIGHT, SS, LADENT_SATAUS, VGM, VGM_UOM, GROSSWEIGHT, GROSSWEIGHT_UOM, SHIPPER_NAME, CONSIGNEE_NAME, SEAL, STOW_CODE, BLOCK_STOWAGE, VENTILATION, TMP, TMPUOM, DG_CLASS, OOG_OH, OOG_OF, OOG_OA, OOG_OL, OOG_OR, GATE_ACTIVITY_CODE, PAYMENT, MODIFY_DATE, RECORD_STATUS) VALUES('" + dt.Rows[0]["Booking Reference"].ToString() + "', '" + dt.Rows[0]["Vessel Name"].ToString() + "', '" + dt.Rows[0]["Voyage"].ToString() + "', '" + dt.Rows[0]["Operator Code"].ToString() + "', '" + dt.Rows[record]["POL"].ToString() + "', '" + dt.Rows[record]["POD"].ToString() + "', '" + dt.Rows[record]["DEL"].ToString() + "', '" + CMDT + "', '" + dt.Rows[record]["Container No"].ToString() + "', '" + dt.Rows[record]["ISO"].ToString().Substring(0, 2) + "', '" + dt.Rows[record]["ISO"].ToString().Substring(0, 2) + "', '" + dt.Rows[record]["ISO"].ToString().Substring(0, 2) + "', '" + dt.Rows[record]["Status"].ToString() + "', '" + dt.Rows[record]["Laden"].ToString() + "', '" + VGM + "', '" + VGM_UOM + "', '" + dt.Rows[record]["GROSSWEIGHT"].ToString() + "', '" + dt.Rows[record]["GROSSWEIGHTUOM"].ToString() + "', '" + SHIPPER_NAME + "', '" + CONSIGNEE_NAME + "', '" + SEAL + "', '" + STOW_CODE + "', '" + BLOCK_STOWAGE + "', '" + VENTILATION + "', '" + TMP + "', '" + TMPUOM + "', '" + DG_CLASS + "', '" + OOG_OH + "', '" + OOG_OF + "', '" + OOG_OA + "','" + OOG_OL + "', '" + OOG_OR + "', 'PRE-ADVISE-" + refname.ToString() + "', '" + PAYMENT + "','" + DateTime.Now + "', 'ACTIVE')";

                        con.Open();

                        try
                        {
                            SqlDataAdapter sda = new SqlDataAdapter("INSERT INTO LCIT_EDI_COPARN.dbo.COPARN_EDIDATA (Booking_No, VESSEL_NAME, VOYAGE, OPERATOR_CODE, POL, POD, DST, CMDT, CNTR_NUM, [SIZE], [TYPE], HEIGHT, SS, LADENT_SATAUS, VGM, VGM_UOM, GROSSWEIGHT, GROSSWEIGHT_UOM, SHIPPER_NAME, CONSIGNEE_NAME, SEAL, STOW_CODE, BLOCK_STOWAGE, VENTILATION, TMP, TMPUOM, DG_CLASS, OOG_OH, OOG_OF, OOG_OA, OOG_OL, OOG_OR, GATE_ACTIVITY_CODE, PAYMENT, MODIFY_DATE, RECORD_STATUS,ISASDRY) VALUES('" + dt.Rows[0]["Booking Reference"].ToString() + "', '" + dt.Rows[0]["Vessel Name"].ToString() + "', '" + dt.Rows[0]["Voyage"].ToString() + "', '" + dt.Rows[0]["Operator Code"].ToString() + "', '" + dt.Rows[record]["POL"].ToString() + "', '" + dt.Rows[record]["POD"].ToString() + "', '" + dt.Rows[record]["DEL"].ToString() + "', '" + CMDT + "', '" + dt.Rows[record]["Container No"].ToString() + "', '" + dt.Rows[record]["ISO"].ToString().Substring(0, 2) + "', '" + dt.Rows[record]["ISO"].ToString().Substring(2, 2) + "', '" + dt.Rows[record]["ISO"].ToString().Substring(4, 2) + "', '" + dt.Rows[record]["Status"].ToString() + "', '" + dt.Rows[record]["Laden"].ToString() + "', '" + VGM + "', '" + VGM_UOM + "', '" + dt.Rows[record]["GROSSWEIGHT"].ToString() + "', '" + dt.Rows[record]["GROSSWEIGHTUOM"].ToString() + "', '" + SHIPPER_NAME + "', '" + CONSIGNEE_NAME + "', '" + SEAL + "', '" + STOW_CODE + "', '" + BLOCK_STOWAGE + "', '" + VENTILATION + "', '" + TMP + "', '" + TMPUOM + "', '" + DG_CLASS + "', '" + OOG_OH + "', '" + OOG_OF + "', '" + OOG_OA + "','" + OOG_OL + "', '" + OOG_OR + "', 'PRE-ADVISE-" + refname.ToString() + "', '" + PAYMENT + "','" + DateTime.Now + "', '" + dt.Rows[0]["Message Status"].ToString() + "','"+ReeferType+"')", con);
                            sda.Fill(EDI_Table);
                        }
                        catch (Exception ex)
                        {
                            status += "INSERT INTO LCIT_EDI_COPARN.dbo.COPARN_EDIDATA (Booking_No, VESSEL_NAME, VOYAGE, OPERATOR_CODE, POL, POD, DST, CMDT, CNTR_NUM, [SIZE], [TYPE], HEIGHT, SS, LADENT_SATAUS, VGM, VGM_UOM, GROSSWEIGHT, GROSSWEIGHT_UOM, SHIPPER_NAME, CONSIGNEE_NAME, SEAL, STOW_CODE, BLOCK_STOWAGE, VENTILATION, TMP, TMPUOM, DG_CLASS, OOG_OH, OOG_OF, OOG_OA, OOG_OL, OOG_OR, GATE_ACTIVITY_CODE, PAYMENT, MODIFY_DATE, RECORD_STATUS,ISASDRY) VALUES('" + dt.Rows[0]["Booking Reference"].ToString() + "', '" + dt.Rows[0]["Vessel Name"].ToString() + "', '" + dt.Rows[0]["Voyage"].ToString() + "', '" + dt.Rows[0]["Operator Code"].ToString() + "', '" + dt.Rows[record]["POL"].ToString() + "', '" + dt.Rows[record]["POD"].ToString() + "', '" + dt.Rows[record]["DEL"].ToString() + "', '" + CMDT + "', '" + dt.Rows[record]["Container No"].ToString() + "', '" + dt.Rows[record]["ISO"].ToString().Substring(0, 2) + "', '" + dt.Rows[record]["ISO"].ToString().Substring(2, 2) + "', '" + dt.Rows[record]["ISO"].ToString().Substring(4, 2) + "', '" + dt.Rows[record]["Status"].ToString() + "', '" + dt.Rows[record]["Laden"].ToString() + "', '" + VGM + "', '" + VGM_UOM + "', '" + dt.Rows[record]["GROSSWEIGHT"].ToString() + "', '" + dt.Rows[record]["GROSSWEIGHTUOM"].ToString() + "', '" + SHIPPER_NAME + "', '" + CONSIGNEE_NAME + "', '" + SEAL + "', '" + STOW_CODE + "', '" + BLOCK_STOWAGE + "', '" + VENTILATION + "', '" + TMP + "', '" + TMPUOM + "', '" + DG_CLASS + "', '" + OOG_OH + "', '" + OOG_OF + "', '" + OOG_OA + "','" + OOG_OL + "', '" + OOG_OR + "', 'PRE-ADVISE-" + refname.ToString() + "', '" + PAYMENT + "','" + DateTime.Now + "', '" + dt.Rows[0]["Message Status"].ToString() + "','"+ReeferType+"')";

                            continue;
                        }
                        con.Close();
                    }
                }
                else
                {
                    status += dt.Rows[0]["Message Status"].ToString();
                }

            }
            else
            {
                status += "Not found edi Record";

            }
        }
        catch (Exception ex)
        {
            status = ex.Message;
        }
        return status.ToString();
    }
}



