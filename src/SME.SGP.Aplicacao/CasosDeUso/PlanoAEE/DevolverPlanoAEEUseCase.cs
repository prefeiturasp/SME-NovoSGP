using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class DevolverPlanoAEEUseCase : AbstractUseCase, IDevolverPlanoAEEUseCase
    {
        public DevolverPlanoAEEUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(DevolucaoPlanoAEEDto param)
        {
            var planoAEE = await mediator.Send(new ObterPlanoAEEPorIdQuery(param.PlanoAEEId));

            if (planoAEE == null)
                throw new NegocioException("Plano AEE não encontrado.");

            planoAEE.Situacao = SituacaoPlanoAEE.Devolvido;

            await mediator.Send(new PersistirPlanoAEECommand(planoAEE));

            await mediator.Send(new ResolverPendenciaPlanoAEECommand(planoAEE.Id));

            if( await ParametroGeracaoPendenciaAtivo())            
                await mediator.Send(new GerarPendenciaDevolucaoPlanoAEECommand(param.PlanoAEEId, param.Motivo));

            return true;
        }
        private async Task<bool> ParametroGeracaoPendenciaAtivo()
        {
            var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.GerarPendenciasPlanoAEE, DateTime.Today.Year));

            return parametro != null && parametro.Ativo;
        }
    }
}
