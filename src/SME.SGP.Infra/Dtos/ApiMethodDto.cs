using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class ApiMethodDto
    {
        public List<string> Routes { get; set; }
        public string ControllerName { get; set; }
        public string MethodName { get; set; }
        public string ParameterList { get; set; }
        public List<string> CustomAttributeName { get; set; }
        public HttpVerbo HttpVerbo { get; set; }
        public bool Authorize { get; set; }
    }
}
