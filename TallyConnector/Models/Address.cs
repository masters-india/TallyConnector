﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace TallyConnector.Models
{

    
    [Serializable]
    [XmlRoot(ElementName = "ADDRESS.LIST")]
    public class HAddress
    {
        private List<string> _Address = new List<string>();

        
        [XmlElement(ElementName = "ADDRESS")]
        public List<string> Address
        {
            get { return _Address; }
            set { _Address = value; }
        }
        [JsonIgnore]
        [XmlIgnore]
        public string FullAddress
        {
            get { return _Address.Count>0?string.Join(" ..\n", _Address):null; }
            set { _Address = value!=null? value.Split(" ..\n".ToCharArray()).ToList():new List<string>(); }
        }

    }


}
