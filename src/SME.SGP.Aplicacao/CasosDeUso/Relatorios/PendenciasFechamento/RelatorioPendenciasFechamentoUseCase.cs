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
            //await mediator.Send(new ValidaSeExisteTurmaPorCodigoQuery(filtroRelatorioPendenciasFechamentoDto.TurmaCodigo));
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            filtroRelatorioPendenciasFechamentoDto.UsuarioNome = usuarioLogado.Nome;
            filtroRelatorioPendenciasFechamentoDto.UsuarioRf = usuarioLogado.CodigoRf;

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.FechamentoPendencias, filtroRelatorioPendenciasFechamentoDto, usuarioLogado));
        }
    }
}
