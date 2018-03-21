using MyAPI.Modules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyAPI.Repositorys
{
    public interface ICustomersRepository
    {
        void Add(CustomersModule item);
        IEnumerable<CustomersModule> GetAll();
        CustomersModule Find(string key);
        void Remove(string Id);
        void Update(CustomersModule item);

        bool CheckValidUserKey(string reqkey);
    }

    public class CustomersRepository : ICustomersRepository
    {
        static List<CustomersModule> _customers = new List<CustomersModule>();

        void ICustomersRepository.Add(CustomersModule item)
        {
            _customers.Add(item);
        }

        public bool CheckValidUserKey(string reqkey)
        {
            var userKeys = new List<string>();
            userKeys.Add("123abc987plm");
            userKeys.Add("098def567okn");
            userKeys.Add("387bgt054edc");
            userKeys.Add("327qwe000swx");

            if (userKeys.Contains(reqkey))
                return true;
            else
                return false;
        }

        CustomersModule ICustomersRepository.Find(string key)
        {
            return _customers
                .Where(e => e.MobilePhone.Equals(key))
                .SingleOrDefault();
        }

        IEnumerable<CustomersModule> ICustomersRepository.GetAll()
        {
            _customers.Add(new CustomersModule()
            {
                ID = Guid.NewGuid().ToString(),
                Name = "Rico",
                MobilePhone = "12345"
            });
            return _customers;
        }

        void ICustomersRepository.Remove(string Id)
        {
            var itemToRemove = _customers.SingleOrDefault(r => r.MobilePhone == Id);
            if (itemToRemove != null)
                _customers.Remove(itemToRemove);
        }

        void ICustomersRepository.Update(CustomersModule item)
        {
            var itemToUpdate = _customers.SingleOrDefault(r => r.MobilePhone == item.MobilePhone);
            if (itemToUpdate != null)
            {
                itemToUpdate.ID = Guid.NewGuid().ToString();
                itemToUpdate.Name = item.Name;
                itemToUpdate.Email = item.Email;
                itemToUpdate.MobilePhone = item.MobilePhone;
            }
        }
    }
}
