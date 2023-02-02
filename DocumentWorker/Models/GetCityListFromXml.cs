using System.Collections.ObjectModel;
using System.Linq;
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
       
        /// <summary>
        /// Получает список всех городов из xml файла
        /// </summary>
        /// <param name="CityListAll">Список городов</param>
        public void GetCityList(ObservableCollection<string>? CityListAll)
        {
            var xDoc = _getXDocument.GetDoc();

            var roots = xDoc.Element("Settings")?.
                Elements("City").Select(p => p.Attribute("CityName")?.Value);

            if (roots is not null)
            {
                foreach (var root in roots)
                    CityListAll?.Add(root);

            }
        }
    }
}
