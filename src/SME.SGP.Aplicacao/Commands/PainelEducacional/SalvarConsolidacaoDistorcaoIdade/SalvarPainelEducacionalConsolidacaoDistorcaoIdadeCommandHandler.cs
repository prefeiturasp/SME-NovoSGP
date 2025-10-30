using MediatR;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoDistorcaoIdade;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoDistorcaoIdade
{
    public class SalvarPainelEducacionalConsolidacaoDistorcaoIdadeCommandHandler : IRequestHandler<SalvarPainelEducacionalConsolidacaoDistorcaoIdadeCommand, bool>
    {
        private readonly IRepositorioPainelEducacionalConsolidacaoDistorcaoIdade repositorio;

        public SalvarPainelEducacionalConsolidacaoDistorcaoIdadeCommandHandler(IRepositorioPainelEducacionalConsolidacaoDistorcaoIdade repositorio)
        {
            this.repositorio= repositorio;
        }

        public async Task<bool> Handle(SalvarPainelEducacionalConsolidacaoDistorcaoIdadeCommand request, CancellationToken cancellationToken)
        {
            if (request.Indicadores?.Any() != true)
                return false;

            var menorAnoLetivo = request.Indicadores.Min(c => c.AnoLetivo);

            await repositorio.LimparConsolidacao(menorAnoLetivo);

            await repositorio.BulkInsertAsync(MapearParaEntidade(request.Indicadores));

            return true;
        }

        private static List<PainelEducacionalConsolidacaoDistorcaoIdade> MapearParaEntidade(IEnumerable<ConsolidacaoDistorcaoIdadeDto> consolidacaoDistorcaoIdadeDto)
        {
            return consolidacaoDistorcaoIdadeDto
                .Select(dto => new PainelEducacionalConsolidacaoDistorcaoIdade
                {
                    AnoLetivo = dto.AnoLetivo,
                    CodigoDre = dto.CodigoDre,
                    CodigoUe = dto.CodigoUe,
                    Modalidade = dto.Modalidade,
                    Ano = dto.Ano,
                    QuantidadeAlunos = dto.QuantidadeAlunos,
                })
                .ToList();
        }
    }
}
