using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Serie_IV
{
    public class PhoneBook
    {
        private Dictionary<string,string> phonebook = new Dictionary<string,string>();
        private bool IsValidPhoneNumber(string phoneNumber)
        {
            if (!(phoneNumber.Length == 10))
            {
                return false;
            }
            if (!(phoneNumber[0]=='0' && phoneNumber[1]!= '0'))
            {
                return false;
            }
            return true;
        }

        public bool ContainsPhoneContact(string phoneNumber)
        {
            try
            {
                string a = phonebook[phoneNumber];
            }
            catch (Exception)
            {
                //Console.WriteLine($"Phone Number : {phoneNumber} not in PhoneBook !");
                return false;
            }
            return true;
        }

        public void PhoneContact(string phoneNumber)
        {
            if (ContainsPhoneContact(phoneNumber))
            {
                Console.WriteLine($"{phoneNumber} : {phonebook[phoneNumber]}");
            }
        }

        public bool AddPhoneNumber(string phoneNumber, string name)
        {
            if (name == "")
            {
                return false;
            }
            if (!ContainsPhoneContact(phoneNumber))
            {
                phonebook.Add(phoneNumber, name);
                return true;
            }
            return false;
        }

        public bool DeletePhoneNumber(string phoneNumber)
        {
            if (ContainsPhoneContact(phoneNumber))
            {
                phonebook.Remove(phoneNumber);
                return true;
            }
            return false;
        }

        public void DisplayPhoneBook()
        {
            if (phonebook.Count == 0)
            {
                Console.WriteLine("Pas de numéros téléphoniques");
                return;
            }
            Console.WriteLine("Annuaire téléphonique :");
            Console.WriteLine("-----------------------");
            foreach (string phonenumber in phonebook.Keys)
            {
                PhoneContact(phonenumber);
            }
            Console.WriteLine("-----------------------");
            return;
        }
    }
}
