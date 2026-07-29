using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ConsultaDisciplina.ServicosFake
{
    public class ObterParametroSistemaPorTipoEAnoQueryHandlerFake : IRequestHandler<ObterParametroSistemaPorTipoEAnoQuery, ParametrosSistema>
    {
        public ObterParametroSistemaPorTipoEAnoQueryHandlerFake() { }
        public async Task<ParametrosSistema> Handle(ObterParametroSistemaPorTipoEAnoQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new ParametrosSistema() { Valor = string.Empty });
        }
    }
}
