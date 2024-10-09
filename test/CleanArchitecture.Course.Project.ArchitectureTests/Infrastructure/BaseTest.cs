using System.Reflection;
using CleanArchitecture.Course.Project.Application.Abstractions.Messaging;
using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;
using CleanArchitecture.Course.Project.Infrastructure;

namespace CleanArchitecture.Course.Project.ArchitectureTests.Infrastructure
{
    public abstract class BaseTest
    {
        protected static readonly Assembly ApplicationAssembly = typeof(IBaseCommand).Assembly;
        protected static readonly Assembly DomainAssembly = typeof(IEntity).Assembly;
        protected static readonly Assembly InfrastructureAssembly = typeof(ApplicationDbContext).Assembly;
        protected static readonly Assembly PresentationAssembly = typeof(Program).Assembly;
        
        
    }
}