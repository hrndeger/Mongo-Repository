using System;
using System.Linq.Expressions;
using Mongo.Repository.Builder;
using MongoDB.Driver;

namespace Mongo.Repository.Version.Builder
{
    public class ProjectionBuilder<TDocument> : IProjectionBuilder<TDocument>
              where TDocument : IDocument
    {
        private ProjectionDefinition<TDocument> _projectionDefinition;


        /// <summary>
        /// Fors the specified expression.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public IProjectionBuilder<TDocument> Include<TProperty>(Expression<Func<TDocument>> expression)
        {
            var propertyName = ExpressionUtils.GetMemberName(expression);

            _projectionDefinition = Builders<TDocument>.Projection.Include(propertyName).Exclude(x => x._id);

            return this;
        }

        /// <summary>
        /// Gets the project definition.
        /// </summary>
        /// <value>
        /// The project definition.
        /// </value>
        public MongoDB.Driver.ProjectionDefinition<TDocument> ProjectDefinition => _projectionDefinition;
    }
}