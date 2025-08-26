using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
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
            var registros = await repositorioPainelEducacionalRegistroFrequenciaAgrupamentoMensal.ObterFrequenciaMensal(request.CodigoDre, request.CodigoUe);

            return MapearParaDto(registros);
        }

        private IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoMensalDto> MapearParaDto(IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoMensal> registros)
        {
            var frequenciaGlobal = new List<PainelEducacionalRegistroFrequenciaAgrupamentoMensalDto>();
            foreach (var item in registros)
            {
                var modalidade = (Modalidade)item.Modalidade;
                var modalidadeInfo = typeof(Modalidade).GetMember(modalidade.ToString()).First();
                var displayAttribute = modalidadeInfo.GetCustomAttribute<DisplayAttribute>();
                var modalidadeNome = displayAttribute?.Name ?? modalidade.ToString();

                frequenciaGlobal.Add(new PainelEducacionalRegistroFrequenciaAgrupamentoMensalDto()
                {
                    Modalidade = modalidadeNome,
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
