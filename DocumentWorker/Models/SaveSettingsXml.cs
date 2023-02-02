using System;
using System.IO;
using System.Linq;
using DocumentWorker.Interfaces;

namespace DocumentWorker.Models
{
    public class SaveSettingsXml : ISaveSettingsXml
    {
        private IGetXDocument _getXDocument;
        public SaveSettingsXml(IGetXDocument getXDocument)
        {
            _getXDocument = getXDocument;
        }

        /// <summary>
        /// Сохраняет настройки после изменения данных
        /// выбранного города
        /// </summary>
        /// <param name="citySet">Выбранный город</param>
        /// <param name="legalEntityNew">Юридическое лицо</param>
        /// <param name="legalNameNew">Название организации на латинице</param>
        /// <param name="phoneNumberSalesDepartmentNew">Телефон отдела продаж</param>
        /// <param name="phoneNumberDeliveryServiceNew">Телефон службы доставки</param>
        /// <param name="SellersRepresentativeNew">Представитель продавца</param>
        public void SaveSettings(
            string citySet, 
            string legalEntityNew,
            string legalNameNew,
            string phoneNumberSalesDepartmentNew,
            string phoneNumberDeliveryServiceNew,
            string SellersRepresentativeNew)
        {
            var xDoc = _getXDocument.GetDoc();

            using (StreamWriter stream = new StreamWriter($"{AppDomain.CurrentDomain.BaseDirectory}Settings\\Settings.xml"))
            {
                //Получаем данные в xml по названию города
                var data = xDoc.Element("Settings")?
                    .Elements("City")
                    .FirstOrDefault(p => p.Attribute("CityName")?.Value == $"{citySet}");

                //Если этот город есть, то вносим изменения в xml файл
                if (data != null)
                {
                    var legalEntityOld = data.Element("LegalEntity");
                    if (legalEntityOld != null) legalEntityOld.Value = legalEntityNew;

                    var legalNameOld = data.Element("LegalName");
                    if (legalNameOld != null) legalNameOld.Value = legalNameNew;

                    var phoneNumberSalesDepartmentOld = data.Element("PhoneNumberSalesDepartment");
                    if (phoneNumberSalesDepartmentOld != null) phoneNumberSalesDepartmentOld.Value = phoneNumberSalesDepartmentNew;

                    var phoneNumberDeliveryServiceOld = data.Element("PhoneNumberDeliveryService");
                    if (phoneNumberDeliveryServiceOld != null) phoneNumberDeliveryServiceOld.Value = phoneNumberDeliveryServiceNew;

                    var SellersRepresentativeOld = data.Element("SellersRepresentative");
                    if (SellersRepresentativeOld != null) SellersRepresentativeOld.Value = SellersRepresentativeNew;

                    xDoc.Save(stream);
                }
            }
        }
    }
}
