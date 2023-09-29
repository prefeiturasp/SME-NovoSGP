using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Aula.ServicosFake
{
    public class ObterParametroSistemaPorTipoEAnoQueryHandlerInfantilFake : IRequestHandler<ObterParametroSistemaPorTipoEAnoQuery, ParametrosSistema>
    {
        public async Task<ParametrosSistema> Handle(ObterParametroSistemaPorTipoEAnoQuery request, CancellationToken cancellationToken)
        {
            if (request.TipoParametroSistema != TipoParametroSistema.PercentualFrequenciaMinimaInfantil)
                throw new NegocioException("O parâmetro deve ser o percentual de frequência mínima do infantil");

            return await Task.FromResult(new ParametrosSistema()
            {
                Valor = "75"
            });
        }
    }
}
