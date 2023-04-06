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
    public class ExcluirCompensacaoAusenciaAlunoAulaPorIdsCommandHandler : IRequestHandler<ExcluirCompensacaoAusenciaAlunoAulaPorIdsCommand, bool>
    {
        private readonly IRepositorioCompensacaoAusenciaAlunoAula repositorioCompensacaoAusenciaAlunoAula;

        public ExcluirCompensacaoAusenciaAlunoAulaPorIdsCommandHandler(IRepositorioCompensacaoAusenciaAlunoAula repositorioCompensacaoAusenciaAlunoAula)
        {
            this.repositorioCompensacaoAusenciaAlunoAula = repositorioCompensacaoAusenciaAlunoAula ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusenciaAlunoAula));
        }

        public async Task<bool> Handle(ExcluirCompensacaoAusenciaAlunoAulaPorIdsCommand request, CancellationToken cancellationToken)
        {
            await repositorioCompensacaoAusenciaAlunoAula.RemoverLogico(request.Ids);
            return true;
        }
    }
}
