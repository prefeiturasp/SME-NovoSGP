using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ObterValorParametroSistemaTipoEAnoQueryHandlerFake : IRequestHandler<ObterValorParametroSistemaTipoEAnoQuery, string>
    {
        private const String VALOR_COMPENSACAO_AUSENCIA = "23";
        private const String VALOR_COMPENSACAO_AUSENCIA_FUND2 = "75";
        private const String VALOR_COMPENSACAO_AUSENCIA_REGENCIA = "75";

        public async Task<string> Handle(ObterValorParametroSistemaTipoEAnoQuery request, CancellationToken cancellationToken)
        {
            if (request.Tipo == TipoParametroSistema.QuantidadeMaximaCompensacaoAusencia)
                return await Task.FromResult(VALOR_COMPENSACAO_AUSENCIA);
            else if (request.Tipo == TipoParametroSistema.CompensacaoAusenciaPercentualFund2)
                return await Task.FromResult(VALOR_COMPENSACAO_AUSENCIA_FUND2);

            return await Task.FromResult(VALOR_COMPENSACAO_AUSENCIA_REGENCIA);
        }
    }
}
