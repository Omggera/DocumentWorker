using System.Collections.Generic;

namespace DocumentWorker.Interfaces
{
    public interface IGetCityListFromXml
    {
        List<string>? CityList { get; set; }

        List<string> GetCityList();
    }
}