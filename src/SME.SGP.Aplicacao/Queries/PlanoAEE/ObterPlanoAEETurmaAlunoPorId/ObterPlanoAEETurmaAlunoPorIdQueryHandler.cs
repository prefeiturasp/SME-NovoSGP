using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanoAEETurmaAlunoPorIdQueryHandler : IRequestHandler<ObterPlanoAEETurmaAlunoPorIdQuery, IEnumerable<PlanoAEETurmaAluno>>
    {
        private readonly IRepositorioPlanoAEETurmaAlunoConsulta repositorioPlanoAEETurmaAlunoConsulta;

        public ObterPlanoAEETurmaAlunoPorIdQueryHandler(IRepositorioPlanoAEETurmaAlunoConsulta repositorioPlanoAEETurmaAlunoConsulta)
        {
            this.repositorioPlanoAEETurmaAlunoConsulta = repositorioPlanoAEETurmaAlunoConsulta ?? throw new ArgumentNullException(nameof(repositorioPlanoAEETurmaAlunoConsulta));
        }

        public Task<IEnumerable<PlanoAEETurmaAluno>> Handle(ObterPlanoAEETurmaAlunoPorIdQuery request, CancellationToken cancellationToken)
        {
            return repositorioPlanoAEETurmaAlunoConsulta.ObterPlanoAEETurmaAlunoPorIdAsync(request.PlanoAEEId);
        }
    }
}
