using Microsoft.EntityFrameworkCore;
using Persistence;

namespace UnitTests.PersistenceTests.Context
{
    public class SqlContextFactory
    {
        public SqlContext CreateMemoryContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<SqlContext>();
            optionsBuilder.UseInMemoryDatabase("TestDatabase");

            return new SqlContext(optionsBuilder.Options);
        }
        
        public void DisposeContext(SqlContext context)
        {
            context.Dispose();
        }
    }
}