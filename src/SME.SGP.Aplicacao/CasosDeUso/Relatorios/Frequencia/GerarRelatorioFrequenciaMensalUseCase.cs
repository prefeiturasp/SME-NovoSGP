using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Relatorios;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class GerarRelatorioFrequenciaMensalUseCase : IGerarRelatorioFrequenciaMensalUseCase
    {
        private readonly IMediator _mediator;

        public GerarRelatorioFrequenciaMensalUseCase(IMediator mediator)
        {
            _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(FiltroRelatorioFrequenciaMensalDto filtro)
        {
            var usuario = await _mediator.Send(ObterUsuarioLogadoQuery.Instance);
            filtro.UsuarioNome = usuario.Nome;
            filtro.UsuarioRf = usuario.CodigoRf;

            if (!filtro.CodigoDre.EhFiltroTodos())
                return await GerarRelatorio(filtro, usuario);

            return await GerarRelatorioPorDre(filtro, usuario);
        }

        private async Task<bool> GerarRelatorio(FiltroRelatorioFrequenciaMensalDto filtro, Usuario usuarioLogado)
            => await _mediator.Send(new GerarRelatorioCommand(TipoRelatorio.FrequenciaMensal, filtro, usuarioLogado,
                RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosFrequenciaMensal, filtro.TipoFormatoRelatorio));

        private async Task<bool> GerarRelatorioPorDre(FiltroRelatorioFrequenciaMensalDto filtro, Usuario usuarioLogado)
        {
            var dres = await _mediator.Send(ObterTodasDresQuery.Instance);
            foreach (var dre in dres)
            {
                filtro.CodigoDre = dre.CodigoDre;
                await _mediator.Send(new GerarRelatorioCommand(TipoRelatorio.FrequenciaMensal, filtro, usuarioLogado,
                    RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosFrequenciaMensal, filtro.TipoFormatoRelatorio));
            }
            return true;
        }
    }
}
