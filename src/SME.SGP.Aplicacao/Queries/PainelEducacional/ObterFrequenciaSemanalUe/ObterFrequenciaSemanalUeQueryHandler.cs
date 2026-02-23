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

            var dadosAgrupados = dados
                .GroupBy(x => x.DataAula)
                .Select(grupo => new PainelEducacionalFrequenciaSemanalUeDto
                {
                    DataAula = grupo.Key,
                    PercentualFrequencia = grupo.FirstOrDefault().PercentualFrequencia
                })
                .OrderBy(x => x.DataAula);

            return dadosAgrupados;
        }
    }
}
