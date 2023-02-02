using System;
using System.IO;
using System.Xml.Linq;
using DocumentWorker.Interfaces;

namespace DocumentWorker.Models
{
    public class GetXDocument : IGetXDocument
    {
        /// <summary>
        /// Реализует класс StreamReader, получает xml документ и закрыает поток
        /// </summary>
        /// <returns>XDocument</returns>
        public XDocument GetDoc()
        {
            XDocument xdoc = new XDocument();
            using (StreamReader stream = new StreamReader($"{AppDomain.CurrentDomain.BaseDirectory}Settings\\Settings.xml"))
            {
                xdoc = XDocument.Load(stream);
            }
            return xdoc;
        }
    }
}
