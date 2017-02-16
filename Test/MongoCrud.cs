using Mongo.Repository;
using Mongo.Repository.Version.Repository;
using Test;

namespace Example
{
    public class MongoCrud : IMongoCrud
    {
        private readonly IMongoRepository _mongoRepository;
        public MongoCrud()
        {
            _mongoRepository = new MongoRepository();
        }

        public void Insert(Customer customer)
        {
            _mongoRepository.Insert(customer);
        }        
    }
}