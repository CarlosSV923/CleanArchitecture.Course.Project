using System.Data;

namespace CleanArchitecture.Course.Project.Application.Abstractions.Data
{
    public interface ISqlConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
