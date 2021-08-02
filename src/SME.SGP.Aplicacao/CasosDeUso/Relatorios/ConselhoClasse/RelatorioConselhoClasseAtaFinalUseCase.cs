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

        public RelatorioConselhoClasseAtaFinalUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(FiltroRelatorioConselhoClasseAtaFinalDto filtroRelatorioConselhoClasseAtaFinalDto)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            if (usuarioLogado == null)
                throw new NegocioException("Não foi possível localizar o usuário.");

            filtroRelatorioConselhoClasseAtaFinalDto.TurmasCodigos.RemoveAll(c => c == "-99");

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.ConselhoClasseAtaFinal, filtroRelatorioConselhoClasseAtaFinalDto, usuarioLogado, formato: filtroRelatorioConselhoClasseAtaFinalDto.TipoFormatoRelatorio));
        }
    }
}
