using Microsoft.AspNetCore.Http;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class HttpContextAccessorFake : IHttpContextAccessor
    {
        public HttpContext HttpContext { get; set; }

        public HttpContextAccessorFake()
        {
            HttpContext = new DefaultHttpContext();
        }
    }
}