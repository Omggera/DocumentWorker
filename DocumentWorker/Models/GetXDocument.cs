using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Resources;
using System.Xml.Linq;
using DocumentWorker.Interfaces;

namespace DocumentWorker.Models
{
    public class GetXDocument : IGetXDocument
    {
        public XDocument GetDoc()
        {
            XDocument xdoc = new XDocument();
            using (StreamReader stream = new StreamReader($"{AppDomain.CurrentDomain.BaseDirectory}SettingsXml\\Settings.xml"))
            {
                xdoc = XDocument.Load(stream);
            }
            return xdoc;
        }
    }
}
