using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{

    public class ObterTurmaDaPendenciaProfessorQueryHandler : IRequestHandler<ObterTurmaDaPendenciaProfessorQuery, Turma>
    {
        private readonly IRepositorioPendenciaProfessor repositorioPendenciaProfessor;

        public ObterTurmaDaPendenciaProfessorQueryHandler(IRepositorioPendenciaProfessor repositorioPendenciaProfessor)
        {
            this.repositorioPendenciaProfessor = repositorioPendenciaProfessor ?? throw new ArgumentNullException(nameof(repositorioPendenciaProfessor));
        }

        public async Task<Turma> Handle(ObterTurmaDaPendenciaProfessorQuery request, CancellationToken cancellationToken)
            => await repositorioPendenciaProfessor.ObterTurmaDaPendencia(request.PendenciaId);
    }
}
