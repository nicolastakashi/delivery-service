using DeliveryService.Infra.Attributes;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Reflection;

namespace DeliveryService.Infra.Api.Filters
{
    public class SwaggerExcludeFilter : ISchemaFilter
    {
        public void Apply(Schema schema, SchemaFilterContext context)
        {
            var type = context.SystemType;
            if (schema?.Properties == null || type == null) return;

            var excludedProperties = type.GetProperties().Where(t => t.GetCustomAttribute<SwaggerExcludeAttribute>() != null);

            foreach (var excludedProperty in excludedProperties)
            {
                if (schema.Properties.ContainsKey(excludedProperty.Name))
                {
                    schema.Properties.Remove(excludedProperty.Name);
                }
            }
        }
    }
}
