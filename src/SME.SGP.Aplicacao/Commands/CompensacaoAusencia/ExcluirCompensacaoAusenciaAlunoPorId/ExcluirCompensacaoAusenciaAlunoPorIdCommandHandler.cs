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
    public class ExcluirCompensacaoAusenciaAlunoPorIdCommandHandler : IRequestHandler<ExcluirCompensacaoAusenciaAlunoPorIdCommand, bool>
    {
        private readonly IRepositorioCompensacaoAusenciaAluno repositorioCompensacaoAusenciaAluno;

        public ExcluirCompensacaoAusenciaAlunoPorIdCommandHandler(IRepositorioCompensacaoAusenciaAluno repositorioCompensacaoAusenciaAluno)
        {
            this.repositorioCompensacaoAusenciaAluno = repositorioCompensacaoAusenciaAluno ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusenciaAluno));
        }

        public async Task<bool> Handle(ExcluirCompensacaoAusenciaAlunoPorIdCommand request, CancellationToken cancellationToken)
        {
            await repositorioCompensacaoAusenciaAluno.RemoverLogico(request.Id);
            return true;
        }
    }
}
