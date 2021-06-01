using Com.Ve.ServerDataReceiver.RavenDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Text;

namespace Com.Ve.WebParserApi.Controllers
{
    [Route("api/FMFFZService.asmx")]
    [ApiController]
    public class FM450Service : ControllerBase
    {
        private const string DeviceName = "FM450";
        private const string ReplySuccess = "<string xmlns=\"http://tempuri.org/\">\"SUCCESS\"</string>";
        private const string ReplyEmptySuccess = "<string xmlns=\"http://tempuri.org/\">\"\"</string>";
        private const string ReplySuccess0 = "<string xmlns=\"http://tempuri.org/\">\"SUCCESS0\"</string>";
        private const string ContentType = "text/xml; charset=utf-8";

        private void Log(string log, LogType logType) =>
            RavenDbConnector.Add(new LogData { Log = $"Device:{DeviceName} - {log}", LogType = logType });

        private void WriteRequest()
        {
            Log($"Method:{Request.Method} Path:{Request.Path}" +
                $" QueryString:{Request.QueryString} Method:{Request.Method}" +
                $" ContentType:{Request.ContentType} {Request.ContentLength}", LogType.Info);

            foreach(var header in Request.Headers)
                Log($"Header - Key:{header.Key} Value:{header.Value.ToString()}", LogType.Info);
        }

        [HttpPost("ChatResponse")]
        public IActionResult ChatResponse([FromForm] string Imei, [FromForm] string Reply)
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
            HttpContext.Response.ContentType = ContentType;
            return new ContentResult
            {
                Content = ReplySuccess0,
                ContentType = ContentType,
                StatusCode = (int)HttpStatusCode.OK,
            };           
        }

        [HttpGet("GetChatResponse")]
        public IActionResult GetChatResponse(string Imei, string Reply)
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
            HttpContext.Response.ContentType = ContentType;
            return new ContentResult
            {
                Content = ReplySuccess0,
                ContentType = ContentType,
                StatusCode = (int)HttpStatusCode.OK,
            };
        }

        [HttpPost("GetCommands")]

        public IActionResult GetCommands([FromForm] string Imei)
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
            HttpContext.Response.ContentType = ContentType;
            return new ContentResult
            {
                Content = ReplySuccess,
                ContentType = ContentType,
                StatusCode = (int)HttpStatusCode.OK,
            };
        }
        [HttpPost("FirmwareUpdated")]
        private IActionResult FirmwareUpdated([FromForm] string Imei, [FromForm] string FirmwareId)
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
            HttpContext.Response.ContentType = ContentType;
            return new ContentResult
            {
                Content = ReplyEmptySuccess,
                ContentType = ContentType,
                StatusCode = (int)HttpStatusCode.OK,
            };
        }

        [HttpPost("DeviceStatus")]
        public IActionResult DeviceStatus([FromForm] string IMEI, [FromForm] string GpsStatus)
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
            HttpContext.Response.ContentType = ContentType;
            return new ContentResult
            {
                Content = ReplySuccess0,
                ContentType = ContentType,
                StatusCode = (int)HttpStatusCode.OK,
            };
        }

        [HttpPost("RawTripLog")]
        public IActionResult RawTripLog([FromForm] string Imei, [FromForm] string TripLogData)
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
                    var request = (HttpWebRequest)WebRequest.Create("http://app.vibhaventerprise.com/fms/WebService.asmx/RawTripLog");

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
            HttpContext.Response.ContentType = ContentType;
            return new ContentResult
            {
                Content = ReplySuccess,
                ContentType = ContentType,
                StatusCode = (int)HttpStatusCode.OK,
            };
        }
    }
}
