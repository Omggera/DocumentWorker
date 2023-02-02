using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DocumentWorker.Interfaces
{
    public interface IGetCityListFromXml
    {
        //ObservableCollection<string>? CityListAll { get; set; }

        void GetCityList(ObservableCollection<string>? CityListAll);
    }
}