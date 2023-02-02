using DocumentWorker.Interfaces;
using System;
using System.IO;
using System.Xml.Linq;

namespace DocumentWorker.Models
{
    public class AddNewCity : IAddNewCity
    {
        private IGetXDocument _xDocument;
        public AddNewCity(IGetXDocument getXDocument)
        {
            _xDocument = getXDocument;
        }

        /// <summary>
        /// Добавляет новый город в файл xml
        /// </summary>
        /// <param name="cityName">Название города</param>
        /// <param name="legalEntity">Юридическое лицо</param>
        /// <param name="legalName">Название организации на латинице</param>
        /// <param name="phoneNumberSalesDepartment">Телефон отдела продаж</param>
        /// <param name="phoneNumberDeliveryService">Телефон службы доставки</param>
        /// <param name="sellersRepresentative">Представитель продавца</param>
        public void Add(
            string cityName,
            string legalEntity,
            string legalName,
            string phoneNumberSalesDepartment,
            string phoneNumberDeliveryService,
            string sellersRepresentative)
        {
            var xDoc = _xDocument.GetDoc();

            using (StreamWriter stream = new StreamWriter($"{AppDomain.CurrentDomain.BaseDirectory}Settings\\Settings.xml"))
            {
                XElement? root = xDoc.Element("Settings");

                if (root != null)
                {
                    root.Add(new XElement("City",
                                new XAttribute("CityName", $"{cityName}"),
                                new XElement("LegalEntity", $"{legalEntity}"),
                                new XElement("LegalName", $"{legalName}"),
                                new XElement("PhoneNumberSalesDepartment", $"{phoneNumberSalesDepartment}"),
                                new XElement("PhoneNumberDeliveryService", $"{phoneNumberDeliveryService}"),
                                new XElement("SellersRepresentative", $"{sellersRepresentative}")
                                ));

                    xDoc.Save(stream);
                }
            }



        }
    }
}
