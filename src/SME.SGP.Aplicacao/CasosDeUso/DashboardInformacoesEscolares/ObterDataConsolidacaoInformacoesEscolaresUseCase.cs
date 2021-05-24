using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
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
            var parametroExecucao = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.ExecucaoConsolidacaoInformacoesEscolares, anoLetivo));
            
            if (parametroExecucao == null)
                throw new NegocioException("Não foi possível localizar a última consolidação de Informações escolares para o Ano informado");

                if (!string.IsNullOrEmpty(parametroExecucao.Valor))
                    return DateTime.Parse(parametroExecucao.Valor);

            return null;
        }
    }
}
