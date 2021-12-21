using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasFinaisConselhoFechamentoPorAlunoTurmasQueryHandler : IRequestHandler<ObterNotasFinaisConselhoFechamentoPorAlunoTurmasQuery, IEnumerable<NotaConceitoFechamentoConselhoFinalDto>>
    {
        private readonly IRepositorioConselhoClasseAlunoConsulta repositorioConselhoClasseAluno;

        public ObterNotasFinaisConselhoFechamentoPorAlunoTurmasQueryHandler(IRepositorioConselhoClasseAlunoConsulta repositorioConselhoClasseAluno)
        {
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
        }

        public async Task<IEnumerable<NotaConceitoFechamentoConselhoFinalDto>> Handle(ObterNotasFinaisConselhoFechamentoPorAlunoTurmasQuery request, CancellationToken cancellationToken)
        {
            return await repositorioConselhoClasseAluno.ObterNotasFinaisAlunoAsync(request.TurmasCodigos, request.AlunoCodigo);
        }
    }
}
