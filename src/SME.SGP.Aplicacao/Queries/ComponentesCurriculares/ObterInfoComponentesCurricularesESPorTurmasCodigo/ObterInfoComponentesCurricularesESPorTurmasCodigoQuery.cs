using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;


namespace SME.SGP.Aplicacao
{
    public class ObterInfoComponentesCurricularesESPorTurmasCodigoQuery : IRequest<IEnumerable<InfoComponenteCurricular>>
    {
        public ObterInfoComponentesCurricularesESPorTurmasCodigoQuery(string[] codigosDeTurmas)
        {
            CodigosDeTurmas = codigosDeTurmas;
        }
        public string[] CodigosDeTurmas { get; set; }
    }
}
