using MediatR;
using SME.SGP.Dominio;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUltimaConsolidacaoDevolutivaUseCase : AbstractUseCase, IObterUltimaConsolidacaoDevolutivaUseCase
    {
        public ObterUltimaConsolidacaoDevolutivaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<string> Executar(int anoLetivo)
        {
            var parametroSistema = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.ExecucaoConsolidacaoDevolutivasTurma, anoLetivo));
            if (parametroSistema != null)
                parametroSistema.Valor = DateTime.Now.ToString();

            return parametroSistema.Valor;
        }
    }
}
