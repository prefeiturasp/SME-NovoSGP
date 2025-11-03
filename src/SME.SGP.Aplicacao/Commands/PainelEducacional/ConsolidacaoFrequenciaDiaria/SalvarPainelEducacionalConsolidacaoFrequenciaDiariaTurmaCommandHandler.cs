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
    public class SalvarPainelEducacionalConsolidacaoFrequenciaDiariaTurmaCommandHandler : IRequestHandler<SalvarPainelEducacionalConsolidacaoFrequenciaDiariaTurmaCommand, bool>
    {
        private readonly IRepositorioPainelEducacionalConsolidacaoFrequenciaDiaria repositorioPainelEducacionalConsolidacaoFrequenciaDiaria;

        public SalvarPainelEducacionalConsolidacaoFrequenciaDiariaTurmaCommandHandler(IRepositorioPainelEducacionalConsolidacaoFrequenciaDiaria repositorioPainelEducacionalConsolidacaoFrequenciaDiaria)
        {
            this.repositorioPainelEducacionalConsolidacaoFrequenciaDiaria = repositorioPainelEducacionalConsolidacaoFrequenciaDiaria;
        }

        public async Task<bool> Handle(SalvarPainelEducacionalConsolidacaoFrequenciaDiariaTurmaCommand request, CancellationToken cancellationToken)
        {
            await repositorioPainelEducacionalConsolidacaoFrequenciaDiaria.BulkInsertAsync(MapearParaEntidade(request.Indicadores));

            return true;
        }

        private static IEnumerable<PainelEducacionalConsolidacaoFrequenciaDiaria> MapearParaEntidade(IEnumerable<ConsolidacaoFrequenciaDiariaTurmaDto> consolidacaoFrequenciaDto)
        {
            return consolidacaoFrequenciaDto
                .Select(dto => new PainelEducacionalConsolidacaoFrequenciaDiaria
                {
                    CodigoDre = dto.CodigoDre,
                    CodigoUe = dto.CodigoUe,
                    TurmaId = dto.TurmaId,
                    NivelFrequencia = dto.NivelFrequencia,
                    Turma = dto.Turma,
                    AnoLetivo = dto.AnoLetivo,
                    TotalEstudantes = dto.TotalEstudantes,
                    TotalPresentes = dto.TotalPresentes,
                    PercentualFrequencia = dto.PercentualFrequencia,
                    DataAula = dto.DataAula,
                });
        }
    }
}
