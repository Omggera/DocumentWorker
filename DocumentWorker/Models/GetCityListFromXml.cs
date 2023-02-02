using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Resources;
using System.Xml;
using System.Xml.Linq;
using DocumentWorker.Interfaces;

namespace DocumentWorker.Models
{
    public class GetCityListFromXml : IGetCityListFromXml
    {
        IGetXDocument _getXDocument;
        public GetCityListFromXml(IGetXDocument getXDocument)
        {
            _getXDocument = getXDocument;
        }
       // public ObservableCollection<string>? CityListAll { get; set; }
        public void GetCityList(ObservableCollection<string>? CityListAll)
        {
            var xDoc = _getXDocument.GetDoc();

            //CityListAll = new ObservableCollection<string>();
            var roots = xDoc.Element("Settings")?.
                Elements("City").Select(p => p.Attribute("CityName")?.Value);

            if (roots is not null)
            {
                foreach (var root in roots)
                    CityListAll?.Add(root);

            }

            //CityListAll.Add("Список пуст");

        }
    }
}
