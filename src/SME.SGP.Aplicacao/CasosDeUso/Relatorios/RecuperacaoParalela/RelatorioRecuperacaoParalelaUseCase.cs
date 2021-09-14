using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using System;
using System.Threading.Tasks;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class RelatorioRecuperacaoParalelaUseCase : IRelatorioRecuperacaoParalelaUseCase
    {
        private readonly IMediator mediator;

        public RelatorioRecuperacaoParalelaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(FiltroRelatorioRecuperacaoParalelaDto filtro)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            filtro.UsuarioNome = usuarioLogado.Nome;
            filtro.UsuarioRf = usuarioLogado.CodigoRf;

            if (usuarioLogado == null)
            {
                throw new NegocioException("Não foi possível identificar o usuário");
            }

            await mediator.Send(new ValidaSeExisteTurmaPorCodigoQuery(filtro.TurmaCodigo));

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.RecuperacaoParalela, filtro, usuarioLogado, rotaRelatorio: RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosPapRelatorioSemestral));
        }
    }
}
