﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TallyConnector.Models
{
    [XmlRoot(ElementName = "COSTCATEGORY")]
    public class CostCategory:TallyXmlJson
    {
        [XmlElement(ElementName = "MASTERID")]
        public int? TallyId { get; set; }

        [XmlAttribute(AttributeName = "NAME")]
        public string Name { get; set; }

        [XmlIgnore]
        public string VName { get; set; }

        [XmlElement(ElementName = "ALLOCATEREVENUE")]
        public string AllocateRevenue { get; set; }

        [XmlElement(ElementName = "ALLOCATENONREVENUE")]
        public string AllocateNonRevenue { get; set; }

        [XmlElement(ElementName = "GUID")]
        public string GUID { get; set; }

        [XmlIgnore]
        public string Alias
        {
            get
            {
                if (this.LanguageNameList.NameList.NAMES.Count > 0)
                {
                    if (VName == null)
                    {
                        VName = this.LanguageNameList.NameList.NAMES[0];
                    }
                    if (Name == VName)
                    {
                        this.LanguageNameList.NameList.NAMES[0] = this.Name;
                        return string.Join("..\n", this.LanguageNameList.NameList.NAMES.GetRange(1, this.LanguageNameList.NameList.NAMES.Count - 1));

                    }
                    else
                    {
                        //Name = this.LanguageNameList.NameList.NAMES[0];
                        return string.Join("..\n", this.LanguageNameList.NameList.NAMES);

                    }
                }
                else
                {
                    this.LanguageNameList.NameList.NAMES.Add(this.Name);
                    return null;
                }


            }
            set
            {
                this.LanguageNameList = new LanguageNameList();
                
                if (value != null)
                {
                    List<string> lis = value.Split("..\n".ToCharArray()).ToList();

                    LanguageNameList.NameList.NAMES.Add(Name);
                    if (value != "")
                    {
                        LanguageNameList.NameList.NAMES.AddRange(lis);
                    }

                }
                else
                {
                    LanguageNameList.NameList.NAMES.Add(Name);
                }


            }
        }

        [JsonIgnore]
        [XmlElement(ElementName = "LANGUAGENAME.LIST")]
        public LanguageNameList LanguageNameList { get; set; }
        /// <summary>
        /// Accepted Values //Create, Alter, Delete
        /// </summary>
        [JsonIgnore]
        [XmlAttribute(AttributeName = "Action")]
        public string Action { get; set; }
    }
    [XmlRoot(ElementName = "ENVELOPE")]
    public class CostCatEnvelope : TallyXmlJson
    {

        [XmlElement(ElementName = "HEADER")]
        public Header Header { get; set; }

        [XmlElement(ElementName = "BODY")]
        public CCBody Body { get; set; } = new CCBody();
    }

    [XmlRoot(ElementName = "BODY")]
    public class CCBody
    {
        [XmlElement(ElementName = "DESC")]
        public Description Desc { get; set; } = new Description();

        [XmlElement(ElementName = "DATA")]
        public CCData Data { get; set; } = new CCData();
    }

    [XmlRoot(ElementName = "DATA")]
    public class CCData
    {
        [XmlElement(ElementName = "TALLYMESSAGE")]
        public CCMessage Message { get; set; } = new CCMessage();

        [XmlElement(ElementName = "COLLECTION")]
        public CostCategoryColl Collection { get; set; } = new CostCategoryColl();


    }

    [XmlRoot(ElementName = "COLLECTION")]
    public class CostCategoryColl
    {
        [XmlElement(ElementName = "COSTCATEGORY")]
        public List<CostCategory> CostCategories { get; set; }
    }

    [XmlRoot(ElementName = "TALLYMESSAGE")]
    public class CCMessage
    {
        [XmlElement(ElementName = "COSTCATEGORY")]
        public CostCategory CostCategory { get; set; }
    }

}
