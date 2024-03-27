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
    public class ObterDataUltimaConsolidacaoDashboardNAAPAQueryHandlerFake : IRequestHandler<ObterDataUltimaConsolicacaoDashboardNaapaQuery, DateTime?>
    {
        public async Task<DateTime?> Handle(ObterDataUltimaConsolicacaoDashboardNaapaQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(DateTimeExtension.HorarioBrasilia());
        }
    }
}
