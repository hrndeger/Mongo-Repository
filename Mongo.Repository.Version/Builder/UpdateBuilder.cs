using System;
using System.Linq.Expressions;
using Mongo.Repository.Builder;
using MongoDB.Driver;

namespace Mongo.Repository.Version.Builder
{
    public class UpdateBuilder<TDocument> : IUpdateBuilder<TDocument>
                where TDocument : IDocument
    {
        private UpdateDefinition<TDocument> _updateDefinition;

        /// <summary>
        /// Fors the specified expression.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="value">The value.</param>
        public IUpdateBuilder<TDocument> Set<TProperty>(Expression<Func<TDocument, TProperty>> expression, TProperty value)
        {
            var propertyName = ExpressionUtils.GetMemberName(expression);

            _updateDefinition = Builders<TDocument>.Update.Set(propertyName, value);

            return this;
        }

        /// <summary>
        /// Sets the with current date.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public IUpdateBuilder<TDocument> SetWithCurrentDate<TProperty>(Expression<Func<TDocument, TProperty>> expression, TProperty value)
        {
            var propertyName = ExpressionUtils.GetMemberName(expression);

            _updateDefinition = Builders<TDocument>.Update.Set(propertyName, value).CurrentDate(i => i.ModifiedOn);

            return this;
        }

        /// <summary>
        /// Gets the update definition.
        /// </summary>
        /// <value>
        /// The update definition.
        /// </value>
        public MongoDB.Driver.UpdateDefinition<TDocument> UpdateDefinition => _updateDefinition;
    }
}