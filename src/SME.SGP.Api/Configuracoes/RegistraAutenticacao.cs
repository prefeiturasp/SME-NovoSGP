using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using static System.Text.Encoding;

namespace SME.SGP.Api.Configuracoes
{
    public static class RegistraAutenticacao
    {
        public static void Registrar(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.Authority = "https://SEU-KC/realms/SEU_REALM";
                o.Audience = "SEU_CLIENT_ID";
                o.RequireHttpsMetadata = true;

                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    NameClaimType = "preferred_username",
                    RoleClaimType = "roles"
                };
                //o.TokenValidationParameters = new TokenValidationParameters
                //{
                //    ValidateLifetime = true,
                //    ValidateAudience = true,
                //    ValidAudience = configuration.GetValue<string>("JwtTokenSettings:Audience"),
                //    ValidateIssuer = true,
                //    ValidIssuer = configuration.GetValue<string>("JwtTokenSettings:Issuer"),
                //    ValidateIssuerSigningKey = true,
                //    ClockSkew = TimeSpan.Zero,
                //    IssuerSigningKey = new SymmetricSecurityKey(UTF8
                //        .GetBytes(configuration.GetValue<string>("JwtTokenSettings:IssuerSigningKey")))
                //};
            });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()

                   .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                   .RequireAuthenticatedUser()
                   .Build());
            });
        }
    }
}