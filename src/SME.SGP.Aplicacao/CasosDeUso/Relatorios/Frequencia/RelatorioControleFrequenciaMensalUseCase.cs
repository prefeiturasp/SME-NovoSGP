using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Relatorios;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RelatorioControleFrequenciaMensalUseCase : IRelatorioControleFrequenciaMensalUseCase
    {
        private readonly IMediator mediator;

        public RelatorioControleFrequenciaMensalUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(FiltroRelatorioControleFrenquenciaMensalDto param)
        {
            var usuario = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            if (usuario.EhNulo())
                throw new NegocioException("Não foi possível localizar o usuário.");

            param.NomeUsuario = usuario.Nome;
            param.CodigoRf = usuario.CodigoRf;

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.RelatorioControleFrequenciaMensal, param, usuario, formato: param.TipoFormatoRelatorio, rotaRelatorio: RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosControleFrequenciaMensal));
        }
    }
}
