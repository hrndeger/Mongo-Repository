using System;
using System.Linq.Expressions;

namespace Mongo.Repository.Builder
{
    public interface IProjectionBuilder<TDocument>
               where TDocument : IDocument
    {
        /// <summary>
        /// Includes the specified expression.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        IProjectionBuilder<TDocument> Include<TProperty>(Expression<Func<TDocument>> expression);
    }
}