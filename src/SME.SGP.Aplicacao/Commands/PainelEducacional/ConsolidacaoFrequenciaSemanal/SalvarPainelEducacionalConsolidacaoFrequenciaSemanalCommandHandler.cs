using MediatR;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional.Frequencia;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.ConsolidacaoFrequenciaSemanal
{
    public class SalvarPainelEducacionalConsolidacaoFrequenciaSemanalCommandHandler : IRequestHandler<SalvarPainelEducacionalConsolidacaoFrequenciaSemanalCommand, bool>
    {
        private readonly IRepositorioPainelEducacionalConsolidacaoFrequenciaSemanal repositorioPainelEducacionalConsolidacaoFrequenciaSemanal;

        public SalvarPainelEducacionalConsolidacaoFrequenciaSemanalCommandHandler(IRepositorioPainelEducacionalConsolidacaoFrequenciaSemanal repositorioPainelEducacionalConsolidacaoFrequenciaSemanal)
        {
            this.repositorioPainelEducacionalConsolidacaoFrequenciaSemanal = repositorioPainelEducacionalConsolidacaoFrequenciaSemanal;
        }

        public async Task<bool> Handle(SalvarPainelEducacionalConsolidacaoFrequenciaSemanalCommand request, CancellationToken cancellationToken)
        {
            await repositorioPainelEducacionalConsolidacaoFrequenciaSemanal.LimparConsolidacao();

            await repositorioPainelEducacionalConsolidacaoFrequenciaSemanal.BulkInsertAsync(MapearParaEntidade(request.Indicadores));

            return true;
        }

        private static IEnumerable<PainelEducacionalConsolidacaoFrequenciaSemanal> MapearParaEntidade(IEnumerable<ConsolidacaoFrequenciaSemanalDto> consolidacaoFrequenciaDto)
        {
            if (consolidacaoFrequenciaDto == null)
                return Enumerable.Empty<PainelEducacionalConsolidacaoFrequenciaSemanal>();

            return consolidacaoFrequenciaDto.Select(dto => new PainelEducacionalConsolidacaoFrequenciaSemanal
            {
                TotalPresentes = dto.TotalPresentes,
                TotalEstudantes = dto.TotalEstudantes,
                CodigoDre = dto.CodigoDre,
                CodigoUe = dto.CodigoUe,
                AnoLetivo = dto.AnoLetivo,
                PercentualFrequencia = dto.PercentualFrequencia,
                DataAula = dto.DataAula
            });
        }
    }
}
