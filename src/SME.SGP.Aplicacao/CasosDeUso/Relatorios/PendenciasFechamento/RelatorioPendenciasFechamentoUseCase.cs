using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.Relatorios;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class RelatorioPendenciasFechamentoUseCase : IRelatorioPendenciasFechamentoUseCase
    {
        private readonly IMediator mediator;

        public RelatorioPendenciasFechamentoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(FiltroRelatorioPendenciasFechamentoDto filtroRelatorioPendenciasFechamentoDto)
        {
            await mediator.Send(new ValidaSeExisteDrePorCodigoQuery(filtroRelatorioPendenciasFechamentoDto.DreCodigo));
            await mediator.Send(new ValidaSeExisteUePorCodigoQuery(filtroRelatorioPendenciasFechamentoDto.UeCodigo));
            if (filtroRelatorioPendenciasFechamentoDto.TurmasCodigo.Length > 0)
            {
                foreach (string codigo in filtroRelatorioPendenciasFechamentoDto.TurmasCodigo)
                {
                    await mediator.Send(new ValidaSeExisteTurmaPorCodigoQuery(codigo));
                }
            }
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.PendenciasFechamento, filtroRelatorioPendenciasFechamentoDto, usuarioLogado));
        }
    }
}
