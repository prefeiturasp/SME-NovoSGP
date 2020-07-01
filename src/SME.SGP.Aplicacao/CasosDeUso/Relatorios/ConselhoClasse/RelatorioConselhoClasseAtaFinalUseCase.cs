using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Linq;
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
        public async Task<bool> Executar(FiltroRelatorioConselhoClasseAtaFinalDto filtroRelatorioConselhoClasseDto)
        {
            var usuarioId = await mediator.Send(new ObterUsuarioLogadoIdQuery());

            if (usuarioId == 0)
                throw new NegocioException("Não foi possível localizar o usuário.");

            filtroRelatorioConselhoClasseDto.TurmasCodigos.RemoveAll(c => c == "-99");

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.ConselhoClasseAtaFinal, filtroRelatorioConselhoClasseDto, usuarioId));
        }
    }
}
