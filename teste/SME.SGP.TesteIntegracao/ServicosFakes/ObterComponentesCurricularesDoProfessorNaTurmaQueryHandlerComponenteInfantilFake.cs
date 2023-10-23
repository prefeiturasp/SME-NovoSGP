using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ObterComponentesCurricularesDoProfessorNaTurmaQueryHandlerComponenteInfantilFake : IRequestHandler<ObterComponentesCurricularesDoProfessorNaTurmaQuery, IEnumerable<ComponenteCurricularEol>>
    {
        public async Task<IEnumerable<ComponenteCurricularEol>> Handle(ObterComponentesCurricularesDoProfessorNaTurmaQuery request, CancellationToken cancellationToken)
        {
            if (!request.RealizarAgrupamentoComponente)
                throw new NegocioException("Para infantil é necessário realizar o agrupamento.");

            return await Task.FromResult(new List<ComponenteCurricularEol>()
            {
                new ComponenteCurricularEol() { Codigo = 1, Regencia = true }
            });
        }
    }
}
