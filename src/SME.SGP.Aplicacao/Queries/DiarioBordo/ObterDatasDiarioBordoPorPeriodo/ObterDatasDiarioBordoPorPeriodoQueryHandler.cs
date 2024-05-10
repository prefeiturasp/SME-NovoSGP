using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDatasDiarioBordoPorPeriodoQueryHandler : IRequestHandler<ObterDatasDiarioBordoPorPeriodoQuery, IEnumerable<DiarioBordoPorPeriodoDto>>
    {
        private readonly IRepositorioAulaConsulta repositorioAula;

        public ObterDatasDiarioBordoPorPeriodoQueryHandler(IRepositorioAulaConsulta repositorioAula)
        {
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
        }
        public async Task<IEnumerable<DiarioBordoPorPeriodoDto>> Handle(ObterDatasDiarioBordoPorPeriodoQuery request, CancellationToken cancellationToken)
         => await repositorioAula.ObterAulasDiariosPorPeriodo(request.TurmaCodigo, request.ComponenteCurricularFilhoCodigo, request.ComponenteCurricularPaiCodigo, request.DataFim, request.DataInicio);
    }
}
