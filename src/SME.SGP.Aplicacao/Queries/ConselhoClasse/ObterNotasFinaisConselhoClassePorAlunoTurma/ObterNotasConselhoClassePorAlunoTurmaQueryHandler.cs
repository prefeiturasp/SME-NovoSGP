using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasConselhoClassePorAlunoTurmaQueryHandler : IRequestHandler<ObterNotasConselhoClassePorAlunoTurmaQuery, IEnumerable<NotaConceitoFechamentoConselhoFinalDto>>
    {
        private readonly IRepositorioConselhoClasseAlunoConsulta repositorioConselhoClasseAluno;

        public ObterNotasConselhoClassePorAlunoTurmaQueryHandler(IRepositorioConselhoClasseAlunoConsulta repositorioConselhoClasseAluno)
        {
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
        }

        public async Task<IEnumerable<NotaConceitoFechamentoConselhoFinalDto>> Handle(ObterNotasConselhoClassePorAlunoTurmaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioConselhoClasseAluno.ObterNotasConselhoAlunoTurma(request.AlunoCodigo,request.TurmasCodigos);
        }
    }
}
