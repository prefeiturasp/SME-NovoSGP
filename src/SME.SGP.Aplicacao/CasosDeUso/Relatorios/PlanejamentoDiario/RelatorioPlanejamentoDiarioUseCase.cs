using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RelatorioPlanejamentoDiarioUseCase : AbstractUseCase, IRelatorioPlanejamentoDiarioUseCase
    {
        public RelatorioPlanejamentoDiarioUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(FiltroRelatorioPlanejamentoDiario filtro)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            filtro.UsuarioNome = usuarioLogado.Nome;

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.PlanejamentoDiario, filtro, usuarioLogado, rotaRelatorio: RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosControlePlanejamentoDiario));
        }
    }
}
