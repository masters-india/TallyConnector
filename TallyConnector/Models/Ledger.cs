﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace TallyConnector.Models
{
    [Serializable]
    [XmlRoot("LEDGER")]
    public class Ledger : TallyXmlJson
    {
        public Ledger()
        {
            FAddress = new HAddress();
            InterestList = new InterestList();
        }

        [XmlElement(ElementName = "MASTERID")]
        public int? TallyId { get; set; }

        [XmlAttribute(AttributeName = "NAME")]
        [JsonIgnore]
        public string OldName { get; set; }

        private string name;
        [XmlElement(ElementName = "NAME")]
        [Required]
        public string Name
        {
            get { return (name == null || name == string.Empty) ? OldName : name; }
            set => name = value;
        }

        [XmlIgnore]
        public string VName { get; set; }

        [XmlElement(ElementName = "PARENT")]
        [Required]
        public string Group { get; set; }

        [XmlIgnore]
        public string Alias
        {
            get
            {
                if (this.LanguageNameList.NameList.NAMES.Count > 0)
                {
                    if (VName ==null)
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
               
                if (value!=null)
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

        [XmlElement(ElementName = "ISDEEMEDPOSITIVE")]
        public string DeemedPositive { get; set; }
        [XmlIgnore]
        public string ForexAmount { get; set; }
        [XmlIgnore]
        public string RateofExchange { get; set; }

        

        private string _OpeningBal;

        [XmlElement(ElementName = "OPENINGBALANCE")]
        public string OpeningBal
        {
            get
            {
                if (ForexAmount != null && RateofExchange != null)
                {
                    _OpeningBal = $"{ForexAmount} @ {RateofExchange}";
                }
                else if (ForexAmount != null)
                {
                    _OpeningBal = ForexAmount;
                }
                return _OpeningBal;
            }
            set
            {
                if (value!=null)
                {
                    double t_opbal;
                    Match CleanedAmount;
                    if (value.ToString().Contains('='))
                    {
                        
                        List<string> SplittedValues = value.ToString().Split('=').ToList();
                        CleanedAmount = Regex.Match(SplittedValues[1], @"[0-9.]+");
                        bool Isnegative = SplittedValues[1].Contains('-');
                        bool sucess = Isnegative ? double.TryParse('-'+CleanedAmount.Value, out t_opbal) : double.TryParse(CleanedAmount.ToString(),out t_opbal);
                        CleanedOpeningBal = t_opbal;
                        var ForexInfo = SplittedValues[0].Split('@');
                        ForexAmount = ForexInfo[0].Trim();
                        RateofExchange = Regex.Match(ForexInfo[1], @"[0-9.]+").Value;
                    }
                    else
                    {
                        double.TryParse(value, out t_opbal);
                        CleanedOpeningBal = t_opbal;
                        _OpeningBal = value;
                    }
                }
                else
                {
                    _OpeningBal = value;
                }
                

            }
        }
        [XmlIgnore]
        public double CleanedOpeningBal { get; set; }

        [XmlIgnore]
        public string ClosingForexAmount { get; set; }
        [XmlIgnore]
        public string ClosingRateofExchange { get; set; }
        [XmlIgnore]
        public double CleanedClosingBal { get; set; }

        private string _ClosingBal;
         
        [XmlElement(ElementName = "CLOSINGBALANCE")]
        public string ClosingBal
        {
            get
            {
                if (ClosingForexAmount != null && ClosingRateofExchange != null)
                {
                    _OpeningBal = $"{ClosingForexAmount} @ {ClosingRateofExchange}";
                }
                else if (ClosingForexAmount != null)
                {
                    _ClosingBal = ClosingForexAmount;
                }
                return _ClosingBal;
            }
            set
            {
                if (value != null)
                {
                    double t_opbal;
                    if (value.ToString().Contains('='))
                    {
                        List<string> SplittedValues = value.ToString().Split('=').ToList();
                        var CleanedAmount = Regex.Match(SplittedValues[1], @"[0-9.]+");
                        bool Isnegative = SplittedValues[1].Contains('-');
                        bool sucess = Isnegative ? double.TryParse('-' + CleanedAmount.Value, out t_opbal) : double.TryParse(CleanedAmount.ToString(), out t_opbal);
                        CleanedClosingBal = t_opbal;
                        var ForexInfo = SplittedValues[0].Split('@');
                        ClosingForexAmount = ForexInfo[0].Trim();
                        ClosingRateofExchange = Regex.Match(ForexInfo[1], @"[0-9.]+").Value;
                    }
                    else
                    {
                        double.TryParse(value, out t_opbal);
                        CleanedClosingBal = t_opbal;
                        _ClosingBal = value;
                    }
                }
                else
                {
                    _ClosingBal = value;
                }
            }
            
        }

        
        private string _Currency;
        [XmlElement(ElementName = "CURRENCYNAME")]
        public string Currency
        {
            get { return _Currency; }
            set
            {
                if (value=="?")
                {
                    _Currency = null;
                }
                else
                {
                    _Currency = value;
                }
                 }
        }

        [XmlElement(ElementName = "TAXTYPE")]
        public string TaxType { get; set; }

        [XmlElement(ElementName = "GSTDUTYHEAD")]
        public string GSTTaxType { get; set; }

        [XmlElement(ElementName = "RATEOFTAXCALCULATION")]
        public string RateofTax { get; set; }

        [XmlElement(ElementName = "ISBILLWISEON")]
        public string IsBillwise { get; set; }

        [XmlElement(ElementName = "BILLCREDITPERIOD")]
        public string CreditPeriod { get; set; }

        [XmlElement(ElementName = "ISCREDITDAYSCHKON")]
        public string IsCreditCheck { get; set; }


        [XmlElement(ElementName = "CREDITLIMIT")]
        public string CreditLimit { get; set; }


        [XmlElement(ElementName = "MAILINGNAME")]
        public string MailingName { get; set; }


        
        [XmlIgnore]
        public string Address
        {
            get
            {
                return FAddress.FullAddress;
            }

            set
            {
                if (value !="")
                {
                    
                    this.FAddress.FullAddress = value;
                }
                

            }

        }

        [XmlElement(ElementName = "COUNTRYNAME")]
        public string Country { get; set; }

        [XmlElement(ElementName = "LEDSTATENAME")]
        public string State { get; set; }

        [XmlElement(ElementName = "PINCODE")]
        public string PinCode { get; set; }

        [XmlElement(ElementName = "LEDGERCONTACT")]
        public string ContactPerson { get; set; }

        [XmlElement(ElementName = "LEDGERPHONE")]
        public string LandlineNo { get; set; }

        [XmlElement(ElementName = "LEDGERMOBILE")]
        public string MobileNo { get; set; }

        [XmlElement(ElementName = "LEDGERFAX")]
        public string FaxNo { get; set; }

        [XmlElement(ElementName = "EMAIL")]
        public string Email { get; set; }

        [XmlElement(ElementName = "EMAILCC")]
        public string Emailcc { get; set; }

        [XmlElement(ElementName = "WEBSITE")]
        public string Website { get; set; }

        [XmlElement(ElementName = "INCOMETAXNUMBER")]
        public string PANNumber { get; set; }

        [XmlElement(ElementName = "GSTREGISTRATIONTYPE")]
        public string GSTRegistrationType { get; set; }

        [XmlElement(ElementName = "ISOTHTERRITORYASSESSEE")]
        public string IsOtherTerritoryAssessee { get; set; }

        [XmlElement(ElementName = "PARTYGSTIN")]
        public string GSTIN { get; set; }

        [XmlElement(ElementName = "ISECOMMOPERATOR")]
        public string IsECommerceOperator { get; set; }

        [XmlElement(ElementName = "CONSIDERPURCHASEFOREXPORT")]
        public string DeemedExport { get; set; }

        [XmlElement(ElementName = "GSTNATUREOFSUPPLY")]
        public string GSTPartyType { get; set; }

        [XmlElement(ElementName = "ISTRANSPORTER")]
        public string IsTransporter { get; set; }

        [XmlElement(ElementName = "TRANSPORTERID")]
        public string TransporterID { get; set; }


        [XmlElement(ElementName = "AFFECTSSTOCK")]
        public string AffectStock { get; set; }

        [XmlElement(ElementName = "ISCOSTCENTRESON")]
        public string IsCostcenter { get; set; }

        [XmlElement(ElementName = "ISREVENUE")]
        public string Isrevenue { get; set; }

        [XmlElement(ElementName = "ISINTERESTON")]
        public string Isintereston { get; set; }


        private InterestList _InterestList;

        
        [JsonIgnore]
        [XmlElement(ElementName = "INTERESTCOLLECTION.LIST")]
        public InterestList InterestList { 
            get 
            {
                _InterestList.FromDate = Interestfromdate;
                _InterestList.ToDate = Interesttodate;
                _InterestList.InterestStyle = InterestStyle;
                _InterestList.InterestBalancetype = InterestBalancetype;
                _InterestList.Interestappliedon = Interestappliedon;
                _InterestList.Interestfromtype = Interestfromtype;
                _InterestList.RoundType = InterestRoundType;
                _InterestList.InterestRate = InterestRate;
                _InterestList.Interestappliedfrom = Interestappliedfrom;
                _InterestList.RoundLimit = InterestRoundLimit;

                
                

                return _InterestList; 
            } 
            set 
            {
                Interestfromdate = value.FromDate;
                Interesttodate = value.ToDate;
                InterestStyle = value.InterestStyle;
                InterestBalancetype = value.InterestBalancetype;
                Interestappliedon = value.Interestappliedon;
                Interestfromtype = value.Interestfromtype;
                InterestRoundType = value.RoundType;
                InterestRate = value.InterestRate;
                Interestappliedfrom = value.Interestappliedfrom;
                InterestRoundLimit = value.RoundLimit;


                _InterestList = value;
            } }

        [XmlIgnore]
        public string Interestfromdate { get; set; }

        [XmlIgnore]
        public string Interesttodate { get; set; }

        [XmlIgnore]
        public string InterestStyle { get; set; }

        [XmlIgnore]
        public string InterestBalancetype { get; set; }

        [XmlIgnore]
        public string Interestappliedon { get; set; }

        [XmlIgnore]
        public string Interestfromtype { get; set; }

        [XmlIgnore]
        public string InterestRoundType { get; set; }

        [XmlIgnore]
        public string InterestRate { get; set; }

        [XmlIgnore]
        public string Interestappliedfrom { get; set; }

        [XmlIgnore]
        public string InterestRoundLimit { get; set; }



        [XmlElement(ElementName = "FORPAYROLL")]
        public string Forpayroll { get; set; }
        
        
        [XmlElement(ElementName = "DESCRIPTION")]
        public string Description { get; set; }

        [XmlElement(ElementName = "NARRATION")]
        public string Notes { get; set; }

        [JsonIgnore]
        [XmlElement(ElementName = "ADDRESS.LIST")]
        public HAddress FAddress { get; set; }


        [JsonIgnore]
        [XmlElement(ElementName = "LANGUAGENAME.LIST")]
        public LanguageNameList LanguageNameList { get; set; }

        [XmlElement(ElementName = "CANDELETE")]
        public string CanDelete { get; set; }

        /// <summary>
        /// Accepted Values //Create, Alter, Delete
        /// </summary>
        [JsonIgnore]
        [XmlAttribute(AttributeName = "Action")]
        public string Action { get; set; }

        [XmlElement(ElementName = "GUID")]
        public string GUID { get; set; }
    }

    [XmlRoot(ElementName = "INTERESTCOLLECTION.LIST")]
    public class InterestList
    {
        [XmlElement(ElementName = "INTERESTFROMDATE")]
        public string FromDate { get; set; }

        [XmlElement(ElementName = "INTERESTTODATE")]
        public string ToDate { get; set; }

        [XmlElement(ElementName = "INTERESTSTYLE")]
        public string InterestStyle { get; set; }

        [XmlElement(ElementName = "INTERESTBALANCETYPE")]
        public string InterestBalancetype { get; set; }

        [XmlElement(ElementName = "INTERESTAPPLON")]
        public string Interestappliedon { get; set; }

        [XmlElement(ElementName = "INTERESTFROMTYPE")]
        public string Interestfromtype { get; set; }

        [XmlElement(ElementName = "ROUNDTYPE")]
        public string RoundType { get; set; }

        [XmlElement(ElementName = "INTERESTRATE")]
        public string InterestRate { get; set; }

        [XmlElement(ElementName = "INTERESTAPPLFROM")]
        public string Interestappliedfrom { get; set; }

        [XmlElement(ElementName = "ROUNDLIMIT")]
        public string RoundLimit { get; set; }

    }

    [XmlRoot(ElementName = "ENVELOPE")]
    public class LedgerEnvelope: TallyXmlJson
    {

        [XmlElement(ElementName = "HEADER")]
        public Header Header { get; set; }

        [XmlElement(ElementName = "BODY")]
        public LBody Body { get; set; } = new LBody();
    }

    [XmlRoot(ElementName = "BODY")]
    public class LBody
    {
        [XmlElement(ElementName = "DESC")]
        public Description Desc { get; set; } = new Description();

        [XmlElement(ElementName = "DATA")]
        public LData Data { get; set; } = new LData();
    }

    [XmlRoot(ElementName = "DATA")]
    public class LData
    {
        [XmlElement(ElementName = "TALLYMESSAGE")]
        public LedgerMessage Message { get; set; } = new LedgerMessage();
        
        [XmlElement(ElementName = "COLLECTION")]
        public LedgColl Collection { get; set; } = new LedgColl();
        

    }

    [XmlRoot(ElementName = "COLLECTION")]
    public class LedgColl
    {
        [XmlElement(ElementName = "LEDGER")]
        public List<Ledger> Ledgers { get; set; }
    }
    [XmlRoot(ElementName = "TALLYMESSAGE")]
    public class LedgerMessage
    {
        [XmlElement(ElementName = "LEDGER")]
        public Ledger Ledger { get; set; }
    }





}
