using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos.Relatorios;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class RelatorioPendenciasFechamentoUseCase : IRelatorioPendenciasFechamentoUseCase
    {
        private readonly IMediator mediator;
        private readonly IRepositorioDre repositorioDre;
        private readonly IRepositorioUe repositorioUe;
        private readonly IRepositorioTurma repositorioTurma;

        public RelatorioPendenciasFechamentoUseCase(IMediator mediator,
                              IRepositorioUe repositorioUe,
                              IRepositorioDre repositorioDre,
                              IRepositorioTurma repositorioTurma)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
            this.repositorioDre = repositorioDre ?? throw new ArgumentNullException(nameof(repositorioDre));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<bool> Executar(FiltroRelatorioPendenciasFechamentoDto filtroRelatorioPendenciasFechamentoDto)
        {
            if (repositorioDre.ObterPorCodigo(filtroRelatorioPendenciasFechamentoDto.DreCodigo) == null)
                throw new NegocioException("Não foi possível encontrar a DRE");

            if (repositorioUe.ObterPorCodigo(filtroRelatorioPendenciasFechamentoDto.UeCodigo) == null)
                throw new NegocioException("Não foi possível encontrar a UE");

            if (!string.IsNullOrEmpty(filtroRelatorioPendenciasFechamentoDto.TurmaCodigo))
            {
                int codigoTurma;
                if (int.TryParse(filtroRelatorioPendenciasFechamentoDto.TurmaCodigo, out codigoTurma) && codigoTurma <= 0)
                    filtroRelatorioPendenciasFechamentoDto.TurmaCodigo = String.Empty;
                else if (await repositorioTurma.ObterPorCodigo(filtroRelatorioPendenciasFechamentoDto.TurmaCodigo) == null)
                    throw new NegocioException("Não foi possível encontrar a turma");
            }

            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.PendenciasFechamento, filtroRelatorioPendenciasFechamentoDto, usuarioLogado));

        }
    }
}
