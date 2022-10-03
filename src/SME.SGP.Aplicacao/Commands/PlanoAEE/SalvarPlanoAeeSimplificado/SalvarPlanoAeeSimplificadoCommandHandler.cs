using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarPlanoAeeSimplificadoCommandHandler : IRequestHandler<SalvarPlanoAeeSimplificadoCommand, bool>
    {
        private readonly IRepositorioPlanoAEE repositorioPlanoAEE;

        public SalvarPlanoAeeSimplificadoCommandHandler(IRepositorioPlanoAEE repositorioPlanoAEE)
        {
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
        }

        public async Task<bool> Handle(SalvarPlanoAeeSimplificadoCommand request, CancellationToken cancellationToken) =>
            (await repositorioPlanoAEE.SalvarAsync(request.PlanoAEE)) > 0;
       
    }
}
