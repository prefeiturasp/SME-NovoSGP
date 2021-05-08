using MediatR;
using SME.SGP.Infra;
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

            var turmas = await mediator.Send(new ObterTurmasFechamentoAcompanhamentoQuery(param.DreCodigo,
                                                                                          param.UeCodigo,
                                                                                          param.TurmaId,
                                                                                          param.Modalidade,
                                                                                          param.Semestre,
                                                                                          param.Bimestre,
                                                                                          param.AnoLetivo));
            return turmas;
        }
    }
}
