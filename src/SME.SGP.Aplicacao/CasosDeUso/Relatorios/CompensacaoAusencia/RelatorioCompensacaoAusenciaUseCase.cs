using MediatR;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RelatorioCompensacaoAusenciaUseCase : IRelatorioCompensacaoAusenciaUseCase
    {
        private readonly IMediator mediator;

        public RelatorioCompensacaoAusenciaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(FiltroRelatorioCompensacaoAusenciaDto filtroRelatorioCompensacaoAusenciaDto)
        {
            await mediator.Send(new ValidaSeExisteDrePorCodigoQuery(filtroRelatorioCompensacaoAusenciaDto.DreCodigo));
            await mediator.Send(new ValidaSeExisteUePorCodigoQuery(filtroRelatorioCompensacaoAusenciaDto.UeCodigo));
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            filtroRelatorioCompensacaoAusenciaDto.UsuarioNome = usuarioLogado.Nome;
            filtroRelatorioCompensacaoAusenciaDto.UsuarioRf = usuarioLogado.CodigoRf;

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.CompensacaoAusencia, filtroRelatorioCompensacaoAusenciaDto, usuarioLogado));
        }
    }
}

