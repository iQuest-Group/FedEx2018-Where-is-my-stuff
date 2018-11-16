using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WIMS.Repository
{
    public abstract class WimsRepository<T> where T: Entity
    {
        private String endpointUrl { get; set; }
        private String primaryKey { get; set; }

        protected String databaseName;
        protected DocumentClient docClient;

        protected WimsRepository(string endpointUrlParam, string primaryKeyParam, string databaseNameParam)
        {
            endpointUrl = endpointUrlParam;
            primaryKey = primaryKeyParam;
            databaseName = databaseNameParam;
            docClient = new DocumentClient(new Uri(endpointUrl), primaryKey);
        }

        protected abstract string CollectionName { get;  }

        public async Task CreateDatabase()
        {
            await docClient.CreateDatabaseIfNotExistsAsync(new Database { Id = databaseName });
        }

        public async Task EnsureCollection()
        {
            Uri databaseUri = UriFactory.CreateDatabaseUri(databaseName);
            await docClient.CreateDocumentCollectionIfNotExistsAsync(databaseUri,
                 new DocumentCollection { Id = CollectionName });
        }

        protected async Task CreateEntityIfNotExists(T entity)
        {
            try
            {
                await docClient.ReadDocumentAsync(CreateDocumentUri(entity));
            }
            catch (DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    Uri documentCollectionUri = UriFactory.CreateDocumentCollectionUri(databaseName, CollectionName);
                    await docClient.CreateDocumentAsync(documentCollectionUri, entity);
                }
                else
                {
                    throw;
                }
            }
        }

        protected Uri CreateDocumentUri(T entity)
        {
            return UriFactory.CreateDocumentUri(databaseName, CollectionName, entity.ID.ToString());
        }

        public async Task AddEntities(T[] entities)
        {
            foreach (T entity in entities)
            {
                await CreateEntityIfNotExists(entity);
            }
        }

        public List<Entity> GetAllRecordsFromCollection()
        {
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            IQueryable<Entity> query = docClient.CreateDocumentQuery<Entity>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, CollectionName), queryOptions);

            return query.ToList();
        }

        public T GetEntity(Guid id)
        {
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            IQueryable<T> query = this.docClient.CreateDocumentQuery<T>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, CollectionName), queryOptions)
                    .Where(i => i.ID == id);

            return query.Take(1).AsEnumerable().First();
        }
    }
}
