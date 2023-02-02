
namespace DocumentWorker.Models
{
    public class City
    {
        /// <summary> Название города </summary>
        public string CityName { get; set; }

        /// <summary> Юридическое лицо </summary>
        public string LegalEntity { get; set; }

        /// <summary> Название организации на латинице </summary>
        public string LegalName { get; set; }

        /// <summary> Телефон отдела продаж </summary>//
        public string PhoneNumberSalesDepartment { get; set; }

        /// <summary> Телефон службы доставки </summary>
        public string PhoneNumberDeliveryService { get; set; }

        /// <summary> Представитель продавца </summary>
        public string SellersRepresentative { get; set; }

        public City(
            string cityName, 
            string legalEntity, 
            string legalName, 
            string phoneNumberSalesDepartment, 
            string phoneNumberDeliveryService, 
            string sellersRepresentative)
        {
            CityName = cityName;
            LegalEntity = legalEntity;
            LegalName = legalName;
            PhoneNumberSalesDepartment = phoneNumberSalesDepartment;
            PhoneNumberDeliveryService = phoneNumberDeliveryService;
            SellersRepresentative = sellersRepresentative;
        }
    }
}
