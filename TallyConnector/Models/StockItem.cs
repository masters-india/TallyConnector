﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TallyConnector.Models
{
    [XmlRoot(ElementName = "STOCKITEM")]
    public class StockItem:TallyXmlJson
    {
        [XmlElement(ElementName = "MASTERID")]
        public int TallyId { get; set; }

        [XmlAttribute(AttributeName = "NAME")]
        public string Name { get; set; }

        [XmlIgnore]
        public string VName { get; set; }

        [XmlElement(ElementName = "PARENT")]
        public string StockGroup { get; set; }

        [XmlElement(ElementName = "CATEGORY")]
        public string Category { get; set; }

        [XmlElement(ElementName = "BASEUNITS")]
        public string BaseUnit { get; set; }

        [XmlElement(ElementName = "OPENINGBALANCE")]
        public string OpeningBal { get; set; }

        [XmlElement(ElementName = "OPENINGVALUE")]
        public string OpeningValue { get; set; }

        [XmlElement(ElementName = "OPENINGRATE")]
        public string OpeningRate { get; set; }



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
                this.LanguageNameList = new();
                
                if (value != null)
                {
                    List<string> lis = value.Split("..\n").ToList();

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

        [XmlElement(ElementName = "GUID")]
        public string GUID { get; set; }
    }
    [XmlRoot(ElementName = "ENVELOPE")]
    public class StockItemEnvelope : TallyXmlJson
    {

        [XmlElement(ElementName = "HEADER")]
        public Header Header { get; set; }

        [XmlElement(ElementName = "BODY")]
        public SIBody Body { get; set; } = new();
    }

    [XmlRoot(ElementName = "BODY")]
    public class SIBody
    {
        [XmlElement(ElementName = "DESC")]
        public Description Desc { get; set; } = new();

        [XmlElement(ElementName = "DATA")]
        public SIData Data { get; set; } = new();
    }

    [XmlRoot(ElementName = "DATA")]
    public class SIData
    {
        [XmlElement(ElementName = "TALLYMESSAGE")]
        public SIMessage Message { get; set; } = new();

        [XmlElement(ElementName = "COLLECTION")]
        public StockItemColl Collection { get; set; } = new StockItemColl();


    }

    [XmlRoot(ElementName = "COLLECTION")]
    public class StockItemColl
    {
        [XmlElement(ElementName = "STOCKITEM")]
        public List<StockItem> StockItems { get; set; }
    }

    [XmlRoot(ElementName = "TALLYMESSAGE")]
    public class SIMessage
    {
        [XmlElement(ElementName = "STOCKITEM")]
        public StockItem StockItem { get; set; }
    }
}
