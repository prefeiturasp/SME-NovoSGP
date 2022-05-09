using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaDiarioBordoIdPorComponenteProfessorBimestreQueryHandler : IRequestHandler<ObterPendenciaDiarioBordoPorComponentePeriodoEscolarProfessorQuery, long>
    {

        private readonly IRepositorioPendenciaAulaConsulta repositorioPendenciaAula;

        public ObterPendenciaDiarioBordoIdPorComponenteProfessorBimestreQueryHandler(IRepositorioPendenciaAulaConsulta repositorioPendenciaAula)
        {
            this.repositorioPendenciaAula = repositorioPendenciaAula ?? throw new ArgumentNullException(nameof(repositorioPendenciaAula));
        }

        public async Task<long> Handle(ObterPendenciaDiarioBordoPorComponentePeriodoEscolarProfessorQuery request, CancellationToken cancellationToken)
        => await repositorioPendenciaAula.ObterPendenciaDiarioBordoPorComponentePeriodoEscolarProfessor(request.ComponenteCurricularId, request.CodigoRf, request.PeriodoEscolarId);
    }
}
