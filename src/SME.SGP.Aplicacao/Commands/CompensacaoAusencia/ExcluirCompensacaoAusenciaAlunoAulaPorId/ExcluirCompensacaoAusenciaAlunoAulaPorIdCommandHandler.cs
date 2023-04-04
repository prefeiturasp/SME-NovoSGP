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
    public class ExcluirCompensacaoAusenciaAlunoAulaPorIdCommandHandler : IRequestHandler<ExcluirCompensacaoAusenciaAlunoAulaPorIdCommand, bool>
    {
        private readonly IRepositorioCompensacaoAusenciaAluno repositorioCompensacaoAusenciaAluno;

        public ExcluirCompensacaoAusenciaAlunoAulaPorIdCommandHandler(IRepositorioCompensacaoAusenciaAluno repositorioCompensacaoAusenciaAluno)
        {
            this.repositorioCompensacaoAusenciaAluno = repositorioCompensacaoAusenciaAluno ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusenciaAluno));
        }

        public async Task<bool> Handle(ExcluirCompensacaoAusenciaAlunoAulaPorIdCommand request, CancellationToken cancellationToken)
        {
            await repositorioCompensacaoAusenciaAluno.ExclusaoLogicaCompensacaoAusenciaAlunoPorId(request.Id);
            return true;
        }
    }
}
