using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SME.SGP.Infra;

namespace SME.SGP.Api.Middlewares
{
    public class ResultadoBaseResult : ObjectResult
    {
        public ResultadoBaseResult(string mensagem)
            : base(RetornaBaseModel(mensagem))
        {
            StatusCode = 601;
        }

        public ResultadoBaseResult(string mensagem, int statusCode)
            : base(RetornaBaseModel(mensagem))
        {
            StatusCode = statusCode;
        }

        public static RetornoBaseDto RetornaBaseModel(string mensagem)
        {
            return new RetornoBaseDto(mensagem);
        }
    }
}
