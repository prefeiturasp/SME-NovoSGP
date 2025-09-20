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

        [Fact(DisplayName = "Verificar se os Controllers que Herdam de  ControllerBase possuem permissionamento e authorize")]
        public Task Verificar_Se_Existe_Permissao_Ou_AllowAnonymou_Authorize_ControllerBase()
        {
            var listaApiMethod = new List<ApiMethodDto>();
            var assembly = Assembly.Load(AssemblyName);
            var apiControllers = assembly.DefinedTypes.Where(a => a.BaseType == typeof(ControllerBase));

            ObterDadosControllers(listaApiMethod, apiControllers, AssemblyName);

            var listMetodos = listaApiMethod.Where(x => x.CustomAttributeName.Count == 0);
            var semAuthorizeAttribute = listaApiMethod.Where(x => !x.Authorize && x.ControllerName != "AutenticacaoIntegracaoController" && x.ControllerName != "CEPController");
            var listAutorizecontrollerName = semAuthorizeAttribute.GroupBy(x => x.ControllerName).ToList();
            var listcontrollerName = listMetodos.GroupBy(c => c.ControllerName).ToList();

            Assert.True(semAuthorizeAttribute.Count() == 0, $"Existe {listAutorizecontrollerName.Count()} Controller(s) Sem AuthorizeAttribute ou ChaveIntegracaoSgpApi\n {string.Join("\n,", listAutorizecontrollerName.Select(c => c.First().ControllerName))}");
            Assert.True(listcontrollerName.Count == 0, $"{listMetodos.Count()} Método(s) em {listcontrollerName.Count} controller(s) sem permissionamento\n {string.Join("\n,", listcontrollerName.Select(c => c.First().ControllerName))}");
            return Task.CompletedTask;
        }
        //[Fact(DisplayName = "Verificar se os Controllers que Herdam de  Controller possuem permissionamento e authorize")]
        //public Task Verificar_Se_Existe_Permissao_Ou_AllowAnonymou_Authorize_Controller()
        //{
        //    var listaApiMethod = new List<ApiMethodDto>();
        //    var assembly = Assembly.Load(AssemblyName);
        //    var apiControllers = assembly.DefinedTypes.Where(a => a.BaseType == typeof(Controller));

        //    ObterDadosControllers(listaApiMethod, apiControllers, AssemblyName);

        //    var listMetodos = listaApiMethod.Where(x => x.CustomAttributeName.Count == 0);
        //    var semAuthorizeAttribute = listaApiMethod.Where(x => x.Authorize == false && x.ControllerName != "CacheController");
        //    var listAutorizecontrollerName = semAuthorizeAttribute.GroupBy(x => x.ControllerName).ToList();
        //    var listcontrollerName = listMetodos.GroupBy(c => c.ControllerName).ToList();

        //    Assert.True(semAuthorizeAttribute.Count() == 0, $"Existe {listAutorizecontrollerName.Count()} Controller(s) Sem AuthorizeAttribute ou ChaveIntegracaoSgpApi\n {string.Join("\n,", listAutorizecontrollerName.Select(c => c.First().ControllerName))}");
        //    Assert.True(listcontrollerName.Count == 0, $"{listMetodos.Count()} Método(s) em {listcontrollerName.Count} controller(s) sem permissionamento\n {string.Join("\n,", listcontrollerName.Select(c => c.First().ControllerName))}");
        //    return Task.CompletedTask;
        //}
        private static void ObterDadosControllers(List<ApiMethodDto> listaApiMethod, IEnumerable<TypeInfo> apiControllers, string assemblyName)
        {
            foreach (var item in apiControllers)
            {
                bool contemAutorize = false;
                var routePrefixAttrib = item.GetCustomAttributes(typeof(System.Web.Http.RoutePrefixAttribute)).FirstOrDefault() as System.Web.Http.RoutePrefixAttribute;

                var routePrefix = routePrefixAttrib?.Prefix ?? string.Empty;

                var metodos = item.GetMethods();


                if (item.CustomAttributes.Where(x => x.AttributeType.Name == "AuthorizeAttribute").Count() > 0 ||
                    item.CustomAttributes.Where(x => x.AttributeType.Name == "ChaveIntegracaoSgpApi").Count() > 0)
                {
                    contemAutorize = true;
                }

                foreach (var metodo in metodos)
                {
                    var customAttributeName = new List<string>();
                    if (!metodo.IsPublic || metodo.IsSpecialName || !metodo.Module.Name.StartsWith(assemblyName))
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
                        else if (contemAutorize && !customAttributeName.Any())
                            customAttributeName.Add("AuthorizeAttribute");

                    }

                    var apiMethod = new ApiMethodDto
                    {
                        Routes = routeSuffixes.Select(a => routePrefix + (routePrefix.Length == 0 ? string.Empty : "/") + a).ToList(),
                        ControllerName = item.Name,
                        MethodName = metodo.Name,
                        ParameterList = signature,
                        CustomAttributeName = customAttributeName,
                        HttpVerbo = httpVerb,
                        Authorize = contemAutorize
                    };

                    listaApiMethod.Add(apiMethod);

                }
            }
        }

        private static HttpVerbo ObterHttpMetodo(MethodInfo metodo)
        {
            var getAttrib = metodo.GetCustomAttributes(typeof(HttpGetAttribute)).FirstOrDefault();
            var postAttrib = metodo.GetCustomAttributes(typeof(HttpPostAttribute)).FirstOrDefault();
            var putAttrib = metodo.GetCustomAttributes(typeof(HttpPutAttribute)).FirstOrDefault();
            var deleteAttrib = metodo.GetCustomAttributes(typeof(HttpDeleteAttribute)).FirstOrDefault();
            var acceptVerbsAttrib = metodo.GetCustomAttributes(typeof(AcceptVerbsAttribute)).FirstOrDefault() as AcceptVerbsAttribute;

            var verbo = acceptVerbsAttrib.EhNulo() || acceptVerbsAttrib.HttpMethods.Count() == 0 ? HttpVerbo.GET
                                    : (HttpVerbo)Enum.Parse(typeof(HttpVerbo), acceptVerbsAttrib.HttpMethods.ToList()[0].ToUpperInvariant());


            var httpVerb = getAttrib.NaoEhNulo() ? HttpVerbo.GET : postAttrib.NaoEhNulo() ? HttpVerbo.POST : putAttrib.NaoEhNulo() ? HttpVerbo.PUT : deleteAttrib.NaoEhNulo() ? HttpVerbo.DELETE : verbo;
            return httpVerb;
        }
    }
}
