using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WIMS.Repository
{
    public class ProductRepository: WIMSRepository
    {
        private const String CollectionName = "Products";
        private const String DemoFileName = "Products.json";


        public ProductRepository(string EndpointUrl, string PrimaryKey, string DatabaseName) : base(EndpointUrl, PrimaryKey, DatabaseName) { }

        public static Product[] GetDemoProducts()
        {
            List<Product> products;
            string path = WIMSRepository.DemoFolderPath + DemoFileName;

            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                products = JsonConvert.DeserializeObject<List<Product>>(json);
            }
            return products.ToArray();
        }
    }
}
