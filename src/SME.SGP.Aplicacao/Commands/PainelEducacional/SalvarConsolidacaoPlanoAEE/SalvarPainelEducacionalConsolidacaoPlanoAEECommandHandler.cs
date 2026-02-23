using MediatR;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoPlanoAEE;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoPlanoAEE
{
    public class SalvarPainelEducacionalConsolidacaoPlanoAEECommandHandler : IRequestHandler<SalvarPainelEducacionalConsolidacaoPlanoAEECommand, bool>
    {
        private readonly IRepositorioPainelEducacionalConsolidacaoPlanoAEE repositorio;

        public SalvarPainelEducacionalConsolidacaoPlanoAEECommandHandler(IRepositorioPainelEducacionalConsolidacaoPlanoAEE repositorio)
        {
            this.repositorio= repositorio;
        }

        public async Task<bool> Handle(SalvarPainelEducacionalConsolidacaoPlanoAEECommand request, CancellationToken cancellationToken)
        {
            if (request.Indicadores?.Any() != true)
                return false;
            
            await repositorio.LimparConsolidacao();

            await repositorio.BulkInsertAsync(MapearParaEntidade(request.Indicadores));

            return true;
        }

        private static List<PainelEducacionalConsolidacaoPlanoAEE> MapearParaEntidade(IEnumerable<DadosParaConsolidarPlanosAEEDto> consolidacaoPlanoAEEDto)
        {
            return consolidacaoPlanoAEEDto
                .Select(dto => new PainelEducacionalConsolidacaoPlanoAEE
                {
                    AnoLetivo = dto.AnoLetivo,
                    CodigoDre = dto.CodigoDre,
                    CodigoUe = dto.CodigoUe,
                    SituacaoPlano = dto.SituacaoPlano,                    
                    QuantidadeSituacaoPlano = dto.QuantidadeSituacaoPlano,
                })
                .ToList();
        }
    }
}
