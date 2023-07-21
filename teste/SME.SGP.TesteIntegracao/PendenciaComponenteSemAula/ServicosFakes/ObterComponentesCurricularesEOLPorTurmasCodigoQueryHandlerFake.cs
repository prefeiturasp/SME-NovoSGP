using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.PendenciaComponenteSemAula.ServicosFakes
{
    public class ObterComponentesCurricularesEOLPorTurmasCodigoQueryHandlerFake : IRequestHandler<ObterComponentesCurricularesEOLPorTurmasCodigoQuery, IEnumerable<ComponenteCurricularDto>>
    {
        public async Task<IEnumerable<ComponenteCurricularDto>> Handle(ObterComponentesCurricularesEOLPorTurmasCodigoQuery request, CancellationToken cancellationToken)
        {
            var retorno = new List<ComponenteCurricularDto>();

            if (request.CodigosDeTurmas.Contains("1"))
                retorno.Add(new ComponenteCurricularDto { Codigo = "138", Descricao = "Lingua Portuguesa", Regencia = false });
            else
                retorno.Add(new ComponenteCurricularDto { Codigo = "1114", Descricao = "REGENCIA_CLASSE_EJA_BASICA", Regencia = true });

            return await Task.FromResult(retorno);
        }
    }
}
