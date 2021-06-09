using MediatR;
using SME.SGP.Infra;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasFechamentoAcompanhamentoUseCase : AbstractUseCase, IObterTurmasFechamentoAcompanhamentoUseCase
    {
        public ObterTurmasFechamentoAcompanhamentoUseCase(IMediator mediator) : base(mediator)
        {
        }
        public async Task<PaginacaoResultadoDto<TurmaAcompanhamentoFechamentoRetornoDto>> Executar(FiltroAcompanhamentoFechamentoTurmasDto param)
        {
            var listarTodasTurmas = param.TurmasId.Any(c => c == -99);
            var turmas = await mediator.Send(new ObterTurmasFechamentoAcompanhamentoQuery(param.DreId,
                                                                                          param.UeId,
                                                                                          param.TurmasId,
                                                                                          param.Modalidade,
                                                                                          param.Semestre,
                                                                                          param.Bimestre,
                                                                                          param.AnoLetivo,
                                                                                          param.SituacaoFechamento,
                                                                                          param.SituacaoConselhoClasse,
                                                                                          listarTodasTurmas));
            return turmas;
        }
    }
}
