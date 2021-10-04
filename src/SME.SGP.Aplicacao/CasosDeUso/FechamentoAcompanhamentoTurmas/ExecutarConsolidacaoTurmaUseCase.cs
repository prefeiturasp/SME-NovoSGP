using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarConsolidacaoTurmaUseCase : AbstractUseCase, IExecutarConsolidacaoTurmaUseCase
    {
        public ExecutarConsolidacaoTurmaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var consolidacaoTurma = mensagemRabbit.ObterObjetoMensagem<ConsolidacaoTurmaDto>();

            if (consolidacaoTurma == null)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand("Não foi possível iniciar a consolidação das turmas. O id da turma e o bimestre não foram informados.", LogNivel.Negocio, LogContexto.Turma));
                return false;
            }

            if (consolidacaoTurma.TurmaId == 0)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand("Não foi possível iniciar a consolidação das turmas. O id da turma não foi informado.", LogNivel.Negocio, LogContexto.Turma));                
                return false;
            }

            var mensagemParaPublicar = JsonConvert.SerializeObject(consolidacaoTurma);
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidarTurmaConselhoClasseSync, mensagemParaPublicar, mensagemRabbit.CodigoCorrelacao, null));
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidarTurmaFechamentoSync, mensagemParaPublicar, mensagemRabbit.CodigoCorrelacao, null));

            return true;
        }
    }
}
