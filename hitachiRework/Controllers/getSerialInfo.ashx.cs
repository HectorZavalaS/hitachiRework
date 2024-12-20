using hitachiRework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hitachiRework.Controllers
{
    /// <summary>
    /// Summary description for getSerialInfo
    /// </summary>
    public class getSerialInfo : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            String json = "{";
            siixsem_aoi_koh_youngEntities m_db = new siixsem_aoi_koh_youngEntities();
            //DataTable models = new DataTable();
            String html = "", htmlHead="", htmlBody = "";
            String serial = context.Request.Form["serial"];
            try
            {
                List<getCmpListBySerial_Result> defectList = m_db.getCmpListBySerial(serial).ToList();
                isReworkable_Result defectResult = m_db.isReworkable(serial).First();
                if (!defectResult.RESULT_CODE.Equals("NOT_FOUND"))
                {

                    html += "<div id='tableInfoDet'><table id='tableSerialsDet' class='table display nowrap' style='font-size:11px;'>";
                    htmlHead = "<thead class='tablehead'>";
                    htmlHead += "<tr>";
                    htmlHead += "<th>SERIAL</th>";
                    htmlHead += "<th>PROGRAM</th>";
                    htmlHead += "<th>CRD</th>";
                    htmlHead += "<th>PART NAME</th>";
                    htmlHead += "<th>PACKAGE TYPE</th>";
                    htmlHead += "<th>ALLOW REWORK</th>";
                    htmlHead += "</tr>";
                    htmlHead += "</thead>";

                    htmlBody += "<tbody>";
                    int i = 1;
                    foreach (getCmpListBySerial_Result defect in defectList)
                    {
                        htmlBody += "<tr class='tablerows'>";

                        htmlBody += "<td style='font-weight:bold;'>" + serial + "</td>";
                        htmlBody += "<td style='font-weight:bold;'>" + defect.PROGRAM + "</td>";
                        htmlBody += "<td style='font-weight:bold;'>" + defect.REF + "</td>";
                        htmlBody += "<td style='font-weight:bold;'>" + defect.PART_NAME + "</td>";
                        htmlBody += "<td style='font-weight:bold;'>" + defect.TYPE + "</td>";
                        if(defect.ALLOW_REWORK == 0)
                            htmlBody += "<td style='font-weight:bold;'>" + "NG" + "</td>";
                        else
                            htmlBody += "<td style='font-weight:bold;'>" + "OK" + "</td>";
                        htmlBody += "</tr>";
                        i++;
                    }
                    htmlBody += "</tbody>";

                    html += htmlHead + htmlBody + "</table></div>";

                    if (defectResult.RESULT_CODE.Equals("OK"))
                    {
                        html += "<div style='font-weight:bold; color:green; font-size:35px;'>PCB RETRABAJABLE<div>";
                    }
                    else
                    {
                        html += "<div style='font-weight:bold; color:red; font-size:35px;'>PCB NO RETRABAJABLE<div>";
                    }

                    json += "\"result\":\"true\",";
                    json += "\"html\":\"" + html + "\"";
                }
                else
                {
                    html += "<div style='font-weight:bold; color:red; font-size:35px;'>" + defectResult.RESULT + "<div>";
                    json += "\"result\":\"false\",";
                    json += "\"MessageError\":\"" + html + "\"";
                }

            }
            catch (Exception ex)
            {
                json += "\"result\":\"false\",";
                json += "\"MessageError\":\"" + ex.Message + "\"";
            }
            json += "}";
            context.Response.ContentType = "text/plain";
            context.Response.Write(json);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}