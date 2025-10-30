using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional.FrequenciaSemanalUe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFrequenciaSemanalUe
{
    public class ObterFrequenciaSemanalUeQueryHandler : IRequestHandler<ObterFrequenciaSemanalUeQuery, IEnumerable<PainelEducacionalFrequenciaSemanalUeDto>>
    {
        private readonly IRepositorioFrequenciaSemanalUe repositorio;

        public ObterFrequenciaSemanalUeQueryHandler(IRepositorioFrequenciaSemanalUe repositorio) 
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<PainelEducacionalFrequenciaSemanalUeDto>> Handle(ObterFrequenciaSemanalUeQuery request, CancellationToken cancellationToken)
        {
            var dados = await repositorio.ObterFrequenciaSemanalUe(request.CodigoUe, request.AnoLetivo);
            
            var dadosProcessados = dados.Select(item => new
            {
                DataAula = RemoverHorasDaData(item.DataAula),
                PercentualFrequencia = item.PercentualFrequencia
            });

            var dadosAgrupados = dadosProcessados
                .GroupBy(x => x.DataAula)
                .Select(grupo => new PainelEducacionalFrequenciaSemanalUeDto
                {
                    DataAula = grupo.Key,
                    PercentualFrequencia = grupo.FirstOrDefault().PercentualFrequencia
                })
                .OrderBy(x => DateTime.TryParse(x.DataAula, out DateTime data) ? data : DateTime.MinValue);

            return dadosAgrupados;
        }

        private string RemoverHorasDaData(string dataCompleta)
        {
            if (string.IsNullOrWhiteSpace(dataCompleta))
                return dataCompleta;

            if (DateTime.TryParse(dataCompleta, out DateTime data))
            {
                return data.ToString("dd/MM/yyyy");
            }

            var partesData = dataCompleta.Split(' ');
            return partesData.Length > 0 ? partesData[0] : dataCompleta;
        }
    }
}
