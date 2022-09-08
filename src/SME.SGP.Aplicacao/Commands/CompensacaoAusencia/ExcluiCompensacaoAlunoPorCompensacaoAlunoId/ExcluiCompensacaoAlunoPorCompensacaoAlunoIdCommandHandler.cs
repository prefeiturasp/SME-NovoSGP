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

        public ExcluiCompensacaoAlunoPorCompensacaoAlunoIdCommandHandler(IRepositorioCompensacaoAusenciaAluno repositorioCompensacaoAusenciaAluno)
        {
            this.repositorioCompensacaoAusenciaAluno = repositorioCompensacaoAusenciaAluno ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusenciaAluno));
        }
        public async Task<bool> Handle(ExcluiCompensacaoAlunoPorCompensacaoAlunoIdCommand request, CancellationToken cancellationToken)
         => await repositorioCompensacaoAusenciaAluno.ExcluirCompensacaoAusenciaAlunoPorId(request.CompensacaoAlunoId);
    }
}
