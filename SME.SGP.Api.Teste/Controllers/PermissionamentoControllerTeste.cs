using Microsoft.AspNetCore.Mvc;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class PermissionamentoControllerTeste
    {
        public string AssemblyName { get; }
        public PermissionamentoControllerTeste()
        {
            AssemblyName = typeof(SME.SGP.Api.Program).Assembly.GetName().Name;
        }

        [Fact]
        public async Task Verificar_Se_Existe_Permissao_Ou_AllowAnonymou()
        {
            var listaApiMethod = new List<ApiMethodDto>();
            var assembly = Assembly.Load(AssemblyName);
            var apiControllers = assembly.DefinedTypes.Where(a => a.BaseType == typeof(ControllerBase));

            foreach (var item in apiControllers)
            {
                var customAttributeName = new List<string>();
                var routePrefixAttrib = item.GetCustomAttributes(typeof(System.Web.Http.RoutePrefixAttribute)).FirstOrDefault() as System.Web.Http.RoutePrefixAttribute;

                var routePrefix = routePrefixAttrib?.Prefix ?? string.Empty;

                var metodos = item.GetMethods();

                foreach (var metodo in metodos)
                {
                    if (!metodo.IsPublic)
                        continue;

                    var httpVerb = ObterHttpMetodo(metodo);

                    var routeAttribs = metodo.GetCustomAttributes(typeof(System.Web.Http.RouteAttribute));

                    var routeSuffixes = routeAttribs.Select(a => (a as RouteAttribute)?.Template).Where(a => !string.IsNullOrWhiteSpace(a)).ToList();

                    var parameters = metodo.GetParameters();

                    var signature = string.Join(", ", parameters.Select(a => a.ParameterType.Name).ToArray<string>());

                    foreach (var customAttribute in metodo.CustomAttributes)
                    {
                        if (customAttribute.AttributeType.Name.Equals("AllowAnonymousAttribute") || customAttribute.AttributeType.Name.Equals("PermissaoAttribute")
                                || customAttribute.AttributeType.Name.Equals("ChaveIntegracaoSgpApi"))
                            customAttributeName.Add(customAttribute.AttributeType.Name);
                    }

                    var apiMethod = new ApiMethodDto
                    {
                        Routes = routeSuffixes.Select(a => routePrefix + (routePrefix.Length == 0 ? string.Empty : "/") + a).ToList(),
                        ControllerName = item.Name,
                        MethodName = metodo.Name,
                        ParameterList = signature,
                        CustomAttributeName = customAttributeName,
                        HttpVerbo = httpVerb,
                    };

                    listaApiMethod.Add(apiMethod);

                }
            }

            var listMetodos = listaApiMethod.Where(x => x.CustomAttributeName.Count == 0);
            var listcontrollerName = listMetodos.GroupBy(c => c.ControllerName).ToList();

            Assert.True(listMetodos.Count() == 0,$"{listMetodos.Count()} Métodos em {listcontrollerName.Count} controllers não possuem permissionamento\n {string.Join("\n,", listcontrollerName.Select(c => c.First().ControllerName))}");
        }

        private static HttpVerbo ObterHttpMetodo(MethodInfo metodo)
        {
            var getAttrib = metodo.GetCustomAttributes(typeof(System.Web.Http.HttpGetAttribute)).FirstOrDefault();
            var postAttrib = metodo.GetCustomAttributes(typeof(System.Web.Http.HttpPostAttribute)).FirstOrDefault();
            var putAttrib = metodo.GetCustomAttributes(typeof(System.Web.Http.HttpPutAttribute)).FirstOrDefault();
            var deleteAttrib = metodo.GetCustomAttributes(typeof(System.Web.Http.HttpDeleteAttribute)).FirstOrDefault();
            var acceptVerbsAttrib = metodo.GetCustomAttributes(typeof(System.Web.Http.AcceptVerbsAttribute)).FirstOrDefault() as System.Web.Http.AcceptVerbsAttribute;

            var verbo = acceptVerbsAttrib == null || acceptVerbsAttrib.HttpMethods.Count == 0 ? HttpVerbo.GET
                                    : (HttpVerbo)Enum.Parse(typeof(HttpVerbo), acceptVerbsAttrib.HttpMethods[0].Method.ToUpperInvariant());


            var httpVerb = getAttrib != null ? HttpVerbo.GET : postAttrib != null ? HttpVerbo.POST : putAttrib != null ? HttpVerbo.PUT : deleteAttrib != null ? HttpVerbo.DELETE : verbo;
            return httpVerb;
        }
    }
}
