using MediatR;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoAbandono;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoAbandono
{
    public class SalvarPainelEducacionalConsolidacaoAbandonoCommandHandler : IRequestHandler<SalvarPainelEducacionalConsolidacaoAbandonoCommand, bool>
    {
        private readonly IRepositorioPainelEducacionalConsolidacaoAbandono repositorioPainelEducacionalConsolidacaoAbandono;

        public SalvarPainelEducacionalConsolidacaoAbandonoCommandHandler(IRepositorioPainelEducacionalConsolidacaoAbandono repositorioPainelEducacionalConsolidacaoAbandono)
        {
            this.repositorioPainelEducacionalConsolidacaoAbandono = repositorioPainelEducacionalConsolidacaoAbandono;
        }

        public async Task<bool> Handle(SalvarPainelEducacionalConsolidacaoAbandonoCommand request, CancellationToken cancellationToken)
        {
            if (request.Indicadores?.Any() != true)
                return false;

            await repositorioPainelEducacionalConsolidacaoAbandono.LimparConsolidacao();

            await repositorioPainelEducacionalConsolidacaoAbandono.BulkInsertAsync(MapearParaEntidade(request.Indicadores));

            return true;
        }

        private static List<PainelEducacionalConsolidacaoAbandono> MapearParaEntidade(IEnumerable<ConsolidacaoAbandonoDto> consolidacaoAbandonoDto)
        {
            return consolidacaoAbandonoDto
                .Select(dto => new PainelEducacionalConsolidacaoAbandono
                {
                    CodigoDre = dto.CodigoDre,
                    Ano = dto.Ano,
                    Modalidade = dto.Modalidade,
                    QuantidadeDesistencias = dto.QuantidadeDesistencias,
                    AnoLetivo = dto.AnoLetivo
                })
                .ToList();
        }
    }
}
