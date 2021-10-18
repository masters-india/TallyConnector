using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TallyConnector.Models
{
    [XmlRoot(ElementName = "COSTCENTRE")]
    public class EmployeeGroup:CostCenter
    {
        [XmlElement(ElementName = "FORPAYROLL")]
        public string ForPayroll { get; set; }

        [XmlElement(ElementName = "ISEMPLOYEEGROUP")]
        public string IsEmployeeGroup { get; set; }


    }

    [XmlRoot(ElementName = "ENVELOPE")]
    public class EmployeeGroupEnvelope : TallyXmlJson
    {

        [XmlElement(ElementName = "HEADER")]
        public Header Header { get; set; }

        [XmlElement(ElementName = "BODY")]
        public EmployeeGroupBody Body { get; set; } = new EmployeeGroupBody();
    }

    [XmlRoot(ElementName = "BODY")]
    public class EmployeeGroupBody
    {
        [XmlElement(ElementName = "DESC")]
        public Description Desc { get; set; } = new Description();

        [XmlElement(ElementName = "DATA")]
        public EmployeeGroupData Data { get; set; } = new EmployeeGroupData();
    }

    [XmlRoot(ElementName = "DATA")]
    public class EmployeeGroupData
    {
        [XmlElement(ElementName = "TALLYMESSAGE")]
        public EmployeeGroupMessage Message { get; set; } = new EmployeeGroupMessage();


        [XmlElement(ElementName = "COLLECTION")]
        public EmployeeGroupColl Collection { get; set; } = new EmployeeGroupColl();


    }

    [XmlRoot(ElementName = "COLLECTION")]
    public class EmployeeGroupColl
    {
        [XmlElement(ElementName = "COSTCENTRE")]
        public List<EmployeeGroup> EmployeeGroups { get; set; }
    }

    [XmlRoot(ElementName = "TALLYMESSAGE")]
    public class EmployeeGroupMessage
    {
        [XmlElement(ElementName = "COSTCENTRE")]
        public EmployeeGroup EmployeeGroup { get; set; }
    }
}
