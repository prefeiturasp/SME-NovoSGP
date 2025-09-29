using MediatR;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFrequenciaGlobal
{
    public class PainelEducacionalRegistroFrequenciaAgrupamentoGlobalQueryHandler : IRequestHandler<PainelEducacionalRegistroFrequenciaAgrupamentoGlobalQuery, IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoGlobalDto>>
    {
        private readonly IRepositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobal repositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobal;

        public PainelEducacionalRegistroFrequenciaAgrupamentoGlobalQueryHandler(IRepositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobal repositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobal)
        {
            this.repositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobal = repositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobal;
        }

        public async Task<IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoGlobalDto>> Handle(PainelEducacionalRegistroFrequenciaAgrupamentoGlobalQuery request, CancellationToken cancellationToken)
        {
            var registros = await repositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobal.ObterFrequenciaGlobal(request.AnoLetivo, request.CodigoDre, request.CodigoUe);

            return MapearParaDto(registros);
        }

        private IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoGlobalDto> MapearParaDto(IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoGlobal> registros)
        {
            var frequenciaGlobal = new List<PainelEducacionalRegistroFrequenciaAgrupamentoGlobalDto>();
            foreach (var item in registros)
            {
                frequenciaGlobal.Add(new PainelEducacionalRegistroFrequenciaAgrupamentoGlobalDto()
                {
                    Modalidade = item.Modalidade,
                    PercentualFrequencia = item.PercentualFrequencia,
                    TotalAlunos = item.TotalAlunos,
                    TotalAulas = item.TotalAulas,
                    TotalAusencias = item.TotalAusencias
                });
            }

            return frequenciaGlobal;
        }
    }
}
