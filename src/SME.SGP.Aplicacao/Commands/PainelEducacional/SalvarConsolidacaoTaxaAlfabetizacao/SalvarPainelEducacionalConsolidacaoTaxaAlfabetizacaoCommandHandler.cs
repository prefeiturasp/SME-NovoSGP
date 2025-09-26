using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoTaxaAlfabetizacao
{
    public class SalvarPainelEducacionalConsolidacaoTaxaAlfabetizacaoCommandHandler : IRequestHandler<SalvarPainelEducacionalConsolidacaoTaxaAlfabetizacaoCommand, bool>
    {
        private readonly IRepositorioPainelEducacionalConsolidacaoTaxaAlfabetizacao repositorioPainelEducacionalConsolidacaoTaxaAlfabetizacao;

        public SalvarPainelEducacionalConsolidacaoTaxaAlfabetizacaoCommandHandler(IRepositorioPainelEducacionalConsolidacaoTaxaAlfabetizacao repositorioPainelEducacionalConsolidacaoTaxaAlfabetizacao)
        {
            this.repositorioPainelEducacionalConsolidacaoTaxaAlfabetizacao = repositorioPainelEducacionalConsolidacaoTaxaAlfabetizacao;
        }

        public async Task<bool> Handle(SalvarPainelEducacionalConsolidacaoTaxaAlfabetizacaoCommand request, CancellationToken cancellationToken)
        {
            if (request.Indicadores?.Any() != true)
                return false;

            await repositorioPainelEducacionalConsolidacaoTaxaAlfabetizacao.LimparConsolidacao();

            await repositorioPainelEducacionalConsolidacaoTaxaAlfabetizacao.BulkInsertAsync(request.Indicadores);

            return true;
        }
    }
}
