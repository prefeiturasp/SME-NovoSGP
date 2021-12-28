using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDatasDiarioBordoPorPeriodoQueryHandler : IRequestHandler<ObterDatasDiarioBordoPorPeriodoQuery, IEnumerable<DiarioBordoPorPeriodoDto>>
    {
        private readonly IRepositorioAula repositorioAula;

        public ObterDatasDiarioBordoPorPeriodoQueryHandler(IRepositorioAula repositorioAula)
        {
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
        }
        public async Task<IEnumerable<DiarioBordoPorPeriodoDto>> Handle(ObterDatasDiarioBordoPorPeriodoQuery request, CancellationToken cancellationToken)
         => await repositorioAula.ObterAulasDiariosPorPeriodo(request.TurmaCodigo, request.ComponenteCurricularFilhoId, request.ComponenteCurricularPaiCodigo, request.DataFim, request.DataInicio);
    }
}
