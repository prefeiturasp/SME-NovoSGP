using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDataConsolidacaoInformacoesEscolaresUseCase : AbstractUseCase, IObterDataConsolidacaoInformacoesEscolaresUseCase
    {
        public ObterDataConsolidacaoInformacoesEscolaresUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<DateTime?> Executar(int anoLetivo)
        {
            var parametroExecucao = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(Dominio.TipoParametroSistema.ExecucaoConsolidacaoInformacoesEscolares, anoLetivo));
            if (!string.IsNullOrEmpty(parametroExecucao.Valor))
                return DateTime.Parse(parametroExecucao.Valor);

            return null;
        }
    }
}
