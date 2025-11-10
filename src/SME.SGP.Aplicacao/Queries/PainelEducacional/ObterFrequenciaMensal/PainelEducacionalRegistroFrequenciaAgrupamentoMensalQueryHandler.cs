using MediatR;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFrequenciaMensal
{
    public class PainelEducacionalRegistroFrequenciaAgrupamentoMensalQueryHandler : IRequestHandler<PainelEducacionalRegistroFrequenciaAgrupamentoMensalQuery, IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoMensalDto>>
    {
        private readonly IRepositorioPainelEducacionalRegistroFrequenciaAgrupamentoMensal repositorioPainelEducacionalRegistroFrequenciaAgrupamentoMensal;

        public PainelEducacionalRegistroFrequenciaAgrupamentoMensalQueryHandler(IRepositorioPainelEducacionalRegistroFrequenciaAgrupamentoMensal repositorioPainelEducacionalRegistroFrequenciaAgrupamentoMensal)
        {
            this.repositorioPainelEducacionalRegistroFrequenciaAgrupamentoMensal = repositorioPainelEducacionalRegistroFrequenciaAgrupamentoMensal;
        }

        public async Task<IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoMensalDto>> Handle(PainelEducacionalRegistroFrequenciaAgrupamentoMensalQuery request, CancellationToken cancellationToken)
        {
            var registros = await repositorioPainelEducacionalRegistroFrequenciaAgrupamentoMensal.ObterFrequenciaMensal(request.AnoLetivo, request.CodigoDre, request.CodigoUe);

            return MapearParaDto(registros);
        }

        private IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoMensalDto> MapearParaDto(IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoMensal> registros)
        {
            var frequenciaGlobal = new List<PainelEducacionalRegistroFrequenciaAgrupamentoMensalDto>();
            foreach (var item in registros)
            {
                frequenciaGlobal.Add(new PainelEducacionalRegistroFrequenciaAgrupamentoMensalDto()
                {
                    Modalidade = item.Modalidade,
                    PercentualFrequencia = item.PercentualFrequencia,
                    TotalAulas = item.TotalAulas,
                    TotalAusencias = item.TotalFaltas,
                    Ano = item.AnoLetivo,
                    Mes = item.Mes
                });
            }

            return frequenciaGlobal;
        }
    }
}
