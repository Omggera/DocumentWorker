using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentWorker.Models
{
    public class City
    {
        public string CityName { get; set; }
        //Юридическое лицо
        public string LegalEntity { get; set; }
        //Название юр.лица
        public string LegalName { get; set; }
        //Телефон отдела продаж
        public string PhoneNumberSalesDepartment { get; set; }
        //Телефон службы доставки
        public string PhoneNumberDeliveryService { get; set; }
        //Представитель Продавца
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
