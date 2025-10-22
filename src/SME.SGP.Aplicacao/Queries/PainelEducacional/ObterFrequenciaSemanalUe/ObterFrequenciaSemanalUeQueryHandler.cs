using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional.FrequenciaSemanalUe;
using System;
using System.Collections.Generic;
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
            return await repositorio.ObterFrequenciaSemanalUe(request.CodigoUe, request.AnoLetivo);
        }
    }
}
