using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RelatorioRaaEscolaAquiUseCase : IRelatorioRaaEscolaAquiUseCase
    {
        private readonly IMediator mediator;

        public RelatorioRaaEscolaAquiUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(FiltroRelatorioEscolaAquiDto filtro)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioPorIdQuery(1));
            filtro.TurmaId = await mediator.Send(new ObterTurmaIdPorCodigoQuery(filtro.TurmaCodigo));
            filtro.Semestre = await ObterSemestreMensagem(filtro.AlunoCodigo);
            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.RaaEscolaAqui, filtro,
            usuarioLogado, formato: TipoFormatoRelatorio.Html,
            rotaRelatorio: RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosRaaEscolaAqui, notificarErroUsuario: true));
        }

        private async Task<int> ObterSemestreMensagem(string alunoCodigo)
        {
            return await mediator.Send(new ObterSemestreAtualRelatorioQuery(alunoCodigo));
        }
    }
}