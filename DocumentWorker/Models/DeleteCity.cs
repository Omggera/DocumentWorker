using DocumentWorker.Interfaces;
using System;
using System.IO;
using System.Linq;
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

        /// <summary>
        /// Удаляет выбранный город
        /// </summary>
        /// <param name="selectedItem">Город выбранный в списке</param>
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
