using System;
using Mongo.Repository;

namespace Example
{
    public class Customer : IDocument
    {
        public string Name { get; set; }
        public object _id { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}