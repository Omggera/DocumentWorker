using System.Collections.ObjectModel;

namespace DocumentWorker.Interfaces
{
    public interface IGetCityListFromXml
    {
        void GetCityList(ObservableCollection<string>? CityListAll);
    }
}