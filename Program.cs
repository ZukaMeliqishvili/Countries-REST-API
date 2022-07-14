using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CountriesRESTApiApplication.Models;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace CountriesRESTApiApplication
{
    class Program
    {
        static async Task<IEnumerable<Country>> GetCountriesDataFromService()
        {
            var uri = new Uri("https://restcountries.com/");
            var httpClient = new HttpClient()
            {
                BaseAddress = uri
            };
            var response = await httpClient.GetAsync("v3.1/all");
            var responseString = await response.Content.ReadAsStringAsync();
            var countries = JsonConvert.DeserializeObject<IEnumerable<Country>>(responseString);
            return countries;
        }

        static void GenerateCountryDataFiles(IEnumerable<Country> countries)
        {
            string uploadPath = @"C:\Users\zuka\Desktop";
            string directoryName = @"Countries data";
            string directoryFullPath = Path.Combine(uploadPath, directoryName);
            DirectoryInfo Dir = new DirectoryInfo(directoryFullPath);
            if (!Dir.Exists)
            {
                Dir.Create();
            }

            foreach (var country in countries)
            {
                var fileName = string.Concat(country.Name.Common, ".txt");
                using (StreamWriter streamWriter = new StreamWriter(Path.Combine(directoryFullPath, fileName), true))
                {
                    try
                    {
                        streamWriter.WriteLine($"region:{country.Region}");
                        streamWriter.WriteLine($"subregion:{country.SubRegion}");
                        streamWriter.WriteLine($"latng:{country.Coordinates.First()},{country.Coordinates.Last()}");
                        streamWriter.WriteLine($"population:{country.Population}");
                    }
                    catch
                    {
                        streamWriter.Close();
                    }
                }

            }
        }
        static async Task Main()
        {
            var countries = await GetCountriesDataFromService();
            GenerateCountryDataFiles(countries);
        }
    }
}