using MediatR;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional.Frequencia;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.ConsolidacaoFrequenciaDiaria
{
    public class SalvarPainelEducacionalConsolidacaoFrequenciaDiariaCommandHandler : IRequestHandler<SalvarPainelEducacionalConsolidacaoFrequenciaDiariaCommand, bool>
    {
        private readonly IRepositorioPainelEducacionalConsolidacaoFrequenciaDiariaDre repositorioPainelEducacionalConsolidacaoFrequenciaDiariaDre;

        public SalvarPainelEducacionalConsolidacaoFrequenciaDiariaCommandHandler(IRepositorioPainelEducacionalConsolidacaoFrequenciaDiariaDre repositorioPainelEducacionalConsolidacaoFrequenciaDiariaDre)
        {
            this.repositorioPainelEducacionalConsolidacaoFrequenciaDiariaDre = repositorioPainelEducacionalConsolidacaoFrequenciaDiariaDre;
        }
        public async Task<bool> Handle(SalvarPainelEducacionalConsolidacaoFrequenciaDiariaCommand request, CancellationToken cancellationToken)
        {
            await repositorioPainelEducacionalConsolidacaoFrequenciaDiariaDre.BulkInsertAsync(MapearParaEntidade(request.Indicadores));

            return true;
        }

        private static IEnumerable<PainelEducacionalConsolidacaoFrequenciaDiariaDre> MapearParaEntidade(IEnumerable<ConsolidacaoFrequenciaDiariaDreDto> consolidacaoFrequenciaDto)
        {
            return consolidacaoFrequenciaDto
                .Select(dto => new PainelEducacionalConsolidacaoFrequenciaDiariaDre
                {
                    CodigoDre = dto.CodigoDre,
                    CodigoUe = dto.CodigoUe,
                    NivelFrequencia = dto.NivelFrequencia,
                    Ue = dto.Ue,
                    AnoLetivo = dto.AnoLetivo,
                    TotalEstudantes = dto.TotalEstudantes,
                    TotalPresentes = dto.TotalPresentes,
                    PercentualFrequencia = dto.PercentualFrequencia,
                    DataAula = dto.DataAula,
                });
        }
    }
}
