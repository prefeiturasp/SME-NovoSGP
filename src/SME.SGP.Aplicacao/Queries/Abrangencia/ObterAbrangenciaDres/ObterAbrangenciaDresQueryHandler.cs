using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAbrangenciaDresQueryHandler : IRequestHandler<ObterAbrangenciaDresQuery, IEnumerable<AbrangenciaDreRetornoDto>>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;

        public ObterAbrangenciaDresQueryHandler(IRepositorioAbrangencia repositorioAbrangencia)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
        }

        public async Task<IEnumerable<AbrangenciaDreRetornoDto>> Handle(ObterAbrangenciaDresQuery request, CancellationToken cancellationToken)
            => await repositorioAbrangencia.ObterDres(request.Login, request.Perfil,
                request.Modalidade, request.Periodo,
                request.ConsideraHistorico,
                request.AnoLetivo, request.Filtro, request.FiltroEhCodigo);
    }
}
