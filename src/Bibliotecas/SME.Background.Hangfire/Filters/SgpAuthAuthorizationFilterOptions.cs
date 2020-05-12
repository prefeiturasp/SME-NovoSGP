using Hangfire.Dashboard.BasicAuthorization;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.Background.Hangfire
{
    public class SgpAuthAuthorizationFilterOptions : BasicAuthAuthorizationFilterOptions
    {
        public static string AdminUser;

        public SgpAuthAuthorizationFilterOptions(IConfiguration configuration)
        {
            // Carrega e valida variáveis de ambiente
            var configUserAdminStr = configuration.GetSection("HangfireUser_Admin").Value;
            if (string.IsNullOrEmpty(configUserAdminStr))
                throw new ArgumentNullException("HangfireUser_Admin", "Não localizado variável de ambiente do usuario Admin do Hangfire!");

            var configUserBasicStr = configuration.GetSection("HangfireUser_Basic").Value;
            if (string.IsNullOrEmpty(configUserBasicStr))
                throw new ArgumentNullException("HangfireUser_Basic", "Não localizado variável de ambiente do usuario Básico do Hangfire!");

            // Separa usuario e senha
            var configUserAdmin = configUserAdminStr.Split(':');
            var configUserBasic = configUserBasicStr.Split(':');
            AdminUser = configUserAdmin[0];

            RequireSsl = false;
            SslRedirect = false;
            LoginCaseSensitive = true;
            Users = new[]
            {
                new BasicAuthAuthorizationUser
                {
                    Login = configUserAdmin[0],
                    PasswordClear = configUserAdmin[1]
                },
                new BasicAuthAuthorizationUser
                {
                    Login = configUserBasic[0],
                    PasswordClear =  configUserBasic[1]
                },
            };
        }
    }
}
