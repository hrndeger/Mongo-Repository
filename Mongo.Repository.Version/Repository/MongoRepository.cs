using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Mongo.Repository.Builder;
using Mongo.Repository.Version.Builder;
using MongoDB.Driver;

namespace Mongo.Repository.Version.Repository
{
    /// <summary>
    /// The Mongo repository implementation
    /// </summary>
    /// <seealso cref="Mongo.Repository.IMongoRepository" />
    public class MongoRepository : IMongoRepository
    {
        #region Fields

        private IMongoDatabase _mongoDatabase;
        private readonly IMongoClient _client;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoRepository"/> class.
        /// </summary>
        public MongoRepository()
        {
            _client = CreateClient();
        }

        /// <summary>
        /// Creates the client.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NullReferenceException">
        /// Mongo host has not configured!
        /// or
        /// Database has not created!
        /// </exception>
        private IMongoClient CreateClient()
        {
            var host = ConfigurationManager.AppSettings["host"].ToString();
            var database = ConfigurationManager.AppSettings["database"];

            if (string.IsNullOrEmpty(host)) throw new NullReferenceException("Mongo host has not configured!");
            if (string.IsNullOrEmpty(database)) throw new NullReferenceException("Database has not created!");

            var client = new MongoClient(MongoUrl.Create(host));
            _mongoDatabase = client.GetDatabase(database);

            return client;
        }

        #endregion


        #region Collection

        private IMongoCollection<TDocument> GetCollection<TDocument>()
            where TDocument : IDocument
        {
            var type = typeof(TDocument);
            return _mongoDatabase.GetCollection<TDocument>(type.Name);
        }

        #endregion


        /// <summary>
        /// Inserts the specified document.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="document">The document.</param>
        public void Insert<TDocument>(TDocument document) where TDocument : IDocument
        {
            try
            {
                document._id = Guid.NewGuid().ToString();

                GetCollection<TDocument>().InsertOne(document);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Inserts the specified documents.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="documents">The documents.</param>
        public void Insert<TDocument>(IEnumerable<TDocument> documents) where TDocument : IDocument
        {
            foreach (var document in documents)
            {
                document._id = Guid.NewGuid().ToString();
            }

            GetCollection<TDocument>().InsertMany(documents);
        }

        /// <summary>
        /// Queries this instance.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <returns></returns>
        public IQueryable<TDocument> Query<TDocument>() where TDocument : IDocument
        {
            return GetCollection<TDocument>().AsQueryable();

        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public TDocument Get<TDocument>(string id) where TDocument : IDocument
        {
            return GetCollection<TDocument>().AsQueryable().FirstOrDefault(x => x._id == id);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="id">The identifier.</param>
        public void Delete<TDocument>(string id) where TDocument : IDocument
        {
            GetCollection<TDocument>().DeleteOne(id);
        }

        /// <summary>
        /// Deletes the specified filter.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="filter">The filter.</param>
        public void Delete<TDocument>(Action<IFilterBuilder<TDocument>> filter) where TDocument : IDocument
        {
            var filterBuilder = new FilterBuilder<TDocument>();
            filter(filterBuilder);

            GetCollection<TDocument>().FindOneAndDelete(filterBuilder.Filter);
        }

        /// <summary>
        /// Deletes the specified filter.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="filter">The filter.</param>
        public void DeleteMany<TDocument>(Action<IFilterBuilder<TDocument>> filter) where TDocument : IDocument
        {
            var filterBuilder = new FilterBuilder<TDocument>();
            filter(filterBuilder);

            GetCollection<TDocument>().DeleteMany(filterBuilder.Filter);
        }

        /// <summary>
        /// Firsts this instance.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <returns></returns>
        public TDocument First<TDocument>() where TDocument : IDocument
        {
            return GetCollection<TDocument>().AsQueryable().FirstOrDefault();
        }

        /// <summary>
        /// Lasts this instance.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <returns></returns>
        public TDocument Last<TDocument>() where TDocument : IDocument
        {
            return GetCollection<TDocument>().AsQueryable().LastOrDefault();
        }

        /// <summary>
        /// Updates the specified filter.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="filter">The filter.</param>
        /// <param name="updates">The updates.</param>
        /// <returns></returns>
        public bool Update<TDocument>(Action<IFilterBuilder<TDocument>> filter, params Action<IUpdateBuilder<TDocument>>[] updates) where TDocument : IDocument
        {
            var filterBuilder = new FilterBuilder<TDocument>();
            filter(filterBuilder);

            foreach (var updateBuilder in updates)
            {
                var tempBuilder = new UpdateBuilder<TDocument>();
                updateBuilder(tempBuilder);

                return GetCollection<TDocument>()
                    .UpdateOne(filterBuilder.Filter, tempBuilder.UpdateDefinition).IsAcknowledged;
            }

            return default(bool);
        }

        /// <summary>
        /// Updates the many.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="filter">The filter.</param>
        /// <param name="updates">The updates.</param>
        /// <returns></returns>
        public bool UpdateMany<TDocument>(Action<IFilterBuilder<TDocument>> filter, params Action<IUpdateBuilder<TDocument>>[] updates) where TDocument : IDocument
        {
            var filterBuilder = new FilterBuilder<TDocument>();
            filter(filterBuilder);

            foreach (var updateBuilder in updates)
            {
                var tempBuilder = new UpdateBuilder<TDocument>();
                updateBuilder(tempBuilder);

                return
                    GetCollection<TDocument>()
                        .UpdateMany(filterBuilder.Filter, tempBuilder.UpdateDefinition)
                        .IsAcknowledged;
            }

            return default(bool);
        }

        public int Count<TDocument>() where TDocument : IDocument
        {
            return GetCollection<TDocument>().AsQueryable().Count();
        }

        public void Group<TDocument>(Action<IProjectionBuilder<TDocument>> projection) where TDocument : IDocument
        {
            var projectionBuilder = new ProjectionBuilder<TDocument>();
            projection(projectionBuilder);

            GetCollection<TDocument>().Aggregate().Group(projectionBuilder.ProjectDefinition);
        }
    }
}