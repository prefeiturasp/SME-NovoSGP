using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaUeUseCase : AbstractUseCase, IExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaUeUseCase
    {
        public ExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaUeUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<FiltroIdAnoLetivoDto>();
            //var ue = await mediator.Send(new ObterCodigoUEDREPorIdQuery(filtro.Id));
            if (filtro.Data.Day.Equals(DateTime.DaysInMonth(filtro.Data.Year, filtro.Data.Month))){
                var consolidacoesFrequenciaMensalInsuficientes = await mediator.Send(new ObterConsolidacoesFrequenciaAlunoMensalInsuficientesQuery(filtro.Id, filtro.Data.Year, filtro.Data.Month));
                if (consolidacoesFrequenciaMensalInsuficientes.PossuiRegistros())
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.ExecutarNotificacaoAlunosBaixaFrequenciaBuscaAtivaProfissionaisNAAPA, consolidacoesFrequenciaMensalInsuficientes, 
                                                                   Guid.NewGuid()));
            }
            return true;
        }
    }
}
