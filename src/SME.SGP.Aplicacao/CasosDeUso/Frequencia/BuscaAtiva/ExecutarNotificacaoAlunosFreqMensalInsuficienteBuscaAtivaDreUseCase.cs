using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaDreUseCase : AbstractUseCase, IExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaDreUseCase
    {
        public ExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaDreUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<FiltroIdAnoLetivoDto>();
            var ues = await mediator.Send(new ObterUEsIdsPorDreQuery(filtro.Id));
            foreach (long UeId in ues)
            {
                filtro.Id = UeId;
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.ExecutarNotificacaoAlunosBaixaFrequenciaBuscaAtivaUe, filtro, Guid.NewGuid()));
            }
            return true;
        }
    }
}
