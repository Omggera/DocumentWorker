using DocumentWorker.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DocumentWorker.Models
{
    public class DeleteCity : IDeleteCity
    {
        private IGetXDocument _xDocument;

        public DeleteCity(IGetXDocument getXDocument)
        {
            _xDocument = getXDocument;
        }

        public void Delete(string? selectedItem)
        {
            var xDoc = _xDocument.GetDoc();
            if (selectedItem != null)
            {
                using (StreamWriter stream = new StreamWriter($"{AppDomain.CurrentDomain.BaseDirectory}Settings\\Settings.xml"))
                {
                    XElement? root = xDoc.Element("Settings");

                    var city = root.Elements("City").FirstOrDefault(p => p.Attribute("CityName")?.Value == selectedItem);

                    if (city != null)
                    {
                        city.Remove();

                        xDoc.Save(stream);
                    }
                }
            }
        }
    }
}
