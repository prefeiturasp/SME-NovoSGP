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
    public class AlterarTotalCompensacoesPorCompensacaoAlunoIdCommandHandler : IRequestHandler<AlterarTotalCompensacoesPorCompensacaoAlunoIdCommand, bool>
    {
        private readonly IRepositorioCompensacaoAusenciaAluno repositorioCompensacaoAusenciaAluno;

        public AlterarTotalCompensacoesPorCompensacaoAlunoIdCommandHandler(IRepositorioCompensacaoAusenciaAluno repositorioCompensacaoAusenciaAluno)
        {
            this.repositorioCompensacaoAusenciaAluno = repositorioCompensacaoAusenciaAluno ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusenciaAluno));
        }
        public async Task<bool> Handle(AlterarTotalCompensacoesPorCompensacaoAlunoIdCommand request, CancellationToken cancellationToken)
         => await repositorioCompensacaoAusenciaAluno.AlterarQuantidadeCompensacoesPorCompensacaoAlunoId(request.CompensacaoAlunoId, request.Quantidade);
    }
}
