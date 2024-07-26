using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluiCompensacaoAlunoPorCompensacaoAlunoIdCommandHandler : IRequestHandler<ExcluiCompensacaoAlunoPorCompensacaoAlunoIdCommand, bool>
    {
        private readonly IRepositorioCompensacaoAusenciaAluno repositorioCompensacaoAusenciaAluno;
        private readonly IRepositorioCompensacaoAusenciaAlunoAula repositorioCompensacaoAusenciaAlunoAula;

        public ExcluiCompensacaoAlunoPorCompensacaoAlunoIdCommandHandler(IRepositorioCompensacaoAusenciaAluno repositorioCompensacaoAusenciaAluno, 
                                                                         IRepositorioCompensacaoAusenciaAlunoAula repositorioCompensacaoAusenciaAlunoAula)
        {
            this.repositorioCompensacaoAusenciaAluno = repositorioCompensacaoAusenciaAluno ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusenciaAluno));
            this.repositorioCompensacaoAusenciaAlunoAula = repositorioCompensacaoAusenciaAlunoAula ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusenciaAlunoAula));
        }
        public async Task<bool> Handle(ExcluiCompensacaoAlunoPorCompensacaoAlunoIdCommand request, CancellationToken cancellationToken)
        {
            await repositorioCompensacaoAusenciaAlunoAula.ExcluirCompensacaoAusenciaPorCompensacaoAlunoId(request.CompensacaoAlunoId);
            return await repositorioCompensacaoAusenciaAluno.ExcluirCompensacaoAusenciaAlunoPorId(request.CompensacaoAlunoId);
        }
    }
}
