namespace CirclesFundMe.API.Filters
{
    public class ApplySummariesOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                MethodInfo action = controllerActionDescriptor.MethodInfo;
                object[] attributes = action.GetCustomAttributes(true);

                if (attributes.FirstOrDefault(a => a is SwaggerOperationAttribute) is SwaggerOperationAttribute swaggerOperationAttribute)
                {
                    operation.Summary = swaggerOperationAttribute.Summary;
                }
            }
        }
    }
}
