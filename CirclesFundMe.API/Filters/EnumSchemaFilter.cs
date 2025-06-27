namespace CirclesFundMe.API.Filters
{
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                schema.Enum.Clear();
                Enum.GetNames(context.Type)
                    .OrderBy(n => n)
                    .ToList()
                    .ForEach(name => schema.Enum.Add(new OpenApiString(name)));

                schema.Type = "string";
                schema.Format = null;

                Dictionary<string, string> descriptions = [];

                foreach (object value in Enum.GetValues(context.Type))
                {
                    FieldInfo? field = context.Type.GetField(value.ToString() ?? "");
                    DescriptionAttribute? descriptionAttribute = field?.GetCustomAttribute<DescriptionAttribute>();
                    if (descriptionAttribute != null)
                    {
                        descriptions.Add(value.ToString() ?? "", descriptionAttribute.Description);
                    }
                }
            }
        }
    }
}
