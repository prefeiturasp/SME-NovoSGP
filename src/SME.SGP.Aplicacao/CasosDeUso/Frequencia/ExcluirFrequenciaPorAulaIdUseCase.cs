using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirFrequenciaPorAulaIdUseCase : AbstractUseCase, IExcluirFrequenciaPorAulaIdUseCase
    {
        public ExcluirFrequenciaPorAulaIdUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroIdDto>();

            await mediator.Send(new ExcluirFrequenciaDaAulaCommand(filtro.Id));

            var aula = await mediator.Send(new ObterAulaPorIdQuery(filtro.Id));
            
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(aula.TurmaId));
            
            await mediator.Send(new IncluirFilaConsolidacaoDiariaDashBoardFrequenciaCommand(turma.Id, aula.DataAula));
            
            await mediator.Send(new IncluirFilaConsolidacaoSemanalMensalDashBoardFrequenciaCommand(turma.Id, turma.CodigoTurma, turma.ModalidadeCodigo == Modalidade.EducacaoInfantil, turma.AnoLetivo, aula.DataAula));
            
            return true;
        }
    }
}
