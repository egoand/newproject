using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEgorov_lab1
{
    class RealEstate
    {
        public string Owner { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int ApproxCost { get; set; }
    }
    class RuralRealEstate : RealEstate
    {
        public string Street { get; set; }
        public int HouseNumber { get; set; }
    }
    class UrbanRealEstate : RuralRealEstate
    {
        public string District { get; set; }
        public string Shore { get; set; }
    }

    class Program
    {
        public static List<string> StrConverter(string data)
        {
            int i = 0;
            int count = 0;
            string res_data = data.TrimStart().TrimEnd();
            string owner = "";
            string x = "";
            if (res_data.StartsWith("\""))
            {
                for (i = 1; i < res_data.Length; i++)
                {
                    x += res_data[i];
                    if (x.EndsWith("\""))
                    {
                        break;
                    }
                    owner += res_data[i];
                    count += 1;
                }
            }
            res_data = res_data.Remove(0, count + 2);
            res_data = res_data.TrimStart();

            List<string> ConvertedData = new List<string>(res_data.Split(' '));
            ConvertedData.Insert(0, owner);
            List<string> result = new List<string>();

            i = 0;
            while (i < ConvertedData.Count)
            {
                if (ConvertedData[i] != "")
                {
                    result.Add(ConvertedData[i]);
                }
                i++;
            }

            return result;
        }

        public static RealEstate REInfoConverter(List<string> data)
        {
            string owner = data[0];
            DateTime registrationDate = DateTime.Parse(data[1]);
            int approxCost = int.Parse(data[2]);

            RealEstate realEstate = new RealEstate
            {
                Owner = owner,
                RegistrationDate = registrationDate,
                ApproxCost = approxCost
            };

            return realEstate;
        }

        public static RuralRealEstate RuralREInfoConverter(List<string> data)
        {
            RealEstate Source = REInfoConverter(data);

            string street = data[3];
            int houseNumber = int.Parse(data[4]);

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

        public static UrbanRealEstate UrbanREInfoConverter(List<string> data)
        {
            RuralRealEstate Source = RuralREInfoConverter(data);

            string district = data[5];
            string shore = data[6];

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

        public static List<string> SplitStringIntoObjects(string inputLine)
        {
            List<string> objects = new List<string>();
            StringBuilder currentObject = new StringBuilder();
            int partCount = 0;

            string[] parts = inputLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string part in parts)
            {
                currentObject.Append(part).Append(" ");

                if (part.EndsWith("\""))
                    partCount = 1;
                else if (!part.StartsWith("\""))
                    partCount++;

                if (partCount == 3 || partCount == 5 || partCount == 7)
                {
                    objects.Add(currentObject.ToString().Trim());
                    currentObject.Clear();
                    partCount = 0;
                }
            }

            string lastObject = currentObject.ToString().Trim();
            if (!string.IsNullOrEmpty(lastObject))
                objects.Add(lastObject);

            return objects;
        }

        public static List<string> ReadDataFromFile(string filePath)
        {
            List<string> allInfo = new List<string>();

            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        List<string> objectsInLine = SplitStringIntoObjects(line.Trim());
                        allInfo.AddRange(objectsInLine);
                    }
                }
            }

            return allInfo;
        }

        static void Main(string[] args)
        {
            List<string> AllInfo = new List<string>();
            List<RealEstate> REI_lst = new List<RealEstate>();
            List<RealEstate> RuralREI_lst = new List<RealEstate>();
            List<RealEstate> UrbanREI_lst = new List<RealEstate>();

            Console.WriteLine("Введите данные (пустая строка - переход к файлу):");
            string consoleInput = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(consoleInput))
            {
                AllInfo = SplitStringIntoObjects(consoleInput.Trim());
            }
            else
            {
                string projectDirectory = Directory.GetCurrentDirectory();
                string filePath = Path.Combine(projectDirectory, "file.txt");
                AllInfo = ReadDataFromFile(filePath);

                if (AllInfo.Count == 0)
                {
                    AllInfo = new List<string>() {
                        "\"Егоров А.Р.\"     2025.09.05 15000000",
                        "\"Емельянов В. И.\"     2025.12.17             4700000  Добрая    18",
                        "\"Трусов Н. А.\"     2010.05.01     7000000  Ленина   9  Октябрьский        Левый"
                    };
                }
            }

            for (int i = 0; i < AllInfo.Count; i++)
            {
                List<string> result = StrConverter(AllInfo[i]);
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
            }

            Console.ReadKey();
        }
    }
}