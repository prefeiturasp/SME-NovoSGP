using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterSecoesPAPQueryHandler : IRequestHandler<ObterSecoesPAPQuery, SecaoTurmaAlunoPAPDto>
    {
        private readonly IRepositorioSecaoRelatorioPeriodicoPAP repositorio;

        public ObterSecoesPAPQueryHandler(IRepositorioSecaoRelatorioPeriodicoPAP repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<SecaoTurmaAlunoPAPDto> Handle(ObterSecoesPAPQuery request, CancellationToken cancellationToken)
        {
            return this.repositorio.ObterSecoesPorAluno(request.CodigoTurma, request.CodigoAluno, request.PAPPeriodoId);
        }
    }
}
