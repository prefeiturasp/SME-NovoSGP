using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarAulaFrequenciaTratarUseCase : AbstractUseCase, IAlterarAulaFrequenciaTratarUseCase
    {
        public AlterarAulaFrequenciaTratarUseCase(IMediator mediator) : base(mediator)
        {

        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<AulaAlterarFrequenciaRequestDto>();

            var aulaParaTratar = await mediator.Send(new ObterAulaPorIdQuery(filtro.AulaId));
            if (aulaParaTratar.NaoEhNulo())
            {
                var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(aulaParaTratar.TurmaId));
                
                await mediator.Send(new AlterarAulaFrequenciaTratarCommand(aulaParaTratar, filtro.QuantidadeAnterior));
                
                await mediator.Send(new RecalcularFrequenciaPorTurmaCommand(aulaParaTratar.TurmaId, aulaParaTratar.DisciplinaId, aulaParaTratar.Id));
                
                await mediator.Send(new IncluirFilaConsolidacaoDiariaDashBoardFrequenciaCommand(turma.Id, aulaParaTratar.DataAula));
                
                await mediator.Send(new IncluirFilaConsolidacaoSemanalMensalDashBoardFrequenciaCommand(turma.Id, turma.CodigoTurma, turma.ModalidadeCodigo == Modalidade.EducacaoInfantil, turma.AnoLetivo, aulaParaTratar.DataAula));
            }

            return true;
        }
    }
}
