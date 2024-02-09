using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidarReflexoFrequenciaBuscaAtivaUeUseCase : AbstractUseCase, IConsolidarReflexoFrequenciaBuscaAtivaUeUseCase
    {
        private readonly IRepositorioRegistroAcaoBuscaAtiva repositorioBuscaAtiva;
        private const int ANUAL = 0;

        public ConsolidarReflexoFrequenciaBuscaAtivaUeUseCase(IMediator mediator, IRepositorioRegistroAcaoBuscaAtiva repositorioBuscaAtiva) : base(mediator)
        {
            this.repositorioBuscaAtiva = repositorioBuscaAtiva ?? throw new System.ArgumentNullException(nameof(repositorioBuscaAtiva)); 
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroIdAnoLetivoDto>();
            var ue = await mediator.Send(new ObterCodigoUEDREPorIdQuery(filtro.Id));
            await mediator.Send(new ExcluirConsolidacoesReflexoFrequenciaBuscaAtivaUeMesCommand(ue.UeCodigo, filtro.Data.Month, filtro.Data.Year));
            if (filtro.Data.Month == DateTimeExtension.HorarioBrasilia().Month)
                await mediator.Send(new ExcluirConsolidacoesReflexoFrequenciaBuscaAtivaUeMesCommand(ue.UeCodigo, ANUAL, filtro.Data.Year));

            var registrosBuscaAtiva = await repositorioBuscaAtiva.ObterIdsRegistrosAlunoResponsavelContatado(filtro.Data.Date, filtro.Id, filtro.Data.Year);
            foreach (var idBuscaAtiva in registrosBuscaAtiva)
            {
                filtro.Id = idBuscaAtiva;
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.ConsolidarReflexoFrequenciaBuscaAtivaAluno, filtro, Guid.NewGuid()));
            }
            return true;
        }
    }
}
