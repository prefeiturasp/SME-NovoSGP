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
    public class AtualizarTurmaAlunoPlanoAEECommandHandler : IRequestHandler<AtualizarTurmaAlunoPlanoAEECommand, bool>
    {
        private readonly IRepositorioPlanoAEE repositorioPlanoAEE;

        public AtualizarTurmaAlunoPlanoAEECommandHandler(IRepositorioPlanoAEE repositorioPlanoAEE)
        {
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
        }
        public async Task<bool> Handle(AtualizarTurmaAlunoPlanoAEECommand request, CancellationToken cancellationToken)
           => (await repositorioPlanoAEE.AtualizarTurmaParaRegularPlanoAEE(request.PlanoAEEId, request.TurmaId)) > 0;
    }
}
