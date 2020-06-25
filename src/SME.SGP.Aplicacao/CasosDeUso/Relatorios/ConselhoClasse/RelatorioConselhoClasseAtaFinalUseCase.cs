using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RelatorioConselhoClasseAtaFinalUseCase : IRelatorioConselhoClasseAtaFinalUseCase
    {
        private readonly IMediator mediator;
        private readonly IServicoFila servicoFila;

        public RelatorioConselhoClasseAtaFinalUseCase(IMediator mediator, IServicoFila servicoFila)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoFila = servicoFila;
        }

        public async Task<bool> Executar(FiltroRelatorioConselhoClasseAtaFinalDto filtroRelatorioConselhoClasseAtaFinalDto)
        {
            var usuarioId = await mediator.Send(new ObterUsuarioLogadoIdQuery());

            if (usuarioId == 0)
                throw new NegocioException("Não foi possível localizar o usuário.");

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.ConselhoClasseAtaFinal, filtroRelatorioConselhoClasseAtaFinalDto, usuarioId));
        }
    }
}
