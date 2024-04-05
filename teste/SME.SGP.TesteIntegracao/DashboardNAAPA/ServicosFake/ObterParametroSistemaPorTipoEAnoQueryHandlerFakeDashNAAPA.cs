using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.DashboardNAAPA.ServicosFake
{
    public class ObterParametroSistemaPorTipoEAnoQueryHandlerFakeDashNAAPA : IRequestHandler<ObterParametroSistemaPorTipoEAnoQuery, ParametrosSistema>
    {
        public async Task<ParametrosSistema> Handle(ObterParametroSistemaPorTipoEAnoQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new ParametrosSistema()
            {
                Ano = DateTime.Now.Year,
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                CriadoRF = "Sistema",
                Valor = DateTime.Now.ToString(),
                Ativo = true,
                Tipo = TipoParametroSistema.GerarConsolidadoEncaminhamentoNAAPA,
                Id = 1,
                Nome = "Consolidação Dash NAAPA"
            });
        }
    }
}
