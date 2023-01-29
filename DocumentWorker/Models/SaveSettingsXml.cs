using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;
using DocumentWorker.Interfaces;
using Microsoft.Win32.SafeHandles;

namespace DocumentWorker.Models
{
    public class SaveSettingsXml : ISaveSettingsXml
    {
        private IGetXDocument _getXDocument;
        public SaveSettingsXml(IGetXDocument getXDocument)
        {
            _getXDocument = getXDocument;
        }

        public void SaveSettings(
            string citySet, 
            string legalEntityNew,
            string legalNameNew,
            string phoneNumberSalesDepartmentNew,
            string phoneNumberDeliveryServiceNew,
            string SellersRepresentativeNew)
        {
            var xDoc = _getXDocument.GetDoc();

            using (StreamWriter stream = new StreamWriter($"{AppDomain.CurrentDomain.BaseDirectory}SettingsXml\\Settings.xml"))
            {
                var data = xDoc.Element("Settings")?
                    .Elements("City")
                    .FirstOrDefault(p => p.Attribute("CityName")?.Value == $"{citySet}");

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
