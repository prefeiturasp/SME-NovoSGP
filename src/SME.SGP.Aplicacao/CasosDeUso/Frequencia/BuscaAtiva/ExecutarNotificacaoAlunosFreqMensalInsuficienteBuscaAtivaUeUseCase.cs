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
            var ue = await mediator.Send(new ObterCodigoUEDREPorIdQuery(filtro.Id));
            
            /*var registrosBuscaAtiva = await repositorioBuscaAtiva.ObterIdsRegistrosAlunoResponsavelContatado(filtro.Data.Date, filtro.Id, filtro.Data.Year);
            foreach (var idBuscaAtiva in registrosBuscaAtiva)
            {
                filtro.Id = idBuscaAtiva;
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.ConsolidarReflexoFrequenciaBuscaAtivaAluno, filtro, Guid.NewGuid()));
            }*/
            return true;
        }
    }
}
