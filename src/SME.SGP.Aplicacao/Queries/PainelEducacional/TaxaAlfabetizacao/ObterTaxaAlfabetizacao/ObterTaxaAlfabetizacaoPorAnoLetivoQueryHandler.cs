using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional.TaxaAlfabetizacao;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.TaxaAlfabetizacao.ObterTaxaAlfabetizacao
{
    public class ObterTaxaAlfabetizacaoPorAnoLetivoQueryHandler : IRequestHandler<ObterTaxaAlfabetizacaoPorAnoLetivoQuery, IEnumerable<TaxaAlfabetizacaoDto>>
    {
        private readonly IRepositorioTaxaAlfabetizacao repositorioTaxaAlfabetizacao;

        public ObterTaxaAlfabetizacaoPorAnoLetivoQueryHandler(IRepositorioTaxaAlfabetizacao repositorioTaxaAlfabetizacao)
        {
            this.repositorioTaxaAlfabetizacao = repositorioTaxaAlfabetizacao;
        }

        public async Task<IEnumerable<TaxaAlfabetizacaoDto>> Handle(ObterTaxaAlfabetizacaoPorAnoLetivoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTaxaAlfabetizacao.ObterTaxaAlfabetizacaoAsync();
        }
    }
}
