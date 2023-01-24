using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using DocumentWorker.Interfaces;

namespace DocumentWorker.Models
{
    public class GetCityListFromXml : IGetCityListFromXml
    {
        public List<string>? CityList { get; set; }
        public List<string> GetCityList()
        {
            var uri = new Uri("pack://application:,,,/SettingsXml/Settings.xml");
            var resourceStream = Application.GetResourceStream(uri).Stream;

            XmlDocument doc = new XmlDocument();
            doc.Load(resourceStream);

            CityList = new List<string>();
            XmlElement? root = doc.DocumentElement;
            XmlNodeList? items = root?.SelectNodes("City");
            if (items is not null)
            {
                foreach (XmlNode item in items)
                    CityList?.Add(item.SelectSingleNode("@CityName")?.Value);

                return CityList;
            }

            CityList.Add("Список пуст");

            return CityList;
        }
    }
}
