using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
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

            return await _mediator.Send(new GerarRelatorioCommand(TipoRelatorio.FrequenciaMensal, filtro, usuario,
                RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosFrequenciaMensal, filtro.TipoFormatoRelatorio));
        }
    }
}
