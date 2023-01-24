using DocumentWorker.Models;

namespace DocumentWorker.Interfaces
{
    public interface IGetXmlData
    {
        public City? City { get; set; }
        public City GetData(string citySet);
    }
}