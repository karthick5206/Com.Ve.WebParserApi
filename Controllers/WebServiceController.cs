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

        [HttpPost("ChatResponse")]
        public string ChatResponse(string Imei, string Reply)
        {
            try
            {
                Log("Chat Response", LogType.Info);
                Log("Received Message IMEI : " + Imei, LogType.Info);
                Log("Received Message Reply : " + Reply, LogType.Info);
            }
            catch (Exception ex)
            {
                Log(ex.StackTrace, LogType.Error);
            }
            string XML = "\"SUCCESS0\"";
            HttpContext.Response.ContentType = "application/xml";
            return XML;
        }

        [HttpPost("GetCommands")]

        public string GetCommands(string Imei)
        {
            try
            {
                Log("GetCommands", LogType.Info);
                Log("Received Message IMEI : " + Imei, LogType.Info);
            }
            catch (Exception ex)
            {
                Log(ex.StackTrace, LogType.Error);
            }
            string XML = "\"\"";
            HttpContext.Response.ContentType = "application/xml";
            return XML;
        }
        [HttpPost("FirmwareUpdated")]
        private string FirmwareUpdated(string Imei, string FirmwareId)
        {
            try
            {
                Log("FirmwareUpdated", LogType.Info);
                Log("Received Message IMEI : " + Imei, LogType.Info);
                Log("Received Message FirmwareId : " + FirmwareId, LogType.Info);
            }
            catch (Exception ex)
            {
                Log(ex.StackTrace, LogType.Error);
            }
            string XML = "\"SUCCESS0\"";
            HttpContext.Response.ContentType = "application/xml";
            return XML;
        }

        [HttpPost("DeviceStatus")]
        public string DeviceStatus(string IMEI, string GpsStatus)
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
            string XML = "\"SUCCESS0\"";
            HttpContext.Response.ContentType = "application/xml";
            return XML;
        }

        [HttpPost("RawTripLog")]
        public string RawTripLog([FromBody] TripLogData tripLogData)
        {
            try
            {
                Log("RawTripLog", LogType.Info);
                Log("Received Message IMEI : " + tripLogData.Imei,
                    LogType.Info);
                Log("Received Message TripLogData : " + tripLogData.TripLogData, LogType.Info);
                var receivedMessage = tripLogData.Imei + "/" + tripLogData.TripLogData;
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
            string XML = "\"SUCCESS\"";
            HttpContext.Response.ContentType = "application/xml";
            return XML;
        }
    }   

    public class TripLogData
    {
        public string Imei { get; set; }
        public string TripLogData { get; set; }
    }
}
