using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


namespace AEgorov_lab1
{
    public class RealEstate
    {
        public string Owner { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int ApproxCost { get; set; }
    }

    public class RuralRealEstate : RealEstate
    {
        public string Street { get; set; }
        public int HouseNumber { get; set; }
    }

    public class UrbanRealEstate : RuralRealEstate
    {
        public string District { get; set; }
        public string Shore { get; set; }
    }

    public static class Errors
    {
        public static void ValidateDataCount(List<string> data, int expectedCount)
        {
            if (data == null)
                throw new ArgumentNullException("Список данных не может быть пустым");

            if (data.Count != expectedCount)
                throw new ArgumentException($"Ожидалось {expectedCount} элементов, но получено {data.Count}");
        }

        public static void ValidateOwner(string owner)
        {
            if (string.IsNullOrWhiteSpace(owner))
                throw new ArgumentException("Имя владельца не может быть пустым");

            if (owner.Length < 3)
                throw new ArgumentException("Имя владельца слишком короткое");
        }

        public static void ValidateRegistrationDate(DateTime date)
        {
            if (date > DateTime.Now)
                throw new ArgumentException("Дата регистрации не может быть в будущем");

            if (date.Year < 1900)
                throw new ArgumentException("Дата регистрации слишком старая");
        }

        public static void ValidateCost(int cost)
        {
            if (cost <= 0)
                throw new ArgumentException("Стоимость должна быть положительной");

            if (cost > 1000000000)
                throw new ArgumentException("Стоимость слишком большая");
        }

        public static void ValidateStreet(string street)
        {
            if (string.IsNullOrWhiteSpace(street))
                throw new ArgumentException("Название улицы не может быть пустым");
        }

        public static void ValidateHouseNumber(int houseNumber)
        {
            if (houseNumber <= 0)
                throw new ArgumentException("Номер дома должен быть положительным");

            if (houseNumber > 10000)
                throw new ArgumentException("Номер дома слишком большой");
        }

        public static void ValidateDistrict(string district)
        {
            if (string.IsNullOrWhiteSpace(district))
                throw new ArgumentException("Район не может быть пустым");
        }

        public static void ValidateShore(string shore)
        {
            if (string.IsNullOrWhiteSpace(shore))
                throw new ArgumentException("Берег не может быть пустым");

            if (shore != "Левый" && shore != "Правый")
                throw new ArgumentException("Берег должен быть: Левый или Правый");
        }
    }

    public class Program
    {
        // "hello" 123
        public static List<string> CleaningString(string data)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(data))
                    throw new ArgumentException("Входная строка не может быть пустой");

                List<string> result = new List<string>();
                string pattern = @"""[^""]*""|\S+";

                MatchCollection matches = Regex.Matches(data, pattern);

                foreach (Match match in matches)
                {
                    string value = match.Value;
                    if (value.StartsWith("\"") && value.EndsWith("\""))
                    {
                        value = value.Substring(1, value.Length - 2);
                    }
                    result.Add(value);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при очистке строки: {ex.Message}", ex);
            }
        }

        public static RealEstate REInfoConverter(List<string> data)
        {
            try
            {
                Errors.ValidateDataCount(data, 3);

                string owner = data[0];
                Errors.ValidateOwner(owner);

                DateTime registrationDate;
                if (!DateTime.TryParse(data[1], out registrationDate))
                    throw new ArgumentException("Некорректный формат даты регистрации");
                Errors.ValidateRegistrationDate(registrationDate);

                int approxCost;
                if (!int.TryParse(data[2], out approxCost))
                    throw new ArgumentException("Некорректный формат стоимости");
                Errors.ValidateCost(approxCost);

                RealEstate realEstate = new RealEstate
                {
                    Owner = owner,
                    RegistrationDate = registrationDate,
                    ApproxCost = approxCost
                };

                return realEstate;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при создании объекта недвижимости: {ex.Message}", ex);
            }
        }

        public static RuralRealEstate RuralREInfoConverter(List<string> data)
        {
            try
            {
                Errors.ValidateDataCount(data, 5);

                RealEstate Source = REInfoConverter(data.GetRange(0, 3));

                string street = data[3];
                Errors.ValidateStreet(street);

                int houseNumber;
                if (!int.TryParse(data[4], out houseNumber))
                    throw new ArgumentException("Некорректный формат номера дома");
                Errors.ValidateHouseNumber(houseNumber);

                RuralRealEstate ruralRealEstate = new RuralRealEstate
                {
                    Owner = Source.Owner,
                    RegistrationDate = Source.RegistrationDate,
                    ApproxCost = Source.ApproxCost,
                    Street = street,
                    HouseNumber = houseNumber
                };

                return ruralRealEstate;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при создании объекта сельской недвижимости: {ex.Message}", ex);
            }
        }

        public static UrbanRealEstate UrbanREInfoConverter(List<string> data)
        {
            try
            {
                Errors.ValidateDataCount(data, 7);

                RuralRealEstate Source = RuralREInfoConverter(data.GetRange(0, 5));

                string district = data[5];
                Errors.ValidateDistrict(district);

                string shore = data[6];
                Errors.ValidateShore(shore);

                UrbanRealEstate urbanRealEstate = new UrbanRealEstate
                {
                    Owner = Source.Owner,
                    RegistrationDate = Source.RegistrationDate,
                    ApproxCost = Source.ApproxCost,
                    Street = Source.Street,
                    HouseNumber = Source.HouseNumber,
                    District = district,
                    Shore = shore
                };

                return urbanRealEstate;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при создании объекта городской недвижимости: {ex.Message}", ex);
            }
        }

        public static List<string> ReadDataFromFile(string filePath)
        {
            try
            {
                List<string> allInfo = new List<string>();

                if (File.Exists(filePath))
                {
                    string[] lines = File.ReadAllLines(filePath);
                    foreach (string line in lines)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            allInfo.Add(line);
                        }
                    }
                }
                else
                {
                    throw new FileNotFoundException($"Файл не найден: {filePath}");
                }

                return allInfo;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при чтении файла: {ex.Message}", ex);
            }
        }

        public static void Main(string[] args)
        {
            try
            {
                List<string> AllInfo = new List<string>();
                List<RealEstate> REI_lst = new List<RealEstate>();
                List<RealEstate> RuralREI_lst = new List<RealEstate>();
                List<RealEstate> UrbanREI_lst = new List<RealEstate>();

                string projectDirectory = Directory.GetCurrentDirectory();
                string filePath = Path.Combine(projectDirectory, "file.txt");

                try
                {
                    AllInfo = ReadDataFromFile(filePath);
                }
                catch (FileNotFoundException)
                {
                    // Если файл не найден, используем тестовые данные
                    AllInfo = new List<string>() {
                        "\"Егоров А.Р.\"     2025.09.05 15000000",
                        "\"Емельянов В. И.\"     2025.12.17             4700000  \"Добрая\"    18",
                        "\"Трусов Н. А.\"     2010.05.01     7000000  \"Ленина\"   9  \"Октябрьский\"        \"Левый\""
                    };
                }

                for (int i = 0; i < AllInfo.Count; i++)
                {
                    try
                    {
                        List<string> result = CleaningString(AllInfo[i]);
                        if (result.Count == 3)
                        {
                            REI_lst.Add(REInfoConverter(result));
                        }
                        else if (result.Count == 5)
                        {
                            RuralREI_lst.Add(RuralREInfoConverter(result));
                        }
                        else if (result.Count == 7)
                        {
                            UrbanREI_lst.Add(UrbanREInfoConverter(result));
                        }
                        else
                        {
                            Console.WriteLine($"Предупреждение: Строка {i + 1} содержит некорректное количество элементов: {result.Count}");
                        }
                        //Console.WriteLine(REI_lst[0].Owner);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка в строке {i + 1}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Критическая ошибка: {ex.Message}");
            }
            Console.ReadKey();
        }
    }
}