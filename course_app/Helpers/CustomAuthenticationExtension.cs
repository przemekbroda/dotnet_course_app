using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace course_app.Helpers
{
    public static class CustomAuthenticationExtention
    {
        public static AuthenticationBuilder AddCustomAuthentication(this AuthenticationBuilder authenticationBuilder, Action<CustomAuthOptions> options)
        {
           return authenticationBuilder.AddScheme<CustomAuthOptions, CustomAuthHandler>("custom auth", "custom auth", options);
        }
    }
}
