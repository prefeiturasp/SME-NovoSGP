using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoTaxaAlfabetizacao
{
    public class SalvarPainelEducacionalConsolidacaoTaxaAlfabetizacaoCommandHandler : IRequestHandler<SalvarPainelEducacionalConsolidacaoTaxaAlfabetizacaoCommand, bool>
    {
        private readonly IRepositorioTaxaAlfabetizacao repositorioTaxaAlfabetizacao;

        public SalvarPainelEducacionalConsolidacaoTaxaAlfabetizacaoCommandHandler(IRepositorioTaxaAlfabetizacao repositorioTaxaAlfabetizacao)
        {
            this.repositorioTaxaAlfabetizacao = repositorioTaxaAlfabetizacao;
        }

        public async Task<bool> Handle(SalvarPainelEducacionalConsolidacaoTaxaAlfabetizacaoCommand request, CancellationToken cancellationToken)
        {
            if (request.Indicadores?.Any() != true)
                return false;

            await repositorioTaxaAlfabetizacao.LimparConsolidacao();

            await repositorioTaxaAlfabetizacao.BulkInsertAsync(request.Indicadores);

            return true;
        }
    }
}
