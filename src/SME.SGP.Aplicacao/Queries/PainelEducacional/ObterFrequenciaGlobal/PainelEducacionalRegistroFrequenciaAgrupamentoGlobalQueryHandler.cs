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
            var registros = await repositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobal.ListarAsync();

            return MapearParaDto(registros);
        }

        private IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoGlobalDto> MapearParaDto(IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoGlobal> registros)
        {
            var frequenciaGlobal = new List<PainelEducacionalRegistroFrequenciaAgrupamentoGlobalDto>();
            foreach (var item in registros)
            {
                try
                {
                    var modalidade = (Modalidade)item.Modalidade;
                    var modalidadeInfo = typeof(Modalidade).GetMember(modalidade.ToString()).First();
                    var displayAttribute = modalidadeInfo.GetCustomAttribute<DisplayAttribute>();
                    var modalidadeNome = displayAttribute?.Name ?? modalidade.ToString();

                    frequenciaGlobal.Add(new PainelEducacionalRegistroFrequenciaAgrupamentoGlobalDto()
                    {
                       Modalidade = modalidadeNome,
                       PercentualFrequencia = item.PercentualFrequencia,
                       TotalAlunos = item.TotalAlunos,
                       TotalAulas = item.TotalAulas,
                       TotalAusencias = item.TotalAusencias,
                       TotalCompensacoes = item.TotalCompensacoes
                    });
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            return frequenciaGlobal;
        }
    }
}
