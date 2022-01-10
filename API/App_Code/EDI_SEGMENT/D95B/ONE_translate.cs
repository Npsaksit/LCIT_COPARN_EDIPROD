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

namespace edi_translate
{
    public class ONE_translate
    {
        cvfile.convertISO cviso = new cvfile.convertISO();
        int checkStowag = 0;

        public string checkEDIVersion(String[] EDI)
        {
            int countUNH = 0;
            string result = "";
            for (int edi_file = 0; edi_file < EDI.Count(); edi_file++)
            {
                // result += EDI[edi_file].ToString();

                switch (EDI[edi_file].Substring(0, 3).ToString())
                {
                    case "UNH":
                        result += (string)UNH(new string[] {
            EDI[edi_file]
          });
                        break;
                    default:
                        break;
                }

            }
            return result;
        }

        // -------------------------------------------------------------
        // -------------------  MESSAGE DEFINITION ---------------------
        // -------------------------------------------------------------
        public string UNH(String[] element)
        {
            // -------------------------------------------------------------
            // -------------------   Message header ---------------------
            // -------------------------------------------------------------
            string resultUNH = "";
            int countUNH = 0;
            int start = 0;
            for (int checkVersion = 0; checkVersion < element.Count(); checkVersion++)
            {
                for (int unhseg = 0; unhseg < element[0].Count() - 1; unhseg++)
                {
                    if (element[checkVersion][unhseg].ToString() == "+")
                    {
                        countUNH++;
                        if (countUNH == 2)
                        {
                            start = unhseg + 1;
                        }
                    }
                    if (countUNH == 2)
                    {
                        if (unhseg < (start + 11))
                        {
                            resultUNH = resultUNH + element[checkVersion][unhseg + 1].ToString();

                        }
                    }
                }
            }
            return resultUNH;
        }

        public string translateEDI(String[] EDI)
        {
            int countbooking = 0;
            int countEQD = 0;
            int countFTX = 0;
            string BGM_Result = "";
            string RFF_Result = "";
            string TDT_Result = "";
            string NAD_Result = "";
            string FTX_Result = "";
            string resultEDI = "";
            string DGS_Result = "";
            resultEDI = "[{";
            for (int edi_file = 0; edi_file < EDI.Count(); edi_file++)
            {
                switch (EDI[edi_file].Substring(0, 3).ToString())
                {
                    case "BGM":
                        BGM_Result += (string)BGM(new string[] {
            EDI[edi_file]
          });
                        // resultEDI += EDI[edi_file].ToString();

                        break;
                    case "TMD":
                        break;
                    case "TSR":
                        break;
                    case "FTX":
                        countFTX++;
                        if (countEQD > 0)
                        {
                            resultEDI += (string)FTX(new string[] {
              EDI[edi_file]
            }, countFTX);
                        }
                        else
                        {
                            FTX_Result += (string)FTX(new string[] {
              EDI[edi_file]
            }, countFTX);
                        }

                        break;
                    case "RFF":
                        countbooking++;
                        if (countEQD > 0)
                        {
                            resultEDI += (string)RFF(new string[] {
              EDI[edi_file]
            });
                        }
                        else
                        {
                            RFF_Result += (string)RFF(new string[] {
              EDI[edi_file]
            });
                        }
                        break;
                    case "TDT":

                        TDT_Result += (string)TDT(new string[] {
            EDI[edi_file]
          });
                        break;
                    case "NAD":
                        if (countEQD > 0)
                        {
                            resultEDI += (string)NAD(new string[] {
              EDI[edi_file]
            });
                        }
                        else
                        {
                            NAD_Result += (string)NAD(new string[] {
              EDI[edi_file]
            });
                        }

                        break;
                    case "GID":
                        break;
                    case "HAN":
                        break;
                    case "LOC":
                        resultEDI += (string)LOC(new string[] {
            EDI[edi_file]
          });
                        break;
                    case "DTM":
                        break;
                    case "EQD":
                        resultEDI = resultEDI.Remove(resultEDI.Length - 1);
                        if (countEQD == 0)
                        {
                            resultEDI += "{";
                        }
                        else
                        {
                            resultEDI += "},{";
                        }

                        countEQD++;
                        checkStowag =0;

                        resultEDI += BGM_Result + TDT_Result + FTX_Result + (string)EQD(new string[] {
            EDI[edi_file]
          }) + NAD_Result + DGS_Result;
                        break;
                    case "EQN":
                        break;
                    case "SEL":
                        resultEDI += (string)SEL(new string[] {
            EDI[edi_file]
          });
                        break;
                    case "CNT":
                        break;
                    case "UNZ":
                        break;
                    case "CTA":
                        break;
                    case "PIA":
                        break;
                    case "SGP":
                        break;
                    case "DGS":
                        if (countEQD > 0)
                        {
                            resultEDI += (string)DGS(new string[] {
              EDI[edi_file]
            });
                        }
                        else
                        {
                            DGS_Result += (string)DGS(new string[] {
              EDI[edi_file]
            });
                        }

                        break;
                    case "DIM":
                        resultEDI += (string)DIM(new string[] {
            EDI[edi_file]
          });
                        break;
                    case "TMP":
                        resultEDI += (string)TMP(new string[] {
            EDI[edi_file]
          });
                        break;
                    case "RNG":
                        break;
                    case "MEA":
                        resultEDI += (string)MEA(new string[] {
            EDI[edi_file]
          });
                        break;
                    case "GOR":
                        break;
                    case "EQA":
                        break;
                    case "COD":
                        break;
                    case "COM":
                        break;
                    case "UNT":
                        break;
                    default:
                        // resultEDI += EDI[edi_file];
                        break;
                }
            }
            resultEDI = resultEDI.Remove(resultEDI.Length - 1);
            resultEDI += "}]";
            return resultEDI;
        }
        public string BGM(String[] BGMelement)
        {
            string elec002 = "";
            string elec002_1001 = "";
            string elec002_1131 = "";
            string elec002_3055 = "";
            string elec002_1000 = "";

            string ele1004 = "";
            string ele1225 = "";
            string ele4343 = "";

            int checkBGMP = 0;
            int checkBGMSub = 0;

            string finalBGM = "";

            for (int checkBGM = 0; checkBGM < BGMelement.Count(); checkBGM++)
            {
                for (int checkBGMCo = 0; checkBGMCo < BGMelement[0].Count() - 1; checkBGMCo++)
                {
                    if (BGMelement[checkBGM][checkBGMCo].ToString() == "+")
                    {
                        checkBGMP++;
                    }

                    if (checkBGMP == 3)
                    {
                        ele1225 += BGMelement[checkBGM][checkBGMCo].ToString();
                    }
                }
            }
            ele1225 = ele1225.Remove(0, 1);
            if (ele1225 == "9")
            {
                finalBGM = "Original";
            }
            else if (ele1225 == "5")
            {
                finalBGM = "Replace";
            }
            else if (ele1225 == "1")
            {
                finalBGM = "Cancellation";
            }
            else
            {
                finalBGM = "Message Status Error (BGM)";
            }
            return "\"Message Status\":" + "\"" + finalBGM + "\"" + ",";
        }

        public string RFF(String[] RFFelement)
        {
            int checkElementRFF = 0;
            string typeRFF = "";
            string resultRFF = "";
            string finalRFF = "";
            string IsAsDry = "";

            for (int checkRFF = 0; checkRFF < RFFelement.Count(); checkRFF++)
            {
                for (int checkRFFCo = 0; checkRFFCo < RFFelement[0].Count() - 1; checkRFFCo++)
                {
                    if (RFFelement[checkRFF][checkRFFCo].ToString() == "+" || RFFelement[checkRFF][checkRFFCo].ToString() == ":")
                    {
                        checkElementRFF++;
                    }
                    if (checkElementRFF == 1)
                    {
                        typeRFF += RFFelement[checkRFF][checkRFFCo].ToString();
                    }
                    if (checkElementRFF == 2)
                    {
                        resultRFF += RFFelement[checkRFF][checkRFFCo].ToString();
                    }
                }
            }

            if (typeRFF.Remove(0, 1) == "BN")
            {
                finalRFF = "\"Booking Reference\":" + "\"" + resultRFF.ToString().Remove(0, 1) + "\"" + ",";
            }
            // if (typeRFF.Remove(0, 1) == "VON")
            // {
            //     finalRFF = "\"Voyage Referance\":" + "\"" + resultRFF.ToString().Remove(0, 1) + "\"" + ",";
            // }

            if (typeRFF.Remove(0, 1) == "CU")
            {
                finalRFF = "\"Shipper Name\":" + "\"" + resultRFF.ToString().Remove(0, 1).Replace(",", "").Replace("'", "") + "\"" + ",";
            }

            if (typeRFF.Remove(0, 1) == "BST")
            {
                if (resultRFF.Remove(0, 1).ToString() != "N")
                {
                    finalRFF = "\"Block Stowage\":" + "\"" + resultRFF.ToString().Remove(0, 1).Replace(",", "").Replace("'", "") + "\"" + ",";
                }

            }
            if(typeRFF.Remove(0,1) == "RAD")
            {
                finalRFF = "\"ReeferAsDry\":"+ "\"" + resultRFF.Remove(0,1).ToString()+"\",";
            }
            return finalRFF;
        }

        public string TDT(String[] TDTelement)
        {
            int checkElementTDT = 0;
            int checkSubElementTDT = 0;
            string typeTDT = "";
            string result = "";
            string finalTDT = "";
            string voyage = "";
            string vesselName = "";

            if (TDTelement[0].Substring(0, 6).ToString() == "TDT+20")
            {
                for (int checkTDT = 0; checkTDT < TDTelement.Count(); checkTDT++)
                {
                    for (int checkTDTCo = 0; checkTDTCo < TDTelement[0].Count() - 1; checkTDTCo++)
                    {

                        if (TDTelement[checkTDT][checkTDTCo].ToString() == "+")
                        {
                            checkElementTDT++;
                        }
                        if (checkElementTDT == 2)
                        {
                            voyage += TDTelement[checkTDT][checkTDTCo].ToString();
                        }

                        if (checkElementTDT == 8)
                        {
                            if (TDTelement[checkTDT][checkTDTCo].ToString() == ":")
                            {
                                checkSubElementTDT++;
                            }
                            if (checkSubElementTDT == 3)
                            {
                                vesselName += TDTelement[checkTDT][checkTDTCo].ToString();
                            }

                        }
                    }
                }

                voyage = "\"Voyage\":" + "\"" + voyage.ToString().Remove(0, 1) + "\"" + ",";
                vesselName = "\"Vessel Name\":" + "\"" + vesselName.ToString().Remove(0, 1) + "\"" + ",";
            }
            else
            {
                // TDT+.......
            }

            return voyage + vesselName;
        }

        public string LOC(String[] LOCelement)
        {

            int checkelementLOC = 0;
            string typeLOC = "";
            string detailLOC = "";
            string resultLOC = "";

            for (int checkLOC = 0; checkLOC < LOCelement.Count(); checkLOC++)
            {
                for (int checkLOCCo = 0; checkLOCCo < LOCelement[0].Count() - 1; checkLOCCo++)
                {

                    if (LOCelement[checkLOC][checkLOCCo].ToString() == "+" || LOCelement[checkLOC][checkLOCCo].ToString() == ":")
                    {
                        checkelementLOC++;
                    }

                    if (checkelementLOC == 1)
                    {
                        typeLOC += LOCelement[checkLOC][checkLOCCo].ToString();
                    }

                    if (checkelementLOC == 2)
                    {
                        detailLOC += LOCelement[checkLOC][checkLOCCo].ToString();
                    }

                }
            }

            if (typeLOC.ToString().Remove(0, 1) == "7")
            {
                resultLOC = "\"DEL\":" + "\"" + detailLOC.Remove(0, 1) + "\",";

            }

            if (typeLOC.ToString().Remove(0, 1) == "9")
            {
                resultLOC = "\"POL\":" + "\"" + detailLOC.Remove(0, 1) + "\",";

            }

            if (typeLOC.ToString().Remove(0, 1) == "11")
            {
                resultLOC = "\"POD\":" + "\"" + detailLOC.Remove(0, 1) + "\",";

            }

            if (typeLOC.ToString().Remove(0, 1) == "88")
            {
                resultLOC = "\"POR\":" + "\"" + detailLOC.Remove(0, 1) + "\",";

            }

            if (typeLOC.ToString().Remove(0, 1) == "99")
            {

                if (detailLOC.ToString().Remove(0, 1) == "THLCH")
                {
                    detailLOC = "+COD";
                }
                else if (detailLOC.ToString().Remove(0, 1) == "THONE")
                {
                    detailLOC = "+CREDIT-LINER";
                }

                resultLOC = "\"PaymentType\":" + "\"" + detailLOC.Remove(0, 1) + "\",";

            }

            return resultLOC;

        }

        public string NAD(String[] NADelement)
        {

            int checkelementNAD = 0;
            string typeNAD = "";
            string resultNAD = "";
            string finalNAD = "";

            for (int checkNAD = 0; checkNAD < NADelement.Count(); checkNAD++)
            {
                for (int checkNADCo = 0; checkNADCo < NADelement[0].Count() - 1; checkNADCo++)
                {
                    if (NADelement[checkNAD][checkNADCo].ToString() == "+" || NADelement[checkNAD][checkNADCo].ToString() == ":")
                    {
                        checkelementNAD++;
                    }

                    if (checkelementNAD == 1)
                    {

                        typeNAD += NADelement[checkNAD][checkNADCo].ToString();
                    }

                    if (checkelementNAD == 2)
                    {
                        resultNAD += NADelement[checkNAD][checkNADCo].ToString();
                    }

                }

            }

            if (typeNAD.Remove(0, 1) == "CF")
            {
                finalNAD = "\"Operator Code\":" + "\"" + resultNAD.Remove(0, 1).Substring(0, 3).ToString() + "\",";
            }
            if (typeNAD.Remove(0, 1) == "CN")
            {
                finalNAD = "\"Consignee Name\":" + "\"" + resultNAD.Remove(0, 1).ToString() + "\",";
            }

            return finalNAD;
        }

        public string FTX(String[] FTXelement, int countFTX)
        {
            int checkElementFTX = 0;
            string typeFTX = "";
            string detailsFTX = "";
            string VENdetailFTX = "";
            string finalFTX = "";

            for (int checkFTX = 0; checkFTX < FTXelement.Count(); checkFTX++)
            {
                for (int checkFTXCo = 0; checkFTXCo < FTXelement[0].Count() - 1; checkFTXCo++)
                {
                    if (FTXelement[checkFTX][checkFTXCo].ToString() == "+")
                    {
                        checkElementFTX++;
                    }

                    if (checkElementFTX == 1)
                    {
                        typeFTX += FTXelement[checkFTX][checkFTXCo].ToString();
                    }
                    if (checkElementFTX == 3)
                    {
                        detailsFTX += FTXelement[checkFTX][checkFTXCo].ToString();
                    }
                    if (checkElementFTX == 4)
                    {

                        VENdetailFTX += FTXelement[checkFTX][checkFTXCo].ToString();

                    }
                }
            }

            if (typeFTX.Remove(0, 1).ToString() == "AAA")
            {
                detailsFTX = VENdetailFTX.ToString();
                if (countFTX < 2)
                {
                    finalFTX = "\"Commodity Description\":" + "\"" + detailsFTX.Remove(0, 1).ToString().Replace(",", "") + "\",";
                }
            }

            if (typeFTX.Remove(0, 1).ToString() == "OSI")
            {

                finalFTX = "\"Ventilation\":" + "\"" + VENdetailFTX.Remove(0, 1).ToString() + "\",";

            }

            if (typeFTX.Remove(0, 1).ToString() == "HAN")
            {
                
                try
                {
                    if(checkStowag == 0)
                    {
                        finalFTX = "\"Stowage\":" + "\"" + detailsFTX.Remove(0, 1).ToString() + "\",";
                        checkStowag++;
                    }
                else
                    {
                        
                        finalFTX = "\"Stowage"+checkStowag+"\":" + "\"" + detailsFTX.Remove(0, 1).ToString() + "\",";
                        checkStowag++;
                    }
                    
                    
                }
                catch (Exception ex)
                {
                    finalFTX = "\"Stowage\":" + "\"" + ex.Message + "\",";
                }

            }

            return finalFTX;

        }

        public string EQD(String[] EQDelement)
        {
            int checkElementEQD = 0;
            string typeEQD = "";
            string cntrNo = "";
            string ISO = "";
            string cntrStatus = "";
            string laden = "";
            string fianlEQD = "";

            for (int checkEQD = 0; checkEQD < EQDelement.Count(); checkEQD++)
            {
                for (int checkEQDCo = 0; checkEQDCo < EQDelement[0].Count() - 1; checkEQDCo++)
                {
                    if (EQDelement[checkEQD][checkEQDCo].ToString() == "+" || EQDelement[checkEQD][checkEQDCo].ToString() == ":")
                    {
                        checkElementEQD++;
                    }
                    if (checkElementEQD == 1)
                    {
                        typeEQD += EQDelement[checkEQD][checkEQDCo].ToString();

                    }
                    if (checkElementEQD == 2)
                    {
                        cntrNo += EQDelement[checkEQD][checkEQDCo].ToString();

                    }
                    if (checkElementEQD == 3)
                    {
                        ISO += EQDelement[checkEQD][checkEQDCo].ToString();

                    }
                    if (checkElementEQD == 7)
                    {
                        cntrStatus += EQDelement[checkEQD][checkEQDCo].ToString();

                    }
                    if (checkElementEQD == 8)
                    {
                        laden += EQDelement[checkEQD][checkEQDCo].ToString();

                    }

                }
            }

            if (typeEQD.Remove(0, 1).ToString() == "CN")
            {

                if (cntrStatus.Remove(0, 1).ToString() == "2")
                {
                    cntrStatus = "EX";
                }

                if (cntrStatus.Remove(0, 1).ToString() == "3")
                {
                    cntrStatus = "IM";
                }

                if (cntrStatus.Remove(0, 1).ToString() == "6")
                {
                    cntrStatus = "TS";
                }

                if (laden.Remove(0, 1).ToString() == "5")
                {
                    laden = "F";
                }

                if (laden.Remove(0, 1).ToString() == "4")
                {
                    laden = "E";
                }

                fianlEQD = "\"Container No\":" + "\"" + cntrNo.Remove(0, 1).ToString() + "\"," +
                  "\"ISO\":" + "\"" + cviso.iso_check(ISO.Remove(0, 1).ToString(), EQDelement[0].ToString()) + "\"," +
                  "\"Status\":" + "\"" + cntrStatus.ToString() + "\"," +
                  "\"Laden\":" + "\"" + laden.ToString() + "\",";

            }

            return fianlEQD;

        }

        public string SEL(String[] SELelement)
        {
            int checkelementSEL = 0;
            string detailSEL = "";
            string finalSEL = "";

            for (int checkSEL = 0; checkSEL < SELelement.Count(); checkSEL++)
            {
                for (int checkSELCo = 0; checkSELCo < SELelement[0].Count() - 1; checkSELCo++)
                {
                    if (SELelement[checkSEL][checkSELCo].ToString() == "+")
                    {
                        checkelementSEL++;
                    }

                    if (checkelementSEL == 1)
                    {

                        detailSEL += SELelement[checkSEL][checkSELCo].ToString();

                    }
                }
            }

            finalSEL = "\"Seal\":" + "\"" + detailSEL.Remove(0, 1).ToString() + "\",";

            return finalSEL;
        }

        public string MEA(String[] MEAelement)
        {
            int checkelementMEA = 0;
            string firstType = "";
            string typeMEA = "";
            string detailMEA = "";
            string weight = "";
            string weightUOM = "";
            string finalMEA = "";

            for (int checkMEA = 0; checkMEA < MEAelement.Count(); checkMEA++)
            {
                for (int checkMEACo = 0; checkMEACo < MEAelement[0].Count() - 1; checkMEACo++)
                {
                    if (MEAelement[checkMEA][checkMEACo].ToString() == "+" || MEAelement[checkMEA][checkMEACo].ToString() == ":")
                    {
                        checkelementMEA++;
                    }

                    if (checkelementMEA == 1)
                    {
                        firstType += MEAelement[checkMEA][checkMEACo].ToString();
                    }

                    if (checkelementMEA == 2)
                    {
                        typeMEA += MEAelement[checkMEA][checkMEACo].ToString();
                    }

                    if (checkelementMEA == 3)
                    {
                        weightUOM += MEAelement[checkMEA][checkMEACo].ToString();
                    }
                    if (checkelementMEA == 4)
                    {
                        weight += MEAelement[checkMEA][checkMEACo].ToString();
                    }

                }
            }

            if (firstType.Remove(0, 1).ToString() == "AAE")
            {
                if (typeMEA.Remove(0, 1).ToString() == "VGM")
                {
                    finalMEA = "\"VGM\":" + "\"" + weight.Remove(0, 1).ToString() + "\"," +
                      "\"VGMUOM\":" + "\"" + weightUOM.Remove(0, 1).Substring(0, 2).ToString() + "\",";
                }
                if (typeMEA.Remove(0, 1).ToString() == "G")
                {
                    finalMEA = "\"GROSSWEIGHT\":" + "\"" + weight.Remove(0, 1).ToString() + "\"," +
                      "\"GROSSWEIGHTUOM\":" + "\"" + weightUOM.Remove(0, 1).Substring(0, 2).ToString() + "\",";
                }
            }

            return finalMEA;

        }

        public string TMP(String[] TMPelement)
        {

            int checkElementTMP = 0;
            string checkTMPType = "";
            string TMP = "";
            string TMPUOM = "";
            string finalTMP = "";

            for (int checkTMP = 0; checkTMP < TMPelement.Count(); checkTMP++)
            {
                for (int checkTMPCo = 0; checkTMPCo < TMPelement[0].Count() - 1; checkTMPCo++)
                {
                    if (TMPelement[checkTMP][checkTMPCo].ToString() == "+" || TMPelement[checkTMP][checkTMPCo].ToString() == ":")
                    {
                        checkElementTMP++;
                    }

                    if (checkElementTMP == 1)
                    {
                        checkTMPType += TMPelement[checkTMP][checkTMPCo].ToString();
                    }

                    if (checkElementTMP == 2)
                    {
                        TMP += TMPelement[checkTMP][checkTMPCo].ToString();
                    }

                    if (checkElementTMP == 3)
                    {
                        TMPUOM += TMPelement[checkTMP][checkTMPCo].ToString();
                    }
                }
            }

            if (checkTMPType.Remove(0, 1).ToString() == "2")
            {

                finalTMP = "\"Temp\":" + "\"" + TMP.Remove(0, 1).ToString() + "\"," +
                  "\"TempUOM\":" + "\"" + TMPUOM.Remove(0, 1).Substring(0, 1).ToString() + "\",";

            }

            return finalTMP;

        }

        public string DGS(String[] DGSelement)
        {
            int checkElementDGS = 0;
            string checkDGSType = "";
            string classDGS = "";
            string undg = "";
            string finalDGS = "";

            for (int checkDGS = 0; checkDGS < DGSelement.Count(); checkDGS++)
            {
                for (int checkDGSCo = 0; checkDGSCo < DGSelement[0].Count() - 1; checkDGSCo++)
                {

                    if (DGSelement[checkDGS][checkDGSCo].ToString() == "+")
                    {
                        checkElementDGS++;
                    }

                    if (checkElementDGS == 1)
                    {
                        checkDGSType += DGSelement[checkDGS][checkDGSCo].ToString();
                    }

                    if (checkElementDGS == 2)
                    {
                        classDGS += DGSelement[checkDGS][checkDGSCo].ToString();
                    }

                    if (checkElementDGS == 3)
                    {
                        undg += DGSelement[checkDGS][checkDGSCo].ToString();
                    }

                }

            }

            if (checkDGSType.Remove(0, 1).ToString() == "IMD")
            {

                finalDGS = "\"DG\":" + "\"" + classDGS.Remove(0, 1) + "(" + undg.Remove(0, 1) + ")\",";

            }

            return finalDGS;

        }

        public string DIM(String[] DIMelement)
        {
            int checkElementDIM = 0;
            string checkDIMType = "";
            string sizeDIM = "";
            string detailDIMH = "";
            string detailDIMW = "";
            string detailDIML = "";
            string finalDIM = "";

            for (int checkDIM = 0; checkDIM < DIMelement.Count(); checkDIM++)
            {
                for (int checkDIMCo = 0; checkDIMCo < DIMelement[0].Count() - 1; checkDIMCo++)
                {

                    if (DIMelement[checkDIM][checkDIMCo].ToString() == "+" || DIMelement[checkDIM][checkDIMCo].ToString() == ":")
                    {
                        checkElementDIM++;
                    }
                    if (checkElementDIM == 1)
                    {
                        checkDIMType += DIMelement[checkDIM][checkDIMCo].ToString();
                    }

                    if (checkElementDIM == 2)
                    {

                        sizeDIM += DIMelement[checkDIM][checkDIMCo].ToString();

                    }

                    if (checkElementDIM == 3)
                    {
                        // Lenght dimention

                        detailDIML += DIMelement[checkDIM][checkDIMCo].ToString();

                    }
                    if (checkElementDIM == 4)
                    {
                        // width dimention
                        detailDIMW += DIMelement[checkDIM][checkDIMCo].ToString();

                    }
                    if (checkElementDIM == 5)
                    {
                        // hight dimention
                        detailDIMH += DIMelement[checkDIM][checkDIMCo].ToString();
                    }
                }
            }

            if (checkDIMType.Remove(0, 1).ToString() == "9" || checkDIMType.Remove(0, 1).ToString() == "13")
            {
                finalDIM = "\"OOG HIGHT\":" + "\"" + "OVER HIGHT " + detailDIMH.Remove(0, 1).ToString() + " " + sizeDIM.Remove(0, 1).Substring(0, 2) + "\",";

            }

            if (checkDIMType.Remove(0, 1).ToString() == "8")
            {
                finalDIM = "\"OOG LEFT\":" + "\"" + "OW LEFT " + detailDIMW.Remove(0, 1).ToString() + " " + sizeDIM.Remove(0, 1).Substring(0, 2) + "\",";

            }

            if (checkDIMType.Remove(0, 1).ToString() == "7")
            {
                finalDIM = "\"OOG RIGHT\":" + "\"" + "OW RIGHT " + detailDIMW.Remove(0, 1).ToString() + " " + sizeDIM.Remove(0, 1).Substring(0, 2) + "\",";

            }

            if (checkDIMType.Remove(0, 1).ToString() == "5")
            {
                finalDIM = "\"OOG FRONT\":" + "\"" + "OL FRONT " + detailDIML.Remove(0, 1).ToString() + " " + sizeDIM.Remove(0, 1).Substring(0, 2) + "\",";

            }

            if (checkDIMType.Remove(0, 1).ToString() == "6")
            {
                finalDIM = "\"OOG REAR\":" + "\"" + "OR REAR " + detailDIML.Remove(0, 1).ToString() + " " + sizeDIM.Remove(0, 1).Substring(0, 2) + "\",";

            }

            return finalDIM;

        }

    }
}