using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Silverlake.Utility
{
    [XmlRoot(ElementName = "M-FILES_IMPORTER_ERROR")]
    public class MFILES_IMPORTER_ERROR
    {
        [XmlAttribute(AttributeName = "timestamp")]
        public string Timestamp { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "Document")]
    public class MFileDocument
    {
        [XmlElement(ElementName = "SourceFile")]
        public string SourceFile { get; set; }
        [XmlElement(ElementName = "ScanDate")]
        public string ScanDate { get; set; }
        [XmlElement(ElementName = "IndexDate")]
        public string IndexDate { get; set; }
        [XmlElement(ElementName = "QCDate")]
        public string QCDate { get; set; }
        [XmlElement(ElementName = "StationID")]
        public string StationID { get; set; }
        [XmlElement(ElementName = "DocumentSetID")]
        public string DocumentSetID { get; set; }
        [XmlElement(ElementName = "DocumentID")]
        public string DocumentID { get; set; }
        [XmlElement(ElementName = "PageNumber")]
        public string PageNumber { get; set; }
        [XmlElement(ElementName = "Verified")]
        public string Verified { get; set; }
        [XmlElement(ElementName = "ScanUser")]
        public string ScanUser { get; set; }
        [XmlElement(ElementName = "IndexUser")]
        public string IndexUser { get; set; }
        [XmlElement(ElementName = "QCUser")]
        public string QCUser { get; set; }
        [XmlElement(ElementName = "BatchID")]
        public string BatchID { get; set; }
        [XmlElement(ElementName = "DocumentCode")]
        public string DocumentCode { get; set; }
        [XmlElement(ElementName = "DocumentTypeName")]
        public string DocumentTypeName { get; set; }
        [XmlElement(ElementName = "AANumber")]
        public string AANumber { get; set; }
        [XmlElement(ElementName = "AccountNumber")]
        public string AccountNumber { get; set; }
        [XmlElement(ElementName = "Class")]
        public string Class { get; set; }
        [XmlElement(ElementName = "M-FILES_IMPORTER_ERROR")]
        public MFILES_IMPORTER_ERROR MFILES_IMPORTER_ERROR { get; set; }
    }

    [XmlRoot(ElementName = "Documents")]
    public class MFileDocuments
    {
        [XmlElement(ElementName = "Document")]
        public List<MFileDocument> Document { get; set; }
    }
}
