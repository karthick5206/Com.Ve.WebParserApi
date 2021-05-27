using Com.Ve.ServerDataReceiver.RavenDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Com.Ve.WebParserApi.Controllers
{
    [Route("api/WebService.asmx")]
    [ApiController]
    public class WebServiceController : ControllerBase
    {
        private void Log(string log, LogType logType)
        {
            RavenDbConnector.Add(new LogData { Log = log, LogType = logType });
        }

        [HttpGet("ChatResponse")]
        public string ChatResponse(String IMEI, String Reply)
        {
            try
            {
                Log("Chat Response", LogType.Info);
                Log("Received Message IMEI : " + IMEI, LogType.Info);
                Log("Received Message Reply : " + Reply, LogType.Info);
            }
            catch (Exception ex)
            {
                Log(ex.StackTrace, LogType.Error);
            }
            string XML = "SUCCESS0";
            HttpContext.Response.ContentType = "application/xml";
            return XML;
        }

        [HttpGet("GetCommands")]

        public string GetCommands(string IMEI)
        {
            try
            {
                Log("GetCommands", LogType.Info);
                Log("Received Message IMEI : " + IMEI, LogType.Info);
            }
            catch (Exception ex)
            {
                Log(ex.StackTrace, LogType.Error);
            }
            string XML = " ";
            HttpContext.Response.ContentType = "application/xml";
            return XML;
        }
        [HttpGet("FirmwareUpdated")]
        private string FirmwareUpdated(String IMEI, String FirmwareId)
        {
            try
            {
                Log("FirmwareUpdated", LogType.Info);
                Log("Received Message IMEI : " + IMEI, LogType.Info);
                Log("Received Message FirmwareId : " + FirmwareId, LogType.Info);
            }
            catch (Exception ex)
            {
                Log(ex.StackTrace, LogType.Error);
            }
            string XML = "SUCCESS0";
            HttpContext.Response.ContentType = "application/xml";
            return XML;
        }

        [HttpGet("DeviceStatus")]
        public string DeviceStatus(String IMEI, String GpsStatus)
        {
            try
            {
                Log("DeviceStatus", LogType.Info);
                Log("Received Message IMEI : " + IMEI, LogType.Info);
                Log("Received Message GpsStatus : " + GpsStatus, LogType.Info);
            }
            catch (Exception ex)
            {
                Log(ex.StackTrace, LogType.Error);
            }
            string XML = "SUCCESS0";
            HttpContext.Response.ContentType = "application/xml";
            return XML;
        }

        [HttpGet("RawTripLog")]
        public string RawTripLog(String IMEI, String TripLogData)
        {
            try
            {
                Log("RawTripLog", LogType.Info);
                Log("Received Message IMEI : " + IMEI,
                    LogType.Info);
                Log("Received Message TripLogData : " + TripLogData, LogType.Info);
                var receivedMessage = IMEI + "/" + TripLogData;
                if (!string.IsNullOrEmpty(receivedMessage) && receivedMessage.Length > 1)
                {
                    Log("Received Message : " + receivedMessage, LogType.Info);
                    var request = (HttpWebRequest)WebRequest.Create("http://localhost:4580/Service.ashx/RawTripLog");

                    //LogFile.Info("Address : " + request.Address);

                    var data = Encoding.ASCII.GetBytes(receivedMessage);

                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = data.Length;

                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                Log(ex.StackTrace, LogType.Error);
            }
            string XML = "SUCCESS";
            HttpContext.Response.ContentType = "application/xml";
            return XML;
        }
    }
}
