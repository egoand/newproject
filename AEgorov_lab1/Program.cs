using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEgorov_lab1
{
    class Company
    {
        public string CompanyName { get; set; }
        public int ApproxIncome { get; set; }
    }
    class RealEstate
    {
        public string Owner { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int ApproxCost { get; set; }
    }
    class TaxInfo
    {
        public bool PaysTaxes { get; set; }
        public Currency AcceptedCurrency { get; set; }
    }
    enum Currency
    {
        RUB,
        USD,
        EUR
    }



    class Program
    {
        public static Company CIInfoConverter(string CI)
        {
            int i = 0;
            int count = 0;
            string res_CI = CI.TrimStart().TrimEnd();
            string companyName = "";
            string x = "";
            if (res_CI.StartsWith("\""))
            {
                for (i = 1; i < res_CI.Length; i++)
                {
                    x += res_CI[i];
                    if (x.EndsWith("\""))
                    {
                        break;
                    }
                    companyName += res_CI[i];
                    count += 1;
                }
            }
            res_CI = res_CI.Remove(0, count + 2);
            count = 0;
            i = 0;

            res_CI = res_CI.TrimStart();

            int approxIncome = int.Parse(res_CI);

            Company company = new Company
            {
                CompanyName = companyName,
                ApproxIncome = approxIncome
            };
            return company;
        }

        public static RealEstate REIInfoConverter(string REI)
        {
            int i = 0;

            string res_REI = REI.TrimStart().TrimEnd();
            string owner = "";
            string x = "";
            int count = 0;
            if (res_REI.StartsWith("\""))
            {
                for (i = 1; i < res_REI.Length; i++)
                {
                    x += res_REI[i];
                    if (x.EndsWith("\""))
                    {
                        break;
                    }
                    owner += res_REI[i];
                    count += 1;
                }
            }
            res_REI = res_REI.Remove(0, count + 2);
            count = 0;
            i = 0;

            res_REI = res_REI.TrimStart();

            List<string> rei_all_lst = new List<string>(res_REI.Split(' '));
            List<string> rei_res_lst = new List<string>();

            i = 0;
            while (i < rei_all_lst.Count)
            {
                if (rei_all_lst[i] != "")
                {
                    rei_res_lst.Add(rei_all_lst[i]);
                }
                i++;
            }
            i = 0;

            DateTime registrationDate = DateTime.Parse(rei_res_lst[0]);
            int approxCost = int.Parse(rei_res_lst[1]);

            RealEstate realEstate = new RealEstate
            {
                Owner = owner,
                RegistrationDate = registrationDate,
                ApproxCost = approxCost
            };

            return realEstate;
        }

        static void Main(string[] args)
        {
            List<string> lst_rei_all_global = new List<string>() { "\"Егоров А.Р.\"     2025.09.05 45000000", "\"Емельянов В. И.\"     2025.12.17             45000000" };
            List<string> lst_сi_all_global = new List<string>() { "\"Аренда недвижимости\"                  1000000000" };

            List<RealEstate> ListRealEstate = new List<RealEstate>();
            List<Company> ListCompany = new List<Company>();

            for (int i = 0; i < lst_rei_all_global.Count; i++)
            {
                ListRealEstate.Add(REIInfoConverter(lst_rei_all_global[i]));
            }

            for (int i = 0; i < lst_сi_all_global.Count; i++)
            {
                ListCompany.Add(CIInfoConverter(lst_сi_all_global[i]));
            }

            int minnum = ListRealEstate[0].ApproxCost;
            int count = 0;
            for (int i = 0; i < ListRealEstate.Count; i++)
            {
                if (minnum > ListRealEstate[i].ApproxCost)
                {
                    minnum = ListRealEstate[i].ApproxCost;
                    count = i;
                }
            }
            Console.WriteLine(ListRealEstate[count].Owner);
            Console.WriteLine(ListRealEstate[count].ApproxCost);

            Console.ReadKey();
        }
    }
}