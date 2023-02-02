using System.Linq;
using DocumentWorker.Interfaces;

namespace DocumentWorker.Models
{
    public class GetXmlData : IGetXmlData
    {
        IGetXDocument _getXDocument;
        public GetXmlData(IGetXDocument getXDocument)
        {
            _getXDocument = getXDocument;
        }

        public City? City { get; set; }

        /// <summary>
        /// Поулчает все данные из xml файла по выбранному городу
        /// </summary>
        /// <param name="citySet">Выбранный город</param>
        /// <returns> Класс City</returns>
        public City GetData(string citySet)
        {
            var data = _getXDocument.GetDoc().Element("Settings")?
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
