using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaDiarioBordoPorComponenteProfessorPeriodoEscolarQueryHandler : IRequestHandler<ObterPendenciaDiarioBordoPorComponenteProfessorPeriodoEscolarQuery, long>
    {

        private readonly IRepositorioPendenciaDiarioBordoConsulta repositorioPendenciaDiarioBordo;

        public ObterPendenciaDiarioBordoPorComponenteProfessorPeriodoEscolarQueryHandler(IRepositorioPendenciaDiarioBordoConsulta repositorio)
        {
            this.repositorioPendenciaDiarioBordo = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<long> Handle(ObterPendenciaDiarioBordoPorComponenteProfessorPeriodoEscolarQuery request, CancellationToken cancellationToken)
        => await repositorioPendenciaDiarioBordo.ObterPendenciaDiarioBordoPorComponenteProfessorPeriodoEscolar(request.ComponenteCurricularId, request.CodigoRf, request.PeriodoEscolarId);
    }
}
