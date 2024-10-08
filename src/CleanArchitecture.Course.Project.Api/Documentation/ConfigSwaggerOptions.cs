using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CleanArchitecture.Course.Project.Api.Documentation
{
    public sealed class ConfigSwaggerOptions(
        IApiVersionDescriptionProvider provider
    ) : IConfigureNamedOptions<SwaggerGenOptions>
    {

        private readonly IApiVersionDescriptionProvider _provider = provider;

        public void Configure(string? name, SwaggerGenOptions options)
        {
            Configure(options);
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    description.GroupName,
                    CreateInfoForApiVersion(description)
                );
            }
        }

        private static OpenApiInfo CreateInfoForApiVersion(
            ApiVersionDescription description
        )
        {
            var info = new OpenApiInfo
            {
                Title = $"Clean Architecture Course Project Api v{description.ApiVersion}",
                Version = description.ApiVersion.ToString()
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
        
    }
}