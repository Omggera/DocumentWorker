using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using DocumentWorker.Interfaces;

namespace DocumentWorker.Models
{
    public class GetXmlData : IGetXmlData
    {
        public City? City { get; set; }
        public City GetData(string citySet)
        {

            var uri = new Uri("pack://application:,,,/SettingsXml/Settings.xml");
            var resourceStream = Application.GetResourceStream(uri).Stream;

            XDocument xdoc = XDocument.Load(resourceStream);

            var data = xdoc.Element("Settings")?
                .Elements("City")
                .FirstOrDefault(p => p.Attribute("CityName")?.Value == $"{citySet}");

            return City = new(
                    data?.Attribute("CityName")?.Value,
                    data?.Element("LegalEntity")?.Value,
                    data?.Element("LegalName")?.Value,
                    data?.Element("PhoneNumberSalesDepartment")?.Value,
                    data?.Element("PhoneNumberDeliveryService")?.Value,
                    data?.Element("SellersRepresentative")?.Value
                    );
        }
    }
}
