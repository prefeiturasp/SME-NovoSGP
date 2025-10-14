using MediatR;
using SME.SGP.Aplicacao.Queries.PainelEducacional.TaxaAlfabetizacao.ObterConsolidacaoSondagemEscrita;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.SondagemEscrita.ObterConsolidacaoSondagemEscrita
{
    public class ObterConsolidacaoSondagemEscritaUeQueryHandler : IRequestHandler<ObterConsolidacaoSondagemEscritaUeQuery, IEnumerable<PainelEducacionalConsolidacaoSondagemEscritaUe>>
    {
        private readonly IRepositorioPainelEducacionalConsolidacaoSondagemEscritaUe repositorioPainelEducacionalConsolidacaoSondagemEscrita;

        public ObterConsolidacaoSondagemEscritaUeQueryHandler(IRepositorioPainelEducacionalConsolidacaoSondagemEscritaUe repositorioPainelEducacionalConsolidacaoSondagemEscrita)
        {
            this.repositorioPainelEducacionalConsolidacaoSondagemEscrita = repositorioPainelEducacionalConsolidacaoSondagemEscrita;
        }

        public Task<IEnumerable<PainelEducacionalConsolidacaoSondagemEscritaUe>> Handle(ObterConsolidacaoSondagemEscritaUeQuery request, CancellationToken cancellationToken)
        {
            return repositorioPainelEducacionalConsolidacaoSondagemEscrita.ObterConsolidacaoAsync(request.AnoLetivo, request.Bimestre, request.CodigoUe, request.CodigoDre);
        }
    }
}
