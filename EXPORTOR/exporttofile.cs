using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Mail;
using ClosedXML.Excel;
using Excel = Microsoft.Office.Interop.Excel;
using Spire.Xls;
using System.Xml;
using System.Net.Http;
using RestSharp;

namespace EXPORTOR
{
    class exporttofile
    {

        public static SqlConnection cone = new SqlConnection("Data Source=lcitedisrv.lcit.com;Initial Catalog=SML_PREADVISE;User ID=sa;Password=p@ssw0rd");
        public static DataTable dt = new DataTable();
        public static DataTable dtEX = new DataTable();

        public static keepfile.flepath fp = new keepfile.flepath();

        public static writeToFile.writeLogFile tolog = new writeToFile.writeLogFile();

        public static Workbook book = new Workbook();

        public static string logfile = "";
        public static string finalFileName = "";

        static void Main(string[] args)
        {
            cone.Open();

            SqlDataAdapter sdaCheckProfile = new SqlDataAdapter("SELECT Line FROM LCIT_EDI_COPARN.dbo.COPARN_CUSPROFILE;", cone);
            DataTable dtCheckProfile = new DataTable();
            DataTable dtQuery = new DataTable();

            sdaCheckProfile.Fill(dtCheckProfile);

            cone.Close();

            if (dtCheckProfile.Rows.Count > 0)
            {
                for (int checkpg = 0; checkpg < dtCheckProfile.Rows.Count; checkpg++)
                {

                    Console.WriteLine(ReadEDIfile(dtCheckProfile.Rows[checkpg][0].ToString())); // From API 

                    cone.Open();
                    SqlDataAdapter sdaQuery = new SqlDataAdapter("SELECT VESSEL_NAME, VOYAGE, OPERATOR_CODE, RECORD_STATUS FROM LCIT_EDI_COPARN.dbo.COPARN_EDIDATA WHERE RECORD_STATUS NOT LIKE 'INACTIVE' AND OPERATOR_CODE LIKE '" + dtCheckProfile.Rows[checkpg][0].ToString() + "' AND VESSEL_NAME !='' AND VOYAGE !='' GROUP BY VESSEL_NAME, VOYAGE,OPERATOR_CODE,RECORD_STATUS", cone);
                    sdaQuery.Fill(dtQuery);

                    cone.Close();

                    if (dtQuery.Rows.Count > 0)
                    {

                        verifyData(dtQuery);

                    }



                }
            }

            Console.WriteLine(dtQuery.Rows.Count);

            Console.WriteLine("Please any key to continue ...");
            // Console.ReadKey(true);
        }

        public static void verifyData(DataTable getdt)
        {
            DataRow dtrow = null;
            for (int checkdt = 0; checkdt < getdt.Rows.Count; checkdt++)
            {
                setGridView();

                Console.WriteLine(checkdt.ToString() + " : SELECT [No], Booking_No, VESSEL_NAME, VOYAGE, OPERATOR_CODE, POL, POD, DST, CMDT, CNTR_NUM, [SIZE], [TYPE], HEIGHT, SS, LADENT_SATAUS, VGM, VGM_UOM, GROSSWEIGHT, GROSSWEIGHT_UOM, SHIPPER_NAME, CONSIGNEE_NAME, SEAL, STOW_CODE, BLOCK_STOWAGE, VENTILATION, TMP, TMPUOM, DG_CLASS, OOG_OH, OOG_OF, OOG_OA, OOG_OL, OOG_OR, GATE_ACTIVITY_CODE, PAYMENT, MODIFY_DATE, RECORD_STATUS FROM LCIT_EDI_COPARN.dbo.COPARN_EDIDATA WHERE OPERATOR_CODE LIKE '" + getdt.Rows[checkdt]["OPERATOR_CODE"].ToString() + "'AND VESSEL_NAME LIKE '" + getdt.Rows[checkdt]["VESSEL_NAME"].ToString() + "'AND VOYAGE LIKE '" + getdt.Rows[checkdt]["VOYAGE"].ToString() + "'AND RECORD_STATUS LIKE '" + getdt.Rows[checkdt]["RECORD_STATUS"].ToString() + "'");

                cone.Open();

           

                // SqlDataAdapter sdaGetDetail = new SqlDataAdapter("SELECT [No], Booking_No, VESSEL_NAME, VOYAGE, OPERATOR_CODE, POL, POD, DST, CMDT, CNTR_NUM, [SIZE], [TYPE], HEIGHT, SS, LADENT_SATAUS, VGM, VGM_UOM, GROSSWEIGHT, GROSSWEIGHT_UOM, SHIPPER_NAME, CONSIGNEE_NAME, SEAL, ISNULL((SELECT ST.LCIT_CODE FROM LCIT_EDI_COPARN.dbo.STOW_CODE ST WHERE ST.OPERATOR_CODE LIKE '" + getdt.Rows[checkdt]["OPERATOR_CODE"].ToString() + "'AND ST.STOW_CODE = LCIT_EDI_COPARN.dbo.COPARN_EDIDATA.STOW_CODE),LCIT_EDI_COPARN.dbo.COPARN_EDIDATA.STOW_CODE) STOW_CODE, BLOCK_STOWAGE, VENTILATION, TMP, TMPUOM, DG_CLASS, OOG_OH, OOG_OF, OOG_OA, OOG_OL, OOG_OR, GATE_ACTIVITY_CODE, PAYMENT, MODIFY_DATE, RECORD_STATUS, IsAsDry FROM LCIT_EDI_COPARN.dbo.COPARN_EDIDATA WHERE OPERATOR_CODE LIKE '" + getdt.Rows[checkdt]["OPERATOR_CODE"].ToString() + "'AND VESSEL_NAME LIKE '" + getdt.Rows[checkdt]["VESSEL_NAME"].ToString() + "'AND VOYAGE LIKE '" + getdt.Rows[checkdt]["VOYAGE"].ToString() + "'AND RECORD_STATUS LIKE '" + getdt.Rows[checkdt]["RECORD_STATUS"].ToString() + "'", cone);

                // SqlDataAdapter sdaGetDetail = new SqlDataAdapter("SELECT [No], Booking_No,VESSEL_NAME,  (SELECT VSL_CODE FROM LCIT_EDI_COPARN.dbo.VESSEL_CODE WHERE  VESSEL_NAME like '" + getdt.Rows[checkdt]["VESSEL_NAME"].ToString() + "') + VOYAGE AS VOYAGE, OPERATOR_CODE, POL, POD, DST, CMDT, CNTR_NUM, [SIZE], [TYPE], HEIGHT, SS, LADENT_SATAUS, VGM, VGM_UOM, GROSSWEIGHT, GROSSWEIGHT_UOM, SHIPPER_NAME, CONSIGNEE_NAME, SEAL, ISNULL((SELECT ST.LCIT_CODE FROM LCIT_EDI_COPARN.dbo.STOW_CODE ST WHERE ST.OPERATOR_CODE LIKE '" + getdt.Rows[checkdt]["OPERATOR_CODE"].ToString() + "'AND ST.STOW_CODE = LCIT_EDI_COPARN.dbo.COPARN_EDIDATA.STOW_CODE), LCIT_EDI_COPARN.dbo.COPARN_EDIDATA.STOW_CODE) STOW_CODE, BLOCK_STOWAGE, VENTILATION, TMP, TMPUOM, DG_CLASS, OOG_OH, OOG_OF, OOG_OA, OOG_OL, OOG_OR, GATE_ACTIVITY_CODE, PAYMENT, MODIFY_DATE, RECORD_STATUS, IsAsDry FROM LCIT_EDI_COPARN.dbo.COPARN_EDIDATA WHERE OPERATOR_CODE LIKE '" + getdt.Rows[checkdt]["OPERATOR_CODE"].ToString() + "'AND VESSEL_NAME LIKE '" + getdt.Rows[checkdt]["VESSEL_NAME"].ToString() + "'AND VOYAGE LIKE '" + getdt.Rows[checkdt]["VOYAGE"].ToString() + "'AND RECORD_STATUS LIKE '" + getdt.Rows[checkdt]["RECORD_STATUS"].ToString() + "'", cone);

// 18-JUL-2022
                SqlDataAdapter sdaGetDetail = new SqlDataAdapter("SELECT [No], Booking_No, VESSEL_NAME, (SELECT CASE WHEN COUNT(*) > 1 THEN (SELECT ' ' + vsl1.VSL_CODE+','FROM LCIT_EDI_COPARN.dbo.VESSEL_CODE vsl1 WHERE vsl1.VESSEL_NAME =  VSS.VESSEL_NAME FOR XML PATH(''))  ELSE (SELECT '' + vsl1.VSL_CODE+''FROM LCIT_EDI_COPARN.dbo.VESSEL_CODE vsl1 WHERE vsl1.VESSEL_NAME =  VSS.VESSEL_NAME FOR XML PATH('')) END FROM LCIT_EDI_COPARN.dbo.VESSEL_CODE VSS WHERE VSS.VESSEL_NAME like '" + getdt.Rows[checkdt]["VESSEL_NAME"].ToString() + "' GROUP BY VSS.VESSEL_NAME) + VOYAGE AS VOYAGE, OPERATOR_CODE, POL, POD, DST, CMDT, CNTR_NUM, [SIZE], [TYPE], HEIGHT, SS, LADENT_SATAUS, VGM, VGM_UOM, GROSSWEIGHT, GROSSWEIGHT_UOM, SHIPPER_NAME, CONSIGNEE_NAME, SEAL, ISNULL((SELECT ST.LCIT_CODE FROM LCIT_EDI_COPARN.dbo.STOW_CODE ST WHERE ST.OPERATOR_CODE LIKE '" + getdt.Rows[checkdt]["OPERATOR_CODE"].ToString() + "'AND ST.STOW_CODE = LCIT_EDI_COPARN.dbo.COPARN_EDIDATA.STOW_CODE), LCIT_EDI_COPARN.dbo.COPARN_EDIDATA.STOW_CODE) STOW_CODE, BLOCK_STOWAGE, VENTILATION, TMP, TMPUOM, DG_CLASS, OOG_OH, OOG_OF, OOG_OA, OOG_OL, OOG_OR, GATE_ACTIVITY_CODE, PAYMENT, MODIFY_DATE, RECORD_STATUS, IsAsDry FROM LCIT_EDI_COPARN.dbo.COPARN_EDIDATA WHERE OPERATOR_CODE LIKE '" + getdt.Rows[checkdt]["OPERATOR_CODE"].ToString() + "'AND VESSEL_NAME LIKE '" + getdt.Rows[checkdt]["VESSEL_NAME"].ToString() + "'AND VOYAGE LIKE '" + getdt.Rows[checkdt]["VOYAGE"].ToString() + "'AND RECORD_STATUS LIKE '" + getdt.Rows[checkdt]["RECORD_STATUS"].ToString() + "'", cone);


                


                DataTable dtGetDetail = new DataTable();
                sdaGetDetail.Fill(dtGetDetail);

                cone.Close();

                foreach (DataRow dr in dtGetDetail.Rows)
                {
                    dtrow = dt.NewRow();
                    dtrow[0] = dr["VESSEL_NAME"].ToString(); // COLUMN : A
                    dtrow[1] = dr["VOYAGE"].ToString(); // COLUMN : B
                    dtrow[2] = dr["CNTR_NUM"].ToString(); // COLUMN : C
                    dtrow[3] = dr["OPERATOR_CODE"].ToString(); // COLUMN : D
                    dtrow[4] = dr["OPERATOR_CODE"].ToString(); // COLUMN : E
                    if(dr["SIZE"].ToString().Length > 2)
                    {
                        dtrow[5] = " ISO : " + dr["SIZE"].ToString(); // COLUMN : F
                    }
                    else
                    {
                        dtrow[5] = dr["SIZE"].ToString(); // COLUMN : F
                    }
                    
                    dtrow[6] = dr["TYPE"].ToString(); // COLUMN : G
                    dtrow[7] = dr["HEIGHT"].ToString(); // COLUMN : H
                    dtrow[8] = dr["SS"].ToString(); // COLUMN : I
                    dtrow[9] = dr["LADENT_SATAUS"].ToString(); // COLUMN : J
                    if (dr["BLOCK_STOWAGE"].ToString().Length > 0)
                    {
                        dtrow[10] = dr["POD"].ToString().Substring(2, 2) + dr["BLOCK_STOWAGE"].ToString(); // COLUMN : K
                    }
                    else
                    {
                        dtrow[10] = dr["POD"].ToString().Substring(2, 3); // COLUMN : K
                    }

                    if (dr["VGM"].ToString() == null || dr["VGM"].ToString() == "")
                    {
                        dtrow[11] = dr["GROSSWEIGHT"].ToString(); // COLUMN : L
                        dtrow[12] = dr["GROSSWEIGHT_UOM"].ToString(); // COLUMN : M

                    }
                    else
                    {
                        dtrow[11] = dr["VGM"].ToString(); // COLUMN : L
                        dtrow[12] = dr["VGM_UOM"].ToString(); // COLUMN : M
                    }

                    dtrow[13] = dr["Booking_No"].ToString(); // COLUMN : N
                    dtrow[14] = dr["PAYMENT"].ToString(); // COLUMN : O
                    dtrow[15] = dr["CONSIGNEE_NAME"].ToString(); // COLUMN : P
                    dtrow[16] = "THLCH"; // COLUMN : Q
                    dtrow[17] = dr["SHIPPER_NAME"].ToString(); // COLUMN : R
                    dtrow[18] = dr["DST"].ToString(); // COLUMN : S
                    dtrow[19] = dr["SEAL"].ToString(); // COLUMN : T
                    dtrow[20] = dr["STOW_CODE"].ToString(); // COLUMN : U
                    dtrow[21] = DateTime.Now.ToString("ddMMMyy-HHmmss").ToUpper() + "COP"; // COLUMN : V

                    if(dr["IsAsDry"].ToString() == "Y")
                    {
                        dtrow[22] = "As Dry"; // COLUMN : W
                    }
                    else
                    {
                        dtrow[22] = dr["TMP"].ToString(); // COLUMN : W
                    }
                    
                    dtrow[23] = dr["TMPUOM"].ToString(); // COLUMN : X

                    if (dr["VENTILATION"].ToString().Length > 0)
                    {
                        if(dr["VENTILATION"].ToString().Substring(0, dr["VENTILATION"].ToString().Length - 3) == "0")
                        {
                            dtrow[24] = ""; // COLUMN : Y
                            dtrow[25] = ""; // COLUMN : Z
                        }
                        else
                        {
                            dtrow[24] = dr["VENTILATION"].ToString().Substring(0, dr["VENTILATION"].ToString().Length - 3); // COLUMN : Y
                            
                            if(dr["VENTILATION"].ToString().Substring(dr["VENTILATION"].ToString().Length - 3, 3) == "PCT")
                            {
                                dtrow[25] = "PO"; // COLUMN : Z
                            }
                            else
                            {
                                dtrow[25] = dr["VENTILATION"].ToString().Substring(dr["VENTILATION"].ToString().Length - 3, 3); // COLUMN : Z
                            }

                            
                        }
                       
                    }
                    else
                    {
                        dtrow[24] = "";
                        dtrow[25] = "";
                    }
                     if(dr["IsAsDry"].ToString() == "Y")
                     {
                         dtrow[26] = "Reefer As Dry " + dr["CMDT"].ToString(); // COLUMN : AA 
                     }
                     else
                     {
                         dtrow[26] = dr["CMDT"].ToString(); // COLUMN : AA 
                     }
                          
                    dtrow[27] = ""; // COLUMN : AB      
                    dtrow[28] = ""; // COLUMN : AC      
                    dtrow[29] = ""; // COLUMN : AD      
                    dtrow[30] = ""; // COLUMN : AE      
                    dtrow[31] = ""; // COLUMN : AF      
                    dtrow[32] = ""; // COLUMN : AG 
                    dtrow[33] = dr["DG_CLASS"].ToString().Replace('-', ';'); // COLUMN : AH 
                    dtrow[34] = ""; // COLUMN : AI 
                    dtrow[35] = ""; // COLUMN : AJ 
                    dtrow[36] = ""; // COLUMN : AK 
                    dtrow[37] = ""; // COLUMN : AL
                    dtrow[38] = ""; // COLUMN : AM 
                    dtrow[39] = ""; // COLUMN : AN 

                    if(dr["IsAsDry"].ToString() == "Y")
                    {
                        dtrow[40] = "Reefer As Dry " + dr["OOG_OH"].ToString() + " " + dr["OOG_OF"].ToString() + " " + dr["OOG_OA"].ToString() + " " + dr["OOG_OL"].ToString() + " " + dr["OOG_OR"].ToString(); // COLUMN : AO 
                    }
                    else
                    {
                        dtrow[40] = dr["OOG_OH"].ToString() + " " + dr["OOG_OF"].ToString() + " " + dr["OOG_OA"].ToString() + " " + dr["OOG_OL"].ToString() + " " + dr["OOG_OR"].ToString(); // COLUMN : AO 
                    }
                    
                    dtrow[41] = ""; // COLUMN : AP 
                    dt.Rows.Add(dtrow);

                }

                using (var wb = new XLWorkbook())
                {
                    try
                    {

                        wb.Worksheets.Add(dt, "DataImport");
                        wb.Worksheets.Add(dtEX, "ColumnMapping");

                        wb.SaveAs(fp.xlsx.ToString() + getdt.Rows[checkdt]["OPERATOR_CODE"].ToString() + "\\" + getdt.Rows[checkdt]["RECORD_STATUS"].ToString() + "\\Shore Pre-advise_" + getdt.Rows[checkdt]["OPERATOR_CODE"].ToString() + "_Vessel-" + getdt.Rows[checkdt]["VESSEL_NAME"].ToString() + "_" + getdt.Rows[checkdt]["VOYAGE"].ToString() + "_" + DateTime.Now.ToString("dd.MM.yyyy.HH.mm.ss") + ".xlsx");

                        logfile += getdt.Rows[checkdt]["OPERATOR_CODE"].ToString() + " | " + DateTime.Now.ToString() + " --------------------- | export file :" + fp.xlsx.ToString() + getdt.Rows[checkdt]["OPERATOR_CODE"].ToString() + "\\" + getdt.Rows[checkdt]["RECORD_STATUS"].ToString() + "\\Shore Pre-advise_" + getdt.Rows[checkdt]["OPERATOR_CODE"].ToString() + "_Vessel-" + getdt.Rows[checkdt]["VESSEL_NAME"].ToString() + "_" + getdt.Rows[checkdt]["VOYAGE"].ToString() + "_" + DateTime.Now.ToString("dd.MM.yyyy.HH.mm.ss") + ".xlsx\r\n";

                        Console.WriteLine(logfile);

                        finalFileName = getdt.Rows[checkdt]["OPERATOR_CODE"].ToString() + "\\" + getdt.Rows[checkdt]["RECORD_STATUS"].ToString() + "\\Shore Pre-advise_" + getdt.Rows[checkdt]["OPERATOR_CODE"].ToString() + "_Vessel-" + getdt.Rows[checkdt]["VESSEL_NAME"].ToString() + "_" + getdt.Rows[checkdt]["VOYAGE"].ToString() + "_" + DateTime.Now.ToString("dd.MM.yyyy.HH.mm.ss") + ".xls";

                        // convertToXls(fp.xlsx.ToString() + getdt.Rows[checkdt]["OPERATOR_CODE"].ToString() + "\\" + getdt.Rows[checkdt]["RECORD_STATUS"].ToString() + "\\Shore Pre-advise " + getdt.Rows[checkdt]["OPERATOR_CODE"].ToString() + " Vessel -" + getdt.Rows[checkdt]["VESSEL_NAME"].ToString() + " " + getdt.Rows[checkdt]["VOYAGE"].ToString() + " " + DateTime.Now.ToString("dd.MM.yyyy HH.mm") + ".xlsx", finalFileName.ToString());

                    }
                    catch (Exception ex)
                    {
                        logfile += getdt.Rows[checkdt]["OPERATOR_CODE"].ToString() + " | "+DateTime.Now.ToString()+" --------------------- | export file :" + ex.Message.ToString();
                    }
                }
                 for(int statusUp = 0 ; statusUp < dtGetDetail.Rows.Count;statusUp++)
                 {

                        cone.Open();

                        SqlDataAdapter sdaUpdateStatus = new SqlDataAdapter("UPDATE LCIT_EDI_COPARN.dbo.COPARN_EDIDATA SET RECORD_STATUS='INACTIVE' WHERE [No]="+dtGetDetail.Rows[statusUp][0].ToString()+"",cone);

                        

                        DataTable dtstatusUp = new DataTable();
                        sdaUpdateStatus.Fill(dtstatusUp);
                        cone.Close();
                 }    
                 
                



                dt.Clear();
                dt.Rows.Clear();
                dt.Columns.Clear();

                dtEX.Clear();
                dtEX.Rows.Clear();
                dtEX.Columns.Clear();

                tolog.toLogfile(logfile);

                convertToXls(getdt.Rows[checkdt]["OPERATOR_CODE"].ToString(), getdt.Rows[checkdt]["RECORD_STATUS"].ToString());

                sendEmail(getdt.Rows[checkdt]["OPERATOR_CODE"].ToString(), getdt.Rows[checkdt]["RECORD_STATUS"].ToString());

            }

        }

        public static void setGridView()
        {
            //-----------------------------------------------------------------------------------------------------------
            //-------------------------------------- SET COLUMNS DATAGRIDVIEW -------------------------------------------
            //-----------------------------------------------------------------------------------------------------------
            dt.Columns.Add("Vessel Name", typeof(string)); // mandatory
            dt.Columns.Add("Vessel Visit", typeof(string)); // mandatory
            dt.Columns.Add("Cntr No", typeof(string)); // mandatory
            dt.Columns.Add("Opr", typeof(string));
            dt.Columns.Add("Owner", typeof(string)); // mandatory
            dt.Columns.Add("Size", typeof(string)); // mandatory
            dt.Columns.Add("Type", typeof(string)); // mandatory
            dt.Columns.Add("Height", typeof(string)); // mandatory
            dt.Columns.Add("SS", typeof(string));
            dt.Columns.Add("Status", typeof(string)); // mandatory
            dt.Columns.Add("POD1", typeof(string)); // mandatory
            dt.Columns.Add("Gross Wt", typeof(string));
            dt.Columns.Add("Wt UOM", typeof(string));
            dt.Columns.Add("Bkg No", typeof(string));
            dt.Columns.Add("Landside Bill Method", typeof(string));
            dt.Columns.Add("Shipper/Consignee A/C No", typeof(string));
            dt.Columns.Add("ORG", typeof(string));
            dt.Columns.Add("Shipper", typeof(string));
            dt.Columns.Add("DST", typeof(string));
            dt.Columns.Add("Dcl. Seal # 1", typeof(string));
            dt.Columns.Add("SPC1", typeof(string));
            dt.Columns.Add("File Ref No", typeof(string));
            dt.Columns.Add("Temp Required", typeof(string));
            dt.Columns.Add("Temp UOM", typeof(string));
            dt.Columns.Add("Vent Set", typeof(string));
            dt.Columns.Add("Vent UOM", typeof(string));
            dt.Columns.Add("CommodityDesc", typeof(string)); // mandatory
            dt.Columns.Add("POD2", typeof(string));
            dt.Columns.Add("VGM", typeof(string));
            dt.Columns.Add("VGM Date", typeof(string));
            dt.Columns.Add("VGM Party", typeof(string));
            dt.Columns.Add("VGM Src", typeof(string));
            dt.Columns.Add("Seal # 1", typeof(string));
            dt.Columns.Add("IMO(UNDG)", typeof(string));
            dt.Columns.Add("OH (CM)", typeof(string));
            dt.Columns.Add("OF (CM)", typeof(string));
            dt.Columns.Add("OA (CM)", typeof(string));
            dt.Columns.Add("OL (CM)", typeof(string));
            dt.Columns.Add("OR (CM)", typeof(string));
            dt.Columns.Add("Discharge ATB", typeof(string));
            dt.Columns.Add("Remark", typeof(string));
            dt.Columns.Add("Good Transit control No.", typeof(string));

            dtEX.Columns.Add("ColumnName", typeof(string));
            dtEX.Columns.Add("FieldName", typeof(string));

            for (int i = 0; i < 43; i++)
            {
                dtEX.Rows.Add();
            }
            dtEX.Rows[0][0] = "Vessel Visit";
            dtEX.Rows[1][0] = "Cntr No";
            dtEX.Rows[2][0] = "Opr";
            dtEX.Rows[3][0] = "Owner";
            dtEX.Rows[4][0] = "Size";
            dtEX.Rows[5][0] = "Type";
            dtEX.Rows[6][0] = "Height";
            dtEX.Rows[7][0] = "SS";
            dtEX.Rows[8][0] = "Status";
            dtEX.Rows[9][0] = "POD1";
            dtEX.Rows[10][0] = "POD2";
            dtEX.Rows[11][0] = "DST";
            dtEX.Rows[12][0] = "Dcl. Seal # 1";
            dtEX.Rows[13][0] = "Seal # 1";
            dtEX.Rows[14][0] = "IMO(UNDG)";
            dtEX.Rows[15][0] = "OH (CM)";
            dtEX.Rows[16][0] = "OF (CM)";
            dtEX.Rows[17][0] = "OA (CM)";
            dtEX.Rows[18][0] = "OL (CM)";
            dtEX.Rows[19][0] = "OR (CM)";
            dtEX.Rows[20][0] = "Temp Required";
            dtEX.Rows[21][0] = "Temp UOM";
            dtEX.Rows[22][0] = "Vent Set";
            dtEX.Rows[23][0] = "Vent UOM";
            dtEX.Rows[24][0] = "Landside Bill Method";
            dtEX.Rows[25][0] = "Shipper/Consignee A/C No";
            dtEX.Rows[26][0] = "Shipper";
            dtEX.Rows[27][0] = "Gross Wt";
            dtEX.Rows[28][0] = "Wt UOM";
            dtEX.Rows[29][0] = "Bkg No";
            dtEX.Rows[30][0] = "ORG";
            dtEX.Rows[31][0] = "Commodity Code";
            dtEX.Rows[32][0] = "SPC1";
            dtEX.Rows[33][0] = "Remark";
            dtEX.Rows[34][0] = "File Ref No";
            dtEX.Rows[35][0] = "CommodityDesc";
            dtEX.Rows[36][0] = "Discharge ATB";
            dtEX.Rows[37][0] = "Special Oper Code";
            dtEX.Rows[38][0] = "Good Transit control No.";
            dtEX.Rows[39][0] = "VGM Src";
            dtEX.Rows[40][0] = "VGM";
            dtEX.Rows[41][0] = "VGM Date";
            dtEX.Rows[42][0] = "VGM Party";
            dtEX.Rows[0][1] = "VslVisit";
            dtEX.Rows[1][1] = "ContainerNo";
            dtEX.Rows[2][1] = "Opr";
            dtEX.Rows[3][1] = "Owner";
            dtEX.Rows[4][1] = "SizeTypeSize";
            dtEX.Rows[5][1] = "SizeTypeType";
            dtEX.Rows[6][1] = "SizeTypeHeight";
            dtEX.Rows[7][1] = "ShippingStatusCode";
            dtEX.Rows[8][1] = "LoadStatus";
            dtEX.Rows[9][1] = "Pod1";
            dtEX.Rows[10][1] = "Pod2";
            dtEX.Rows[11][1] = "Dst";
            dtEX.Rows[12][1] = "DeclaredSealNumber1";
            dtEX.Rows[13][1] = "SealNumber1";
            dtEX.Rows[14][1] = "IMCOs";
            dtEX.Rows[15][1] = "OHCM";
            dtEX.Rows[16][1] = "OFCM";
            dtEX.Rows[17][1] = "OBCM";
            dtEX.Rows[18][1] = "OLCM";
            dtEX.Rows[19][1] = "ORCM";
            dtEX.Rows[20][1] = "Temp_UI";
            dtEX.Rows[21][1] = "TempUOM";
            dtEX.Rows[22][1] = "VentSetting";
            dtEX.Rows[23][1] = "VentUnitCode";
            dtEX.Rows[24][1] = "LandsideBillMethod";
            dtEX.Rows[25][1] = "LandsideBillAccount";
            dtEX.Rows[26][1] = "ShipperName";
            dtEX.Rows[27][1] = "GrossWgtUI";
            dtEX.Rows[28][1] = "WeightUom";
            dtEX.Rows[29][1] = "BookingNo";
            dtEX.Rows[30][1] = "Org";
            dtEX.Rows[31][1] = "CommodityCode";
            dtEX.Rows[32][1] = "AlternativeStowCode";
            dtEX.Rows[33][1] = "OperRemarks";
            dtEX.Rows[34][1] = "UserDefinedName4";
            dtEX.Rows[35][1] = "CommodityDesc";
            dtEX.Rows[36][1] = "ContainerDischargeDate";
            dtEX.Rows[37][1] = "SpecialOperationCode";
            dtEX.Rows[38][1] = "GoodsTransitcontrolno";
            dtEX.Rows[39][1] = "CntrVgmSource";
            dtEX.Rows[40][1] = "CntrVgm_UI";
            dtEX.Rows[41][1] = "CntrVgmTime";
            dtEX.Rows[42][1] = "CntrVgmPartyInfo";

        }

        public static void convertToXls(string Operator, string recordstatus)
        {

            string flepath = fp.xlsx.ToString() + Operator + "\\" + recordstatus + "\\";
            string flepath2 = fp.xls.ToString() + Operator + "\\" + recordstatus + "\\";
            DirectoryInfo di = new DirectoryInfo(@flepath);
            DirectoryInfo di2 = new DirectoryInfo(@flepath2);

            foreach (FileInfo fle in di.GetFiles("*.*"))
            {

                book.LoadFromFile(di.ToString() + fle.Name);
                book.SaveToFile(di2.ToString() + fle.Name.ToString().Substring(0, fle.Name.Length - 1), ExcelVersion.Version97to2003);

                // Console.WriteLine(di2.ToString() + fle.Name.ToString());

                logfile += Operator + " | " + DateTime.Now.ToString() + "--------------------- | Convert file to:" + di2.ToString() + fle.Name.ToString().Substring(0, fle.Name.Length - 1) + "\r\n";

                File.Delete(di.ToString() + fle.Name);

            }
            // 
            // book.SaveToFile(fp.xls.ToString() + FleName.ToString());

            //
            //Console.WriteLine(pathOrigin);

        }

        public static void sendEmail(String Operator, String recordStatus)
        {
            string filePath = fp.xls.ToString() + Operator + "\\" + recordStatus + "\\";
            string bodyContact ="<style type='text/css'> .tg  {border-collapse:collapse;border-color:#93a1a1;border-spacing:0;} .tg td{background-color:#fdf6e3;border-color:#93a1a1;border-style:solid;border-width:1px;color:#002b36; font-family:Arial, sans-serif;font-size:14px;overflow:hidden;padding:10px 5px;word-break:normal;} .tg th{background-color:#657b83;border-color:#93a1a1;border-style:solid;border-width:1px;color:#fdf6e3; font-family:Arial, sans-serif;font-size:14px;font-weight:normal;overflow:hidden;padding:10px 5px;word-break:normal;} .tg .tg-ytkg{background-color:#eee8d5;border-color:inherit;font-size:12px;text-align:center;vertical-align:middle} .tg .tg-4bi3{background-color:#000000;border-color:#000000;color:#ffffff;font-family:Arial, Helvetica, sans-serif !important;; font-size:12px;font-weight:bold;text-align:center;vertical-align:bottom} .tg .tg-9o1m{border-color:inherit;font-size:12px;text-align:center;vertical-align:middle} .tg .tg-u9r8{border-color:inherit;font-size:12px;text-align:center;vertical-align:bottom} .tg .tg-02fc{background-color:#eee8d5;border-color:inherit;font-size:12px;text-align:left;vertical-align:bottom} .tg .tg-2776{border-color:inherit;font-size:12px;text-align:left;vertical-align:bottom} </style> <table class='tg'> <thead> <tr> <th class='tg-4bi3' colspan='3'>   <br>LAEM CHABANG TERMINAL   TEAM (TML LCH)   </th> </tr> </thead> <tbody> <tr> <td class='tg-ytkg' colspan='3'>&nbsp;&nbsp;&nbsp;<br><span style='color:black'>Group email : </span><a href='mailto:th.ope.tml.lch.ex@one-line.com'>th.ope.tml.lch.ex@one-line.com</a>&nbsp;&nbsp;&nbsp;</td> </tr> <tr> <td class='tg-9o1m'>&nbsp;&nbsp;&nbsp;<br><span style='color:black'>NAME</span>&nbsp;&nbsp;&nbsp;</td> <td class='tg-9o1m'>&nbsp;&nbsp;&nbsp;<br><span style='color:black'>Telephone</span>&nbsp;&nbsp;&nbsp;</td> <td class='tg-u9r8'>&nbsp;&nbsp;&nbsp;<br><span style='color:black'>Mobile</span>&nbsp;&nbsp;&nbsp;</td> </tr> <tr> <td class='tg-02fc'>&nbsp;&nbsp;&nbsp;<br>Mr.&nbsp;&nbsp;&nbsp;Kritsana Prongcharoen&nbsp;&nbsp;&nbsp;</td> <td class='tg-02fc'>&nbsp;&nbsp;&nbsp;<br>033-311-8144 &nbsp;&nbsp;&nbsp;</td> <td class='tg-02fc'>&nbsp;&nbsp;&nbsp;<br>064-219-4195&nbsp;&nbsp;&nbsp;</td> </tr> <tr> <td class='tg-2776'>&nbsp;&nbsp;&nbsp;<br>Mr.&nbsp;&nbsp;&nbsp;Chaiyapat Innarong&nbsp;&nbsp;&nbsp;</td> <td class='tg-2776'>&nbsp;&nbsp;&nbsp;<br>033-311-8143&nbsp;&nbsp;&nbsp;</td> <td class='tg-2776'>&nbsp;&nbsp;&nbsp;<br>093-459-1654&nbsp;&nbsp;&nbsp;</td> </tr> <tr> <td class='tg-02fc'>&nbsp;&nbsp;&nbsp;<br>Mr.&nbsp;&nbsp;&nbsp;Narin Jittakit&nbsp;&nbsp;&nbsp;</td> <td class='tg-02fc'>&nbsp;&nbsp;&nbsp;<br>033-311-8139&nbsp;&nbsp;&nbsp;</td> <td class='tg-02fc'>&nbsp;&nbsp;&nbsp;<br>094-496-1916&nbsp;&nbsp;&nbsp;</td> </tr> <tr> <td class='tg-2776'>&nbsp;&nbsp;&nbsp;<br>Ms.&nbsp;&nbsp;&nbsp;Patra Thongsawang&nbsp;&nbsp;&nbsp;</td> <td class='tg-2776'>&nbsp;&nbsp;&nbsp;<br>033-311-8152&nbsp;&nbsp;&nbsp;</td> <td class='tg-2776'>&nbsp;&nbsp;&nbsp;<br>064-219-5526&nbsp;&nbsp;&nbsp;</td> </tr> <tr> <td class='tg-02fc'>&nbsp;&nbsp;&nbsp;<br>Ms.&nbsp;&nbsp;&nbsp;Aunchalee Promsorn&nbsp;&nbsp;&nbsp;</td> <td class='tg-02fc'>&nbsp;&nbsp;&nbsp;<br>033-311-8146&nbsp;&nbsp;&nbsp;</td> <td class='tg-02fc'>&nbsp;&nbsp;&nbsp;<br>064-219-5485&nbsp;&nbsp;&nbsp;</td> </tr> <tr> <td class='tg-2776'>&nbsp;&nbsp;&nbsp;<br>Mr.&nbsp;&nbsp;&nbsp;Cholladech Kultakerng&nbsp;&nbsp;&nbsp;</td> <td class='tg-2776'>&nbsp;&nbsp;&nbsp;<br>033-311-8145&nbsp;&nbsp;&nbsp;</td> <td class='tg-2776'>&nbsp;&nbsp;&nbsp;<br>081-491-4891&nbsp;&nbsp;&nbsp;</td> </tr> <tr> <td class='tg-02fc'>&nbsp;&nbsp;&nbsp;<br>Ms.&nbsp;&nbsp;&nbsp;Pichaya Kamwong&nbsp;&nbsp;&nbsp;</td> <td class='tg-02fc'>&nbsp;&nbsp;&nbsp;<br>033-311-8159&nbsp;&nbsp;&nbsp;</td> <td class='tg-02fc'>&nbsp;&nbsp;&nbsp;<br>088-415-4198&nbsp;&nbsp;&nbsp;</td> </tr> <tr> <td class='tg-2776'>&nbsp;&nbsp;&nbsp;<br>Mr.&nbsp;&nbsp;&nbsp;Prasert Phlynern&nbsp;&nbsp;&nbsp;</td> <td class='tg-2776'>&nbsp;&nbsp;&nbsp;<br>033-311-8140&nbsp;&nbsp;&nbsp;</td> <td class='tg-2776'>&nbsp;&nbsp;&nbsp;<br>064-219-5538&nbsp;&nbsp;&nbsp;</td> </tr> </tbody> </table>";
            DirectoryInfo di = new DirectoryInfo(@filePath);

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("172.19.240.77");
            System.Net.Mail.Attachment attachment;

            mail.From = new MailAddress(Operator.ToString() + "_coparn_edi@lcit.com");
            mail.IsBodyHtml = true;
            // mail.To.Add("<Auto-PreAdvise@lcit.com>,<doc.officer@lcit.com>,<Document.Controller@lcit.com>");
            // mail.CC.Add("<awichan@lcit.com>");
			// mail.Bcc.Add("<it.edi@lcit.com>");
            mail.To.Add("<psaksit@lcit.com>");
            // mail.CC.Add("");

            foreach (FileInfo fi in di.GetFiles("*.*"))
            {
                mail.Subject = fi.Name.ToString().Replace(".xls", "") + " : " + recordStatus.ToString() + " File";
                if (recordStatus.ToString() == "Original")
                {
                    mail.Body = "Please see " + recordStatus + " in attached file for pre-advise EDI " + DateTime.Now.ToString("dd-MMM-yyyy") + ". <br> If any problem please reply to Below contact and CC it@lcit.com <br><br>" + bodyContact;
                }
                else if (recordStatus.ToString() == "Replace")
                {
                    mail.Body = "Please see " + recordStatus + " in attached file for pre-advise EDI " + DateTime.Now.ToString("dd-MMM-yyyy") + ". <br> If any problem please reply to Below contact and CC it@lcit.com <br><br>"+ bodyContact;
                }
                else if (recordStatus.ToString() == "Cancellation")
                {
                    mail.Body = "<h2>Cancle Shore</h2>"+"Please see " + recordStatus + " in attached file for pre-advise EDI " + DateTime.Now.ToString("dd-MMM-yyyy") + ". If any problem please reply to Below contact and CC it@lcit.com <br><br>"+ bodyContact;
                }

                attachment = new System.Net.Mail.Attachment(fi.Directory.ToString()+"\\" + fi.Name.ToString());
                mail.Attachments.Add(attachment);

                logfile += Operator + " | " + DateTime.Now.ToString() + " --------------------- | send email file :" + fi.Name.ToString() + "\r\n";

                SmtpServer.Port = 25;
                SmtpServer.Credentials = new System.Net.NetworkCredential("lcit\administrator", PS("QERNNDA4TEAzbQ =="));
                SmtpServer.Send(mail);
                mail.Attachments.Dispose();

                try
                {

                    fi.MoveTo(Path.Combine(fi.Directory.ToString() + fi.Name.ToString(), fi.Directory.ToString() + "\\backup\\" + fi.Name.ToString()));
                }
                catch (Exception ex)
                {
                    logfile += Operator + " |"+DateTime.Now.ToString()+" --------------------- | to backup :" + ex.Message + "\r\n";
                }

                // File.Delete(di.ToString() + fi.Name.ToString());
            }
        }

        public static string PS(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);

            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string ReadEDIfile(String operatorCode)
        {
            string ResultAPI = "";
            try
            {
                var client = new RestClient("http://lcitedi-coparn.lcit.com/api/coparn.asmx/translateCoparn");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("operatorCode", operatorCode);
                IRestResponse response = client.Execute(request);
                ResultAPI = response.Content.ToString();
               // Console.WriteLine(response.Content);
            }
            catch (Exception ex)
            {
                ResultAPI = ex.Message.ToString();
                //Console.WriteLine(ex.Message);
            }

            return ResultAPI;
        }
    }
}