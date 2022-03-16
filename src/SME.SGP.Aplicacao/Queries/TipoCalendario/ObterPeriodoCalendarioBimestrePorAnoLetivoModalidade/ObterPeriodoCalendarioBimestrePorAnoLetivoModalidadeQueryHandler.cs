using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoCalendarioBimestrePorAnoLetivoModalidadeQueryHandler : IRequestHandler<ObterPeriodoCalendarioBimestrePorAnoLetivoModalidadeQuery, IEnumerable<PeriodoCalendarioBimestrePorAnoLetivoModalidadeDto>>
    {
        private readonly IRepositorioTipoCalendarioConsulta repositorioTipoCalendario;

        public ObterPeriodoCalendarioBimestrePorAnoLetivoModalidadeQueryHandler(IRepositorioTipoCalendarioConsulta repositorioTipoCalendario)
        {
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
        }
        public async Task<IEnumerable<PeriodoCalendarioBimestrePorAnoLetivoModalidadeDto>> Handle(ObterPeriodoCalendarioBimestrePorAnoLetivoModalidadeQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTipoCalendario.ObterPeriodoTipoCalendarioBimestreAsync(request.AnoLetivo, (int)request.Modalidade, request.Semestre);
        }
    }
}
