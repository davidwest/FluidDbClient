using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Reflection;

namespace FluidDbClient.Sql.Test
{
    public static class EfConfigurationExtensions
    {
        public static void AddAllTypeConfigurations(this DbModelBuilder modelBuilder)
        {
            var typesToRegister =
                Assembly.GetAssembly(typeof(DataContext)).GetTypes()
                    .Where(type => type.BaseType != null && type.BaseType.IsGenericType
                                                         && (type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>) ||
                                                             type.BaseType.GetGenericTypeDefinition() == typeof(ComplexTypeConfiguration<>)));

            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }
        }

        public static PrimitivePropertyConfiguration HasUniqueIndexAnnotation(
            this PrimitivePropertyConfiguration property,
            string indexName,
            int columnOrder = 0)
        {
            var indexAttribute = new IndexAttribute(indexName, columnOrder) { IsUnique = true };
            var indexAnnotation = new IndexAnnotation(indexAttribute);

            return property.HasColumnAnnotation(IndexAnnotation.AnnotationName, indexAnnotation);
        }
    }
}
