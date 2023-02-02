namespace DocumentWorker.Interfaces
{
    public interface IAddNewCity
    {
        void Add(string cityName, string legalEntity, string legalName, string phoneNumberSalesDepartment, string phoneNumberDeliveryService, string sellersRepresentative);
    }
}