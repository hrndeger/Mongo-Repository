using System;
using System.Collections.Generic;
using System.Linq;
using Mongo.Repository.Builder;

namespace Mongo.Repository
{
    public interface IMongoRepository
    {
        /// <summary>
        /// Inserts the specified document.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="document">The document.</param>
        void Insert<TDocument>(TDocument document)
          where TDocument : IDocument;

        /// <summary>
        /// Inserts the specified documents.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="documents">The documents.</param>
        void Insert<TDocument>(IEnumerable<TDocument> documents)
            where TDocument : IDocument;

        /// <summary>
        /// Queries this instance.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <returns></returns>
        IQueryable<TDocument> Query<TDocument>()
            where TDocument : IDocument;
        

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        TDocument Get<TDocument>(string id)
            where TDocument : IDocument;

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="id">The identifier.</param>
        void Delete<TDocument>(string id)
            where TDocument : IDocument;

        /// <summary>
        /// Deletes the specified filter.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="filter">The filter.</param>
        void Delete<TDocument>(Action<IFilterBuilder<TDocument>> filter)
            where TDocument : IDocument;

        /// <summary>
        /// Deletes the specified filter.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="filter">The filter.</param>
        void DeleteMany<TDocument>(Action<IFilterBuilder<TDocument>> filter)
            where TDocument : IDocument;

        /// <summary>
        /// Firsts this instance.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <returns></returns>
        TDocument First<TDocument>()
            where TDocument : IDocument;

        /// <summary>
        /// Lasts this instance.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <returns></returns>
        TDocument Last<TDocument>()
            where TDocument : IDocument;

        /// <summary>
        /// Updates the specified filter.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="filter">The filter.</param>
        /// <param name="updates">The updates.</param>
        bool Update<TDocument>(Action<IFilterBuilder<TDocument>> filter, params Action<IUpdateBuilder<TDocument>>[] updates)
            where TDocument : IDocument;

        /// <summary>
        /// Updates the many.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="filter">The filter.</param>
        /// <param name="updates">The updates.</param>
        bool UpdateMany<TDocument>(Action<IFilterBuilder<TDocument>> filter, params Action<IUpdateBuilder<TDocument>>[] updates)
            where TDocument : IDocument;

        /// <summary>
        /// Counts this instance.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <returns></returns>
        int Count<TDocument>()
            where TDocument : IDocument;

        /// <summary>
        /// Groups the specified projection.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="projection">The projection.</param>
        void Group<TDocument>(Action<IProjectionBuilder<TDocument>> projection)
            where TDocument : IDocument;
    }
}