using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClasseAlunosPorTurmaQueryHandler : IRequestHandler<ObterConselhoClasseAlunosPorTurmaQuery, IEnumerable<ConselhoClasseFechamentoAlunoDto>>
    {
        private readonly IRepositorioConselhoClasseAlunoConsulta repositorioConselhoClasseAluno;

        public ObterConselhoClasseAlunosPorTurmaQueryHandler(IRepositorioConselhoClasseAlunoConsulta repositorioConselhoClasseAluno)
        {
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
        }

        public Task<IEnumerable<ConselhoClasseFechamentoAlunoDto>> Handle(ObterConselhoClasseAlunosPorTurmaQuery request, CancellationToken cancellationToken)
        {
            return repositorioConselhoClasseAluno.ObterConselhoClasseAlunosPorTurma(request.TurmaCodigo);
        }
    }
}
