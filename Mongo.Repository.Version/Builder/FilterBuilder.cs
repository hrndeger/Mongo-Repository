using System;
using System.Linq.Expressions;
using Mongo.Repository.Builder;
using MongoDB.Driver;

namespace Mongo.Repository.Version.Builder
{
    public class FilterBuilder<TDocument> : IFilterBuilder<TDocument>
        where TDocument : IDocument
    {
        private MongoDB.Driver.FilterDefinition<TDocument> _filter;

        /// <summary>
        /// Gets the filter.
        /// </summary>
        /// <value>
        /// The filter.
        /// </value>
        public MongoDB.Driver.FilterDefinition<TDocument> Filter => _filter;

        /// <summary>
        /// Fors the specified expression.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="param">The parameter.</param>
        /// <returns></returns>
        public IFilterBuilder<TDocument> For<TProperty>(Expression<Func<TDocument, TProperty>> expression, TProperty param)
        {
            var propertyName = ExpressionUtils.GetMemberName(expression);
            _filter = Builders<TDocument>.Filter.Eq(propertyName, param);

            return this;
        }
    }
}