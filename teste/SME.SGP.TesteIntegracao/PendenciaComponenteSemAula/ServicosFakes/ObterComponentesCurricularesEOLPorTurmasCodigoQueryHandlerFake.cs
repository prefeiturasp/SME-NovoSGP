using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.PendenciaComponenteSemAula.ServicosFakes
{
    public class ObterComponentesCurricularesEOLPorTurmasCodigoQueryHandlerFake : IRequestHandler<ObterComponentesCurricularesEOLPorTurmasCodigoQuery, IEnumerable<ComponenteCurricularEol>>
    {
        public async Task<IEnumerable<ComponenteCurricularEol>> Handle(ObterComponentesCurricularesEOLPorTurmasCodigoQuery request, CancellationToken cancellationToken)
        {
            var retorno = new List<ComponenteCurricularEol>();

            if (request.CodigosDeTurmas.Contains("2"))
                retorno.Add(new ComponenteCurricularEol { Codigo = 1114, Descricao = "REGENCIA_CLASSE_EJA_BASICA", Regencia = true });
            else
                retorno.Add(new ComponenteCurricularEol { Codigo = 138, Descricao = "Lingua Portuguesa", Regencia = false });

            return await Task.FromResult(retorno);
        }
    }
}
