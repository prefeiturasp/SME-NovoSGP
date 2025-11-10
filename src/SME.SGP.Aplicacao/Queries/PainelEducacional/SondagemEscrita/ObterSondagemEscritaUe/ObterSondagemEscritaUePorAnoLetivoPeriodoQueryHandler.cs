using MediatR;
using SME.SGP.Aplicacao.Queries.PainelEducacional.TaxaAlfabetizacao.ObterSondagemEscrita;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional.SondagemEscrita;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.SondagemEscrita.ObterSondagemEscrita
{
    public class ObterSondagemEscritaUePorAnoLetivoPeriodoQueryHandler : IRequestHandler<ObterSondagemEscritaUePorAnoLetivoPeriodoQuery, IEnumerable<SondagemEscritaUeDto>>
    {
        private readonly IRepositorioSondagemEscritaUe repositorioSondagemEscrita;

        public ObterSondagemEscritaUePorAnoLetivoPeriodoQueryHandler(IRepositorioSondagemEscritaUe repositorioSondagemEscrita)
        {
            this.repositorioSondagemEscrita = repositorioSondagemEscrita;
        }

        public async Task<IEnumerable<SondagemEscritaUeDto>> Handle(ObterSondagemEscritaUePorAnoLetivoPeriodoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioSondagemEscrita.ObterSondagemEscritaAsync();
        }
    }
}
