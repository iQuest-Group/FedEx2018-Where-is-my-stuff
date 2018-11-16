using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace WIMS_Repository
{
    public class WIMSRepository
    {
        private String EndpointUrl { get; set; }
        private String PrimaryKey { get; set; }

        internal String DatabaseName = "Inventory";
        internal DocumentClient docClient;

        internal static String DemoFolderPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"DemoData\");

        public WIMSRepository(string EndpointUrl, string PrimaryKey, string DatabaseName)
        {
            this.EndpointUrl = EndpointUrl;
            this.PrimaryKey = PrimaryKey;
            this.DatabaseName = DatabaseName;
            this.docClient = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);
        }
        public async Task CreateDatabase()
        {
            await this.docClient.CreateDatabaseIfNotExistsAsync(new Database { Id = this.DatabaseName });
        }
        public async Task CreateCollection(string CollectionName)
        {
            await this.docClient.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(this.DatabaseName), new DocumentCollection { Id = CollectionName});
        }

        public async Task CreateEntityIfNotExists(string collectionName, Entity entity)
        {
            try
            {
                await docClient.ReadDocumentAsync(UriFactory.CreateDocumentUri(this.DatabaseName, collectionName, entity.ID.ToString()));
            }
            catch (DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    await docClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(this.DatabaseName, collectionName), entity);
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task AddEntities(string collectionName, Entity[] entities)
        {
            foreach (Entity entity in entities)
            {
                await this.CreateEntityIfNotExists(collectionName, entity);
            }
        }

        public List<Entity> GetAllRecordsFromCollection(string collectionName)
        {
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            IQueryable<Entity> query = this.docClient.CreateDocumentQuery<Entity>(
                    UriFactory.CreateDocumentCollectionUri(this.DatabaseName, collectionName), queryOptions);

            return query.ToList();
        }
    }
}
