namespace iQuest.Fedex2018.Wims.Repository
{
    public class ProductRepository: WimsRepository<Product>
    {
        public ProductRepository(string EndpointUrl, string PrimaryKey, string DatabaseName) : 
            base(EndpointUrl, PrimaryKey, DatabaseName) {
        }

        protected override string CollectionName => "Products";
    }
}
