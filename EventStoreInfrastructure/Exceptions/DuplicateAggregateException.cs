using System;

namespace EventStoreInfrastructure.Exceptions
{
    public class DuplicateAggregateException : Exception
    {
        public DuplicateAggregateException(Guid id) : base(CreateMessage(id))
        {
        }

        private static string CreateMessage(Guid id)
        {
            return $"Aggregate already exists with id {id}";
        }
    }
}