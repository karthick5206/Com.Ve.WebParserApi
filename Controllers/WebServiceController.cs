using Com.Ve.ServerDataReceiver.RavenDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
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

        private void WriteRequest()
        {
            Log($"Method:{Request.Method} Path:{Request.Path}" +
                $" QueryString:{Request.QueryString} Method:{Request.Method}" +
                $" ContentType:{Request.ContentType} {Request.ContentLength}", LogType.Info);
        }

        [HttpPost("ChatResponse")]
        public XElement ChatResponse([FromForm] string Imei, [FromForm] string Reply)
        {
            try
            {
                WriteRequest();
                Log("Chat Response", LogType.Info);
                Log("Received Message IMEI : " + Imei, LogType.Info);
                Log("Received Message Reply : " + Reply, LogType.Info);
            }
            catch (Exception ex)
            {
                Log(ex.StackTrace, LogType.Error);
            }
            string XML = "<string xmlns=\"http://tempuri.org/\">SUCCESS0</string>";
            HttpContext.Response.ContentType = "application/xml";
            return XDocument.Parse(XML).Root;
        }

        [HttpPost("GetCommands")]

        public XElement GetCommands([FromForm] string Imei)
        {
            try
            {
                WriteRequest();
                Log("GetCommands", LogType.Info);
                Log("Received Message IMEI : " + Imei, LogType.Info);
            }
            catch (Exception ex)
            {
                Log(ex.StackTrace, LogType.Error);
            }
            string XML = "<string xmlns=\"http://tempuri.org/\"></string>";
            HttpContext.Response.ContentType = "application/xml";
            return XDocument.Parse(XML).Root;
        }
        [HttpPost("FirmwareUpdated")]
        private XElement FirmwareUpdated([FromForm] string Imei, [FromForm] string FirmwareId)
        {
            try
            {
                WriteRequest();
                Log("FirmwareUpdated", LogType.Info);
                Log("Received Message IMEI : " + Imei, LogType.Info);
                Log("Received Message FirmwareId : " + FirmwareId, LogType.Info);
            }
            catch (Exception ex)
            {
                Log(ex.StackTrace, LogType.Error);
            }
            string XML = "<string xmlns=\"http://tempuri.org/\">SUCCESS0</string>";
            HttpContext.Response.ContentType = "application/xml";
            return XDocument.Parse(XML).Root;
        }

        [HttpPost("DeviceStatus")]
        public XElement DeviceStatus([FromForm] string IMEI, [FromForm] string GpsStatus)
        {
            try
            {
                WriteRequest();
                Log("DeviceStatus", LogType.Info);
                Log("Received Message IMEI : " + IMEI, LogType.Info);
                Log("Received Message GpsStatus : " + GpsStatus, LogType.Info);
            }
            catch (Exception ex)
            {
                Log(ex.StackTrace, LogType.Error);
            }
            string XML = "<string xmlns=\"http://tempuri.org/\">SUCCESS0</string>";
            HttpContext.Response.ContentType = "application/xml";
            return XDocument.Parse(XML).Root;
        }

        [HttpPost("RawTripLog")]
        public XElement RawTripLog([FromForm] string Imei, [FromForm] string TripLogData)
        {
            try
            {
                WriteRequest();
                Log("RawTripLog", LogType.Info);
                Log("Received Message IMEI : " + Imei,
                    LogType.Info);
                Log("Received Message TripLogData : " + TripLogData, LogType.Info);
                var receivedMessage = Imei + "/" + TripLogData;
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
            string XML = "<string xmlns=\"http://tempuri.org/\">SUCCESS</string>";
            HttpContext.Response.ContentType = "application/xml";
            return XDocument.Parse(XML).Root;
        }
    }

    [XmlRoot(ElementName = "string", Namespace = "http://tempuri.org/")]
    public class XmlOutput
    {
        private XmlSerializerNamespaces _namespaces;

        public XmlOutput()
        {
            this._namespaces = new XmlSerializerNamespaces(new XmlQualifiedName[] {
            // Don't do this!! Microsoft's documentation explicitly says it's not supported.
            // It doesn't throw any exceptions, but in my testing, it didn't always work.

            // new XmlQualifiedName(string.Empty, string.Empty),  // And don't do this:
            // new XmlQualifiedName("", "")

            // DO THIS:
            new XmlQualifiedName(string.Empty, "urn:Abracadabra") // Default Namespace
            // Add any other namespaces, with prefixes, here.
        });
        }
    }
}
