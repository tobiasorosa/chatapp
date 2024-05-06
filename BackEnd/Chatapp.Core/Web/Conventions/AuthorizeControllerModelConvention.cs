using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace Chatapp.Core.Web.Conventions
{
    public class AuthorizeControllerModelConvention : IControllerModelConvention
    {
        private string DefaultPolicyName { get; }

        public AuthorizeControllerModelConvention(string defaultPolicyName)
        {
            DefaultPolicyName = defaultPolicyName;
        }

        public void Apply(ControllerModel controller)
        {
            foreach (var action in controller.Actions)
            {
                if (!action.Attributes.Any(x => x is AuthorizeAttribute))
                {
                    action.Filters.Add(new AuthorizeFilter(DefaultPolicyName));
                }
            }
        }
    }
}
