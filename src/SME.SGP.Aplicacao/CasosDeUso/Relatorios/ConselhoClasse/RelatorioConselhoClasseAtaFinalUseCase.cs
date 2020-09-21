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
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            if (usuarioLogado == null)
                throw new NegocioException("Não foi possível localizar o usuário.");

            if (filtroRelatorioConselhoClasseAtaFinalDto.TipoFormatoRelatorio == TipoFormatoRelatorio.Xlsx &&
                filtroRelatorioConselhoClasseAtaFinalDto.TurmasCodigos.Count > 1)
                throw new NegocioException("Não é possível emitir este relatório para este formato para mais de uma turma.");

            filtroRelatorioConselhoClasseAtaFinalDto.TurmasCodigos.RemoveAll(c => c == "-99");

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.ConselhoClasseAtaFinal, filtroRelatorioConselhoClasseAtaFinalDto, usuarioLogado, filtroRelatorioConselhoClasseAtaFinalDto.TipoFormatoRelatorio));
        }
    }
}
