namespace DocumentWorker.Interfaces
{
    public interface ISaveSettingsXml
    {
        void SaveSettings(
            string citySet,
            string legalEntityNew,
            string legalNameNew,
            string phoneNumberSalesDepartmentNew,
            string phoneNumberDeliveryServiceNew,
            string SellersRepresentativeNew);
    }
}