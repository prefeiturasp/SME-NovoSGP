using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterProfessorDaTurmaPorAulaIdQueryHandler : IRequestHandler<ObterProfessorDaTurmaPorAulaIdQuery, ProfessorDto>
    {
        private readonly IRepositorioUsuario repositorioUsuario;

        public ObterProfessorDaTurmaPorAulaIdQueryHandler(IRepositorioUsuario repositorioUsuario)
        {
            this.repositorioUsuario = repositorioUsuario ?? throw new ArgumentNullException(nameof(repositorioUsuario));
        }

        public async Task<ProfessorDto> Handle(ObterProfessorDaTurmaPorAulaIdQuery request, CancellationToken cancellationToken)
                      => await repositorioUsuario.ObterProfessorDaTurmaPorAulaId(request.AulaId);
    }
}
