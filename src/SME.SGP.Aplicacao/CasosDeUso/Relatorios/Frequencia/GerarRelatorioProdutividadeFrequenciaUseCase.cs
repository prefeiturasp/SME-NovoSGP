using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class GerarRelatorioProdutividadeFrequenciaUseCase : IGerarRelatorioProdutividadeFrequenciaUseCase
    {
        private readonly IMediator mediator;

        public GerarRelatorioProdutividadeFrequenciaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }
        public async Task<bool> Executar(FiltroRelatorioProdutividadeFrequenciaDto filtro)
        {
            var usuario = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            if (usuario.EhNulo())
                throw new NegocioException("Não foi possível localizar o usuário.");

            filtro.UsuarioNome = usuario.Nome;
            filtro.UsuarioRf = usuario.CodigoRf;

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.ProdutividadeFrequencia, filtro, usuario, formato:TipoFormatoRelatorio.Xlsx, rotaRelatorio: RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosProdutividadeFrequencia));
        }
    }
}
