using Silverlake.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;
using System.Xml.Serialization;

namespace Silverlake.Api.Controllers
{
    public class XMLFileTransmissionController : ApiController
    {
        public void PostFile(HttpRequestMessage request)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(request.Content.ReadAsStreamAsync().Result);
            MemoryStream xmlStream = new MemoryStream();
            xmlDoc.Save(xmlStream);
            xmlStream.Flush();//Adjust this if you want read your data 
            xmlStream.Position = 0;

            XmlSerializer deserializer = new XmlSerializer(typeof(Sample));
            TextReader textReader = new StreamReader(xmlStream);
            Sample movies;
            movies = (Sample)deserializer.Deserialize(textReader);
            textReader.Close();
        }
    }
}
