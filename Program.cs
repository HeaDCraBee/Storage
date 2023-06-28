using Storage.Controller;
using Storage.Domain;
using Storage.DAL;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

namespace Storage
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Select action:\n" +
                    "1.Print pallets\n" +
                    "2.Print boxes\n" +
                    "3.Add pallet\n" +
                    "4.Add box to pallet\n" +
                    "5.Group pallets\n" +
                    "6.Show 3 pallets with higher boxes expiration date\n" +
                    "7.Exit");

                var controller = new StorageController(new StorageRepository(new StorageDbContext()));

                var answer = Console.ReadLine();
                try
                {
                    switch (answer)
                    {
                        case "1":
                            if (controller.GetPallets().Count == 0)
                            {
                                Console.WriteLine("There is no pallets");
                                break;
                            }
                            foreach (var pallet in controller.GetPallets())
                                Console.WriteLine(pallet);
                            break;

                        case "2":
                            if (controller.GetBoxes().Count == 0)
                            {
                                Console.WriteLine("There is no boxes");
                                break;
                            }
                            foreach (var box in controller.GetBoxes())
                                Console.WriteLine(box);
                            break;

                        case "3":
                            Console.WriteLine("Enter a space-separated width, height and depth of new pallet");
                            var newPallet = Console.ReadLine().Split(" ");

                            if (newPallet.Length != 3)
                            {
                                Console.WriteLine("You must enter width, height and depth");
                                break;
                            }

                            if (!float.TryParse(newPallet[0], out var width) | !float.TryParse(newPallet[1], out var height) | !float.TryParse(newPallet[2], out var depth))
                            {
                                Console.WriteLine("Entered incorrect values");
                            }

                            controller.AddPallet(new Pallet(width, height, depth));
                            Console.WriteLine("successfully");
                            break;

                        case "4":
                            Console.WriteLine("Enter a space-separated width, height, depth and date of manufacture in format dd/MM/yyyy of new box");
                            var newBox = Console.ReadLine().Split(" ");

                            if (newBox.Length != 4)
                            {
                                Console.WriteLine("You must enter width, height, depth and date of manufacture in format dd/MM/yyyy");
                                break;
                            }

                            if (!float.TryParse(newBox[0], out width) | !float.TryParse(newBox[1], out height) | !float.TryParse(newBox[2], out depth) | !DateTime.TryParse(newBox[3], out var dateOfManufacture))
                            {
                                Console.WriteLine("Entered incorrect values");
                                break;
                            }

                            var expirationDate = DateTime.MinValue;

                            Console.WriteLine("Do you want to enter expiration date?[y/n]");
                            answer = Console.ReadLine();
                            if (answer == "y")
                            {
                                Console.WriteLine("Enter expiration date in format dd/MM/yyyy");
                                if (!DateTime.TryParse(Console.ReadLine(), out expirationDate))
                                {
                                    Console.WriteLine("Entered incorrect value");
                                    break;
                                }
                            }

                            Console.WriteLine("Avaliable pallets:");

                            var pallets = controller.GetPallets();
                            foreach (var pallet in pallets)
                                Console.WriteLine(pallet);

                            Console.WriteLine("Enter the Id of the pallet in which to place the box");
                            if (!int.TryParse(Console.ReadLine(), out var palletId))
                            {
                                Console.WriteLine("Entered incorrect value");
                                break;
                            }

                            var enteredPallet = pallets.FirstOrDefault(p => p.Id == palletId);

                            if (enteredPallet == null)
                            {
                                Console.WriteLine("There is no such pallet");
                                break;
                            }

                            controller.AddBox(expirationDate == DateTime.MinValue ? new Box(width, height, depth, dateOfManufacture) : new Box(width, height, depth, dateOfManufacture, expirationDate), enteredPallet);
                            Console.WriteLine("successfully");
                            break;

                        case "5":
                            foreach (var pair in controller.GetGroups())
                            {
                                Console.WriteLine(pair.Key.ToShortDateString());

                                foreach (var pls in pair.Value)
                                    Console.WriteLine(pls);
                            }
                            break;

                        case "6":
                            foreach (var pls in controller.GetPalletsWithHigherBoxExpirationDate())
                            {
                                if (pls != null)
                                    Console.WriteLine(pls);
                            }
                            break;

                        case "7":
                            Environment.Exit(0);
                            break;

                        default:
                            Console.WriteLine("Unknown action");
                            break;
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Something went wrong, please try again:\n{ex.Message}");
                }

                Console.WriteLine($"Press any key...");
                Console.ReadKey();
            }            
        }
    }
}