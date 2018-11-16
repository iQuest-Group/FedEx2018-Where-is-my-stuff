using iQuest.Fedex2018.Wims.Repository;
using iQuest.Fedex2018.Wims.StockProvisioner.Properties;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace iQuest.Fedex2018.Wims.StockProvisioner
{
    static class Program
    {
        static void Main(string[] args)
        {
            StockRepository stockRepository = new StockRepository(
                   Settings.Default.AzureCosmosDBEndpointUrl,
                   Settings.Default.AzureCosmodBDPrimaryKey,
                   Settings.Default.AzureCosmosDatabaseName);

            Task.WaitAll(stockRepository.EnsureCollection());

            Product[] products = GetDemoProducts();
            Task.WaitAll(stockRepository.AddEntities(products));
        }

         public static Product[] GetDemoProducts()
        {
            List<Product> products;
            string path = "DemoData\\Products.json";

            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                products = JsonConvert.DeserializeObject<List<Product>>(json);
            }
            return products.ToArray();
        }


    }
}
