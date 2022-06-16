using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Picc_d.o.o
{
    class Program
    {
        static void Main(string[] args)
        {
            MainMenu();
        }
        public static void MainMenu()
        {
            while (true)
            {
                Console.WriteLine("Please choose one of the following options:" +
                  " 1 - Add," +
                  " 2 - Search," +
                  " 3 - Update," +
                  " 4 - Delete," +
                  " 5 - Exit\n");

                var choice = Console.ReadLine();
                choice = choice.Trim();

                if (choice == "1")
                {
                    AddData();
                    continue;
                }
                else if (choice == "2")
                {
                    SearchData();
                    continue;
                }
                else if (choice == "3")
                {
                    UpdateData();
                    continue;
                }
                else if (choice == "4")
                {
                    DeleteData();
                    continue;
                }
                else if (choice == "5") break;
                else
                {
                    Console.WriteLine("Invalid input");
                    continue;
                }
            }
        }

        public static void AddData()
        {

            Console.WriteLine("ADDING NEW ITEM\n");
            Console.WriteLine("Mandatory fields are: Type, Make and Model, Mileage and Year of Manufacture\n.");

            Console.WriteLine("Enter vehicle type: ");
            var typeInput = Console.ReadLine();

            Console.WriteLine("Enter vehicle color: ");
            var colorInput = Console.ReadLine();

            Console.WriteLine("Enter vehicle make and model: ");
            var makeAndModelInput = Console.ReadLine();

            double mileageInput;
            while (true)
            {
                Console.WriteLine("Enter vehicle mileage: ");
                try
                {
                    mileageInput = Convert.ToDouble(Console.ReadLine());
                    if (mileageInput >= 0)
                        break;
                    else
                    {
                        Console.WriteLine("Input is invalid format");
                        continue;
                    }
                }
                catch
                {
                    Console.WriteLine("Input is invalid format");
                    continue;
                }

            }

            int yearOfManufactureInput;
            while (true)
            {
                Console.WriteLine("Enter vehicle year of manufacture: ");
                try
                {
                    yearOfManufactureInput = Convert.ToInt32(Console.ReadLine());
                    if (yearOfManufactureInput >= 999 && yearOfManufactureInput <= DateTime.Now.Year)
                        break;
                    else
                    {
                        Console.WriteLine("Input is invalid format");
                        continue;
                    }
                }
                catch
                {
                    Console.WriteLine("Input is invalid format");
                    continue;
                }

            }

            Console.WriteLine("Enter vehicle description: ");
            var descriptionInput = Console.ReadLine();

            double priceInput;
            while (true)
            {
                Console.WriteLine("Enter vehicle price: ");
                try
                {
                    priceInput = Convert.ToDouble(Console.ReadLine());
                    if (priceInput >= 0)
                        break;
                    else
                    {
                        Console.WriteLine("Input is invalid format");
                        continue;
                    }
                }
                catch
                {
                    Console.WriteLine("Input is invalid format");
                    continue;
                }

            }

            bool isPriceNegotiableInput;
            while (true)
            {
                Console.WriteLine("Is price negotiable? (true/false): ");
                try
                {
                    isPriceNegotiableInput = Convert.ToBoolean(Console.ReadLine());
                    if (isPriceNegotiableInput == true || isPriceNegotiableInput == false)
                        break;
                    else
                    {
                        Console.WriteLine("Input is invalid format");
                        continue;
                    }
                }
                catch
                {
                    Console.WriteLine("Input is invalid format");
                    continue;
                }

            }
            var contactPhoneNumberInput = "";
            while (true)
            {
                Console.WriteLine("Enter contact phone number: ");
                contactPhoneNumberInput = Console.ReadLine();

                var phoneRegex = new Regex(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}");
                if (phoneRegex.IsMatch(contactPhoneNumberInput))
                    break;
                else
                {
                    Console.WriteLine("Phone is invalid format");
                    continue;
                }
            }
            if (typeInput == null || makeAndModelInput == null || mileageInput == null || yearOfManufactureInput == null)
                Console.WriteLine("You need to enter all mandatory fields (Type, Make and Model, Mileage and Year of Manufacture)\n");
            else
            {
                using (PiccEntities picc = new PiccEntities())
                {
                    Vehicle vehicle = new Vehicle
                    {
                        Type = typeInput,
                        Color = colorInput,
                        MakeAndModel = makeAndModelInput,
                        Mileage = mileageInput,
                        YearOfManufacture = yearOfManufactureInput,
                        Description = descriptionInput
                    };
                    picc.Vehicles.Add(vehicle);

                    VehicleShoppingArticle vehicleShoppingArticle = new VehicleShoppingArticle
                    {
                        Price = priceInput,
                        IsPriceNegotiable = isPriceNegotiableInput,
                        ContactPhoneNumber = contactPhoneNumberInput,
                        VehicleID = vehicle.ID
                    };
                    picc.VehicleShoppingArticles.Add(vehicleShoppingArticle);
                    picc.SaveChanges();

                    Console.WriteLine("Article is added.\n");
                }
            }
        }

        public static void SearchData()
        {

            Console.WriteLine("SEARCHING DATA\n");
            Console.WriteLine("Please choose filtering option:" +
              " 1 - Mileage," +
              " 2 - Type," +
              " 3 - Price," +
              " 4 - All of the above ");
            var filteringInput = Console.ReadLine();

            PiccEntities picc = new PiccEntities();
            picc.Vehicles.Load();

            var filteringResults = (from v in picc.Vehicles
                                    join va in picc.VehicleShoppingArticles on v.ID equals va.VehicleID

                                    select new
                                    {
                                        v.ID,
                                        v.Type,
                                        v.Color,
                                        v.MakeAndModel,
                                        v.Mileage,
                                        v.YearOfManufacture,
                                        v.Description,
                                        va.Price,
                                        va.IsPriceNegotiable,
                                        va.ContactPhoneNumber
                                    });

            if (filteringInput == "1")
            {
                Console.WriteLine("Please enter minumum mileage: ");
                var mileageInputMin = Convert.ToDouble(Console.ReadLine());
                Console.WriteLine("Please enter maximum mileage: ");
                var mileageInputMax = Convert.ToDouble(Console.ReadLine());

                var filteredByMilleage = filteringResults.Where(v => v.Mileage >= mileageInputMin && v.Mileage <= mileageInputMax).ToList();

                Console.WriteLine(String.Format("|{0,5}|{1,10}|{2,10}|{3,15}|{4,10}|{5,12}|{6,20}|{7,10}|{8,16}|{9,15}|",
                  "ID", "Type", "Color", "Make and Model", "Mileage", "Year of Man.", "Description", "Price", "Negotiable Price", "Phone Number"));

                foreach (var res in filteredByMilleage)
                {

                    Console.WriteLine(String.Format("|{0,5}|{1,10}|{2,10}|{3,15}|{4,10}|{5,12}|{6,20}|{7,10}|{8,16}|{9,15}|",
                      res.ID, res.Type.TrimEnd(), res.Color.Trim(), res.MakeAndModel.Trim(), res.Mileage, res.YearOfManufacture, res.Description.Trim(), res.Price, res.IsPriceNegotiable, res.ContactPhoneNumber.Trim()));
                }
            }

            if (filteringInput == "2")
            {
                Console.WriteLine("Please enter type: ");
                var typeInput = Console.ReadLine();

                var filteredByType = filteringResults.Where(v => v.Type == typeInput).ToList();

                Console.WriteLine(String.Format("|{0,5}|{1,10}|{2,10}|{3,15}|{4,10}|{5,12}|{6,20}|{7,10}|{8,16}|{9,15}|",
                  "ID", "Type", "Color", "Make and Model", "Mileage", "Year of Man.", "Description", "Price", "Negotiable Price", "Phone Number"));

                foreach (var res in filteredByType)
                {
                    Console.WriteLine(String.Format("|{0,5}|{1,10}|{2,10}|{3,15}|{4,10}|{5,12}|{6,20}|{7,10}|{8,16}|{9,15}|",
                      res.ID, res.Type.TrimEnd(), res.Color.Trim(), res.MakeAndModel.Trim(), res.Mileage, res.YearOfManufacture, res.Description.Trim(), res.Price, res.IsPriceNegotiable, res.ContactPhoneNumber.Trim()));
                }

            }
            if (filteringInput == "3")
            {
                Console.WriteLine("Please enter minimum price: ");
                var priceInputMin = Convert.ToDouble(Console.ReadLine());
                Console.WriteLine("Please enter maximum price: ");
                var priceInputMax = Convert.ToDouble(Console.ReadLine());

                var filteredByPrice = filteringResults.Where(v => v.Mileage >= priceInputMin && v.Mileage <= priceInputMax).ToList();

                Console.WriteLine(String.Format("|{0,5}|{1,10}|{2,10}|{3,15}|{4,10}|{5,12}|{6,20}|{7,10}|{8,16}|{9,15}|",
                  "ID", "Type", "Color", "Make and Model", "Mileage", "Year of Man.", "Description", "Price", "Negotiable Price", "Phone Number"));

                foreach (var res in filteredByPrice)
                {
                    Console.WriteLine(String.Format("|{0,5}|{1,10}|{2,10}|{3,15}|{4,10}|{5,12}|{6,20}|{7,10}|{8,16}|{9,15}|",
                      res.ID, res.Type.TrimEnd(), res.Color.Trim(), res.MakeAndModel.Trim(), res.Mileage, res.YearOfManufacture, res.Description.Trim(), res.Price, res.IsPriceNegotiable, res.ContactPhoneNumber.Trim()));
                }

            }
            if (filteringInput == "4")
            {

                Console.WriteLine("Please enter minumum mileage: ");
                var mileageInputMin = Convert.ToDouble(Console.ReadLine());
                Console.WriteLine("Please enter maximum mileage: ");
                var mileageInputMax = Convert.ToDouble(Console.ReadLine());

                Console.WriteLine("Please enter type: ");
                var typeInput = Console.ReadLine();

                Console.WriteLine("Please enter minimum price: ");
                var priceInputMin = Convert.ToDouble(Console.ReadLine());
                Console.WriteLine("Please enter maximum price: ");
                var priceInputMax = Convert.ToDouble(Console.ReadLine());

                var filteredByMilleageTypeAndPrice = (from v in picc.Vehicles
                                                      join va in picc.VehicleShoppingArticles on v.ID equals va.VehicleID
                                                      where v.Mileage >= mileageInputMin && v.Mileage <= mileageInputMax &&
                                                            v.Type == typeInput &&
                                                            va.Price >= priceInputMin && va.Price <= priceInputMax
                                                      select new
                                                      {
                                                          v.ID,
                                                          v.Type,
                                                          v.Color,
                                                          v.MakeAndModel,
                                                          v.Mileage,
                                                          v.YearOfManufacture,
                                                          v.Description,
                                                          va.Price,
                                                          va.IsPriceNegotiable,
                                                          va.ContactPhoneNumber
                                                      }).ToList();

                Console.WriteLine(String.Format("|{0,5}|{1,10}|{2,10}|{3,15}|{4,10}|{5,12}|{6,20}|{7,10}|{8,16}|{9,15}|",
                  "ID", "Type", "Color", "Make and Model", "Mileage", "Year of Man.", "Description", "Price", "Negotiable Price", "Phone Number"));

                foreach (var res in filteredByMilleageTypeAndPrice)
                {
                    Console.WriteLine(String.Format("|{0,5}|{1,10}|{2,10}|{3,15}|{4,10}|{5,12}|{6,20}|{7,10}|{8,16}|{9,15}|",
                      res.ID, res.Type.TrimEnd(), res.Color.Trim(), res.MakeAndModel.Trim(), res.Mileage, res.YearOfManufacture, res.Description.Trim(), res.Price, res.IsPriceNegotiable, res.ContactPhoneNumber.Trim()));
                }

            }
        }
        public static void UpdateData()
        {
            PiccEntities picc = new PiccEntities();
            picc.Vehicles.Load();
            var filteringResults = (from v in picc.Vehicles
                                    join va in picc.VehicleShoppingArticles on v.ID equals va.VehicleID
                                    select new
                                    {
                                        v.ID,
                                        v.Type,
                                        v.Color,
                                        v.MakeAndModel,
                                        v.Mileage,
                                        v.YearOfManufacture,
                                        v.Description,
                                        va.Price,
                                        va.IsPriceNegotiable,
                                        va.ContactPhoneNumber
                                    }).ToList();

            foreach (var res in filteringResults)
            {
                Console.WriteLine(String.Format("|{0,5}|{1,2}|{2,2}|{3,2}|{4,2}|{5,2}|{6,2}|{7,2}|{8,2}|{9,2}|",
                  res.ID, res.Type.TrimEnd(), res.Color.Trim(), res.MakeAndModel.Trim(), res.Mileage, res.YearOfManufacture, res.Description.Trim(), res.Price, res.IsPriceNegotiable, res.ContactPhoneNumber.Trim()));
            }
            Console.WriteLine("\n\n\n\nPlease enter ID of record to update: ");
            int updateInputInt;
            while (true)
            {
                try
                {
                    updateInputInt = Convert.ToInt32(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Invalid input");
                    break;
                }

                Console.WriteLine("Choose one of the following fields to update: 1 - Price, 2 - Description");
                var fieldToUpdate = Console.ReadLine();

                if (fieldToUpdate == "1")
                {
                    Console.WriteLine("\n\n\n\nPlease enter new price\n\n");
                    try
                    {
                        var newPrice = Convert.ToDouble(Console.ReadLine());

                        VehicleShoppingArticle vehicleArticleToUpdate = (from va in picc.VehicleShoppingArticles join v in picc.Vehicles on va.VehicleID equals v.ID where va.VehicleID >= updateInputInt select va).FirstOrDefault();
                        vehicleArticleToUpdate.Price = newPrice;
                        Console.WriteLine("Field is updated!");
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("Invalid input");
                        break;
                    }

                }
                else if (fieldToUpdate == "2")
                {
                    Console.WriteLine("\n\n\n\nPlease enter new description\n\n");
                    try
                    {
                        var newDescription = Console.ReadLine();

                        Vehicle vehicleToUpdate = (from v in picc.Vehicles join va in picc.VehicleShoppingArticles on v.ID equals va.VehicleID where va.VehicleID >= updateInputInt select v).FirstOrDefault();

                        vehicleToUpdate.Description = newDescription;
                        Console.WriteLine("Field is updated!");
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("Invalid input");
                        break;
                    }

                }
                else
                {
                    Console.WriteLine("Invalid choice!");
                    continue;
                }

            }

            picc.SaveChanges();
        }
        public static void DeleteData()
        {
            PiccEntities picc = new PiccEntities();
            picc.Vehicles.Load();
            var filteringResults = (from v in picc.Vehicles
                                    join va in picc.VehicleShoppingArticles on v.ID equals va.VehicleID
                                    select new
                                    {
                                        v.ID,
                                        v.Type,
                                        v.Color,
                                        v.MakeAndModel,
                                        v.Mileage,
                                        v.YearOfManufacture,
                                        v.Description,
                                        va.Price,
                                        va.IsPriceNegotiable,
                                        va.ContactPhoneNumber
                                    }).ToList();

            Console.WriteLine(String.Format("|{0,5}|{1,10}|{2,10}|{3,15}|{4,10}|{5,12}|{6,20}|{7,10}|{8,16}|{9,15}|",
              "ID", "Type", "Color", "Make and Model", "Mileage", "Year of Man.", "Description", "Price", "Negotiable Price", "Phone Number"));

            foreach (var res in filteringResults)
            {

                Console.WriteLine(String.Format("|{0,5}|{1,10}|{2,10}|{3,15}|{4,10}|{5,12}|{6,20}|{7,10}|{8,16}|{9,15}|",
                  res.ID, res.Type.TrimEnd(), res.Color.Trim(), res.MakeAndModel.Trim(), res.Mileage, res.YearOfManufacture, res.Description.Trim(), res.Price, res.IsPriceNegotiable, res.ContactPhoneNumber.Trim()));
            }

            Console.WriteLine("\n\n\n\nPlease enter ID of record to delete\n\n");

            var deleteInput = Convert.ToInt32(Console.ReadLine());

            var recordToDelete = (from v in picc.Vehicles where v.ID == deleteInput select v).FirstOrDefault();
            try
            {
                picc.Vehicles.Remove(recordToDelete);
            }
            catch
            {
                //Console.WriteLine("Please enter valid ID");
            }

            var recordToDelete2 = (from va in picc.VehicleShoppingArticles where va.VehicleID == deleteInput select va).FirstOrDefault();

            try
            {
                picc.VehicleShoppingArticles.Remove(recordToDelete2);
            }
            catch
            {
                Console.WriteLine("Please enter valid ID");
            }

            picc.SaveChanges();
        }
    }
}