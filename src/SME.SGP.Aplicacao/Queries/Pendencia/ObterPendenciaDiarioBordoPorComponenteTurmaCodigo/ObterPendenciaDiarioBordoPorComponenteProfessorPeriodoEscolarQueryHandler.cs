using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaDiarioBordoPorComponenteProfessorPeriodoEscolarQueryHandler : IRequestHandler<ObterPendenciaDiarioBordoPorComponenteProfessorPeriodoEscolarQuery, long>
    {

        private readonly IRepositorioPendenciaAulaConsulta repositorioPendenciaAula;

        public ObterPendenciaDiarioBordoPorComponenteProfessorPeriodoEscolarQueryHandler(IRepositorioPendenciaAulaConsulta repositorioPendenciaAula)
        {
            this.repositorioPendenciaAula = repositorioPendenciaAula ?? throw new ArgumentNullException(nameof(repositorioPendenciaAula));
        }

        public async Task<long> Handle(ObterPendenciaDiarioBordoPorComponenteProfessorPeriodoEscolarQuery request, CancellationToken cancellationToken)
        => await repositorioPendenciaAula.ObterPendenciaDiarioBordoPorComponenteProfessorPeriodoEscolarTurma(request.ComponenteCurricularId, request.CodigoRf, request.PeriodoEscolarId, request.CodigoTurma);
    }
}
