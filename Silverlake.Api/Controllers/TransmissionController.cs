using Silverlake.Service;
using Silverlake.Service.IService;
using Silverlake.Utility;
using Silverlake.Utility.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Xml;

namespace Silverlake.Api.Controllers
{
    public class TransmissionController : ApiController
    {
        private static readonly Lazy<ISetService> lazySetServiceObj = new Lazy<ISetService>(() => new SetService());

        public static ISetService ISetService { get { return lazySetServiceObj.Value; } }

        // GET api/values
        public IEnumerable<Set> Get()
        {
            return ISetService.GetData(0, 0, true);
        }

        //public void PostRawXMLMessage(HttpRequestMessage request)
        //{
        //    var xmlDoc = new XmlDocument();
        //    xmlDoc.Load(request.Content.ReadAsStreamAsync().Result);
        //    StringWriter sw = new StringWriter();
        //    XmlTextWriter xw = new XmlTextWriter(sw);
        //    xmlDoc.WriteTo(xw);
        //    //String XmlString = sw.ToString();
        //    //byte[] bytes = Encoding.UTF8.GetBytes(XmlString);

        //    //ISampleService.PostBlobData(new Sample()
        //    //{
        //    //    XmlString = bytes,
        //    //    CreatedDate = DateTime.Now
        //    //});

        //    string savePath = ConfigurationManager.AppSettings["XMLSavePath"].ToString();
        //    xmlDoc.Save(savePath + "sample" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xml");
        //}

        public void PostFile(HttpRequestMessage request)
        {
            Stream stream = request.Content.ReadAsStreamAsync().Result;
            byte[] fileBytes = new byte[stream.Length];
            stream.Read(fileBytes, 0, fileBytes.Length);
            stream.Close();

            string mimeType = request.Content.Headers.ContentType.MediaType;

            string fileExtension = Extension.GetDefaultExtension(mimeType);

            string savePath = ConfigurationManager.AppSettings["XMLSavePath"].ToString();
            string filePath = savePath + "sample" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileExtension;
            using (Stream file = File.OpenWrite(filePath))
            {
                file.Write(fileBytes, 0, fileBytes.Length);
            }
        }
    }
}
