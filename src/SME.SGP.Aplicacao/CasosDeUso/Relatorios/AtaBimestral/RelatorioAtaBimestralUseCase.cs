using System;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Relatorios;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class RelatorioAtaBimestralUseCase : IRelatorioAtaBimestralUseCase
    {
        private readonly IMediator mediator;

        public RelatorioAtaBimestralUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(FiltroRelatorioAtaBimestralDto filtro)
        {
            await mediator.Send(new ValidaSeExisteDrePorCodigoQuery(filtro.DreCodigo));
            await mediator.Send(new ValidaSeExisteUePorCodigoQuery(filtro.UeCodigo));
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            filtro.UsuarioNome = usuarioLogado.Nome;
            filtro.UsuarioRF = usuarioLogado.CodigoRf;
            return await mediator.Send(new GerarRelatorioCommand(
                TipoRelatorio.AtaBimestral,
                filtro,
                usuarioLogado,
                RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosAtaBimestral)
            );
        }
    }
}