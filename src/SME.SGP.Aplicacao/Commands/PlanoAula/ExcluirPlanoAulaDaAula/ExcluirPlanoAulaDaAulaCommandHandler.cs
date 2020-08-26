using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPlanoAulaDaAulaCommandHandler : IRequestHandler<ExcluirPlanoAulaDaAulaCommand, bool>
    {
        private readonly IRepositorioPlanoAula repositorioPlanoAula;

        public ExcluirPlanoAulaDaAulaCommandHandler(IRepositorioPlanoAula repositorioPlanoAula)
        {
            this.repositorioPlanoAula = repositorioPlanoAula ?? throw new ArgumentNullException(nameof(repositorioPlanoAula));
        }

        public async Task<bool> Handle(ExcluirPlanoAulaDaAulaCommand request, CancellationToken cancellationToken)
        {
            await repositorioPlanoAula.ExcluirPlanoDaAula(request.AulaId);
            return true;
        }
    }
}
