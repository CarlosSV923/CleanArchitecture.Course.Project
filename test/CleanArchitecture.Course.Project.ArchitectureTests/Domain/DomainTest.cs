using System.Reflection;
using CleanArchitecture.Course.Project.ArchitectureTests.Infrastructure;
using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;
using FluentAssertions;
using NetArchTest.Rules;
using Xunit;

namespace CleanArchitecture.Course.Project.ArchitectureTests.Domain
{
    public class DomainTest : BaseTest
    {

        [Fact]
        public void Entities_ShouldHave_PrivateConstructorNoParameters()
        {
            IEnumerable<Type> entities = Types.InAssembly(DomainAssembly)
                .That()
                .Inherit(typeof(Entity<>))
                .GetTypes();
            
            List<Type> errorEntities = [];

            foreach (var entity in entities)
            {
                ConstructorInfo[] constructors = entity.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);

                if(!Array.Exists(constructors, c => c.IsPrivate && c.GetParameters().Length == 0))
                {
                    errorEntities.Add(entity);
                }
            }

            errorEntities.Should().BeEmpty();
        }


    }
}