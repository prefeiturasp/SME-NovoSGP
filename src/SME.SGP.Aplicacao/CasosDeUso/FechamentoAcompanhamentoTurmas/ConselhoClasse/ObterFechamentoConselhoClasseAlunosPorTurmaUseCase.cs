using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoConselhoClasseAlunosPorTurmaUseCase : AbstractUseCase, IObterFechamentoConselhoClasseAlunosPorTurmaUseCase
    {
        public ObterFechamentoConselhoClasseAlunosPorTurmaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<ConselhoClasseAlunoDto>> Executar(FiltroConselhoClasseConsolidadoTurmaBimestreDto param)
        {
            var lista = await mediator.Send(new ObterAlunosEStatusConselhoClasseConsolidadoPorTurmaEbimestreQuery(param.TurmaId, param.Bimestre, param.SituacaoConselhoClasse));
            return lista;
        }
 
    }
}
