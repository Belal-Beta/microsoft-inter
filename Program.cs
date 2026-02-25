using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
namespace contact_Manager
{
    class Program
    {
        static void Main(string[] args)
        {
            
            clsUser currentUser = new clsUser("C:\\Users\\belal\\source\\repos\\contact indexing service\\contacts.txt");
            currentUser.start();
        }
    }

    class clsUser
    {
        private readonly string _ContactPath;
        private const string SEPERATOR = "#//#";
        class clsContact : IComparable<clsContact>
        {
            private static int IDSetter = 1;
            private readonly int _id;
            private string _name;
            private string _email;
            private string _phoneNumber;
            private readonly DateTime _creationDate;
            public int ID { get { return this._id; } }
            public string Name { get { return _name; } set { _name = value; } }
            public string Email { get { return _email; } set { _email = value; } }
            public string PhoneNumber { get { return _phoneNumber; } set { _phoneNumber = value; } }
            public DateTime CreationDate { get { return this._creationDate; } }
            public clsContact(string name, string email, string phoneNumber, int defaultID = -1, DateTime defaultDateTime = new DateTime())
            {
                this._id = (defaultID != -1 ? defaultID : IDSetter);
                IDSetter++;
                this.Name = name;
                this.Email = email;
                while (!long.TryParse(phoneNumber, out long temp))
                {
                    phoneNumber = Console.ReadLine();
                }
                this.PhoneNumber = phoneNumber;
                this._creationDate = (defaultDateTime != DateTime.MinValue ? defaultDateTime : DateTime.Now);
            }
            public int CompareTo(clsContact? other)
            {
                if (other == null) return 1;
                return PhoneNumber.CompareTo(other.PhoneNumber);
            }
            public override string ToString()
            {
                return $"Name: {Name}\tPhone Number: {PhoneNumber}\tEmail: {Email}";
            }
            public void ViewContactDetails()
            {
                Console.WriteLine("===Contact Details===");
                Console.WriteLine($"ID: {ID}");
                Console.WriteLine($"Name: {Name}");
                Console.WriteLine($"Phone Number: {PhoneNumber}");
                Console.WriteLine($"Email: {Email}");
                Console.WriteLine($"Created: {CreationDate.ToString()}");
                Console.WriteLine("=====================");
            }
        }
        private SortedSet<clsContact> _contacts;
        private delegate bool Filter(clsContact contact);
        private  void ViewContactsByName()
        {
            string name = Console.ReadLine();
            ListAllContactsWithCondition(c => c.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        }
        private  void ViewContactsByEmail()
        {
            string email = Console.ReadLine();

            ListAllContactsWithCondition(c => c.Email.Contains(email, StringComparison.OrdinalIgnoreCase));
        }
        private  void ViewContactsByPhoneNumber()
        {
            string phoneNumber = Console.ReadLine();
            ListAllContactsWithCondition(c => c.PhoneNumber.Contains(phoneNumber));
        }
        public clsUser(string ContactsPont)

        {
            this._contacts = new SortedSet<clsContact>();
            this._ContactPath = ContactsPont;
            if (File.Exists(_ContactPath))
            {
                string[] lines = File.ReadAllLines(this._ContactPath);
                foreach (string line in lines)
                {
                    var contactInfo = line.Split(SEPERATOR);
                    this._contacts.Add(new clsContact(contactInfo[1], contactInfo[2], contactInfo[3], int.Parse(contactInfo[0]), DateTime.Parse(contactInfo[4])));
                    //name#//#email#//#phonenumber#//#ID#//#Date
                }
            }
            else
            {
                File.Create(_ContactPath).Close();
            }
        }
        private  clsContact FindContact(clsContact contact)
        {
            if (this._contacts.TryGetValue(contact, out clsContact found))
            {
                return found;
            }
            else
            {
                Console.WriteLine("This value does not exist already!");
                return null;
            }
        }
        private  clsContact GetContact()
        {
            string name, email, phoneNumber;
            Console.Write("please enter the name of the new contact: ");
            name = Console.ReadLine();
            Console.Write("please enter the phone number of the new contact: ");
            phoneNumber = Console.ReadLine();
            Console.Write("please enter the email of the new contact: ");
            email = Console.ReadLine();
            return new clsContact(name, email, phoneNumber);
        }
        private  void ListAllContactsWithCondition(Filter filter)
        {
            foreach (var contact in _contacts)
            {
                if (filter(contact))
                {
                    Console.WriteLine(contact.ToString());
                }
            }
        }
        private void AddNewContact()
        {
            var contact = this.GetContact();
            this._contacts.Add(contact);
        }
        private void EditContact()
        {
            var TargetContact = this.GetContact();
            TargetContact = this.FindContact(TargetContact);
            if (TargetContact != null)
            {
                var NewContactInfo = this.GetContact();
                TargetContact = NewContactInfo;
            }
        }
        private void DeleteContact()
        {
            var TargetContact = this.GetContact();
            TargetContact = this.FindContact(TargetContact);
            if (TargetContact != null)
            {
                this._contacts.Remove(TargetContact);
            }
        }
        private void SearchContact()
        {
            var searched = this.GetContact();
            searched = this.FindContact(searched);
            if (searched != null)
            {
                Console.WriteLine(searched.ToString());
            }
        }
        private void ViewContact()
        {
            var TargetContact = this.GetContact();
            TargetContact = this.FindContact(TargetContact);
            if (TargetContact != null)
            {
                TargetContact.ViewContactDetails();
            }
        }
        private void ViewAllContacts()
        {
            ListAllContactsWithCondition(c => true);
        }
        private void filterContacts()
        {
            Console.Write("1-filter by name.");
            Console.Write("2-filter by email.");
            Console.Write("3-filter by phone number.");
            int choise = 5;
            bool isValid = false;
            while (!isValid || choise > 3 || choise < 1)
            {
                Console.Write("please enter the number of your choise: ");
                string input = Console.ReadLine();
                isValid = int.TryParse(input, out choise);
                if (!isValid)
                {
                    Console.WriteLine("Invalid input! Please enter a valid integer.");
                }
            }
            switch (choise)
            {
                case 1:
                    ViewContactsByName();
                    break;
                case 2:
                    ViewContactsByEmail();
                    break;
                case 3:
                    ViewContactsByPhoneNumber();
                    break;
                default:
                    break;
            }
        }
        private void printMainMinue()
        {
            Console.WriteLine("===Main Minue===");
            Console.WriteLine("1)add new contact");
            Console.WriteLine("2)edit contact");
            Console.WriteLine("3)delete contact");
            Console.WriteLine("4)view contact");
            Console.WriteLine("5)list all contact");
            Console.WriteLine("6)search contact");
            Console.WriteLine("7)filter contact");
            Console.WriteLine("8)save contact");
            Console.WriteLine("9)Exit contact");
            Console.WriteLine("================");
        }
        private void save()
        {
            foreach (var c in _contacts)
            {
                string line=c.Name+SEPERATOR+c.Email+ SEPERATOR + c.PhoneNumber+ SEPERATOR + c.ID+ SEPERATOR + c.CreationDate;
                File.AppendAllText(_ContactPath, line);
            }
        }
        public void start()
        {
            do
            {
                printMainMinue();
                int choise = 10;
                bool isValid = false;
                while (!isValid || choise > 9 || choise < 1)
                {
                    Console.Write("please enter the number of your choise: ");
                    string input = Console.ReadLine();
                    isValid = int.TryParse(input, out choise);
                    if (!isValid)
                    {
                        Console.WriteLine("Invalid input! Please enter a valid integer.");
                    }
                }
                switch (choise)
                {
                    case 1:
                        this.AddNewContact();
                        break;
                    case 2:
                        this.EditContact();
                        break;
                    case 3:
                        this.DeleteContact();
                        break;
                    case 4:
                        this.ViewContact();
                        break;
                    case 5:
                        this.ViewAllContacts();
                        break;
                    case 6:
                        this.SearchContact();
                        break;
                    case 7:
                        this.filterContacts();
                        break;
                    case 8:
                        this.save();
                        break;
                    case 9:
                        Console.Write("do you want to save changes [y/n]: ");
                        char chiose =Convert.ToChar(Console.ReadKey());
                        if (chiose == 'y')
                        {
                            this.save();
                        }
                        return;
                        break;
                    default:
                        break;
                }
            }
            while (true);
        }
    }
}