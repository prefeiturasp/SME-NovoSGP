using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarPlanoAEEReestruturacaoCommandHandler : IRequestHandler<SalvarPlanoAEEReestruturacaoCommand, long>
    {
        private readonly IRepositorioPlanoAEEReestruturacao reposiotorio;

        public SalvarPlanoAEEReestruturacaoCommandHandler(IRepositorioPlanoAEEReestruturacao reposiotorio)
        {
            this.reposiotorio = reposiotorio ?? throw new ArgumentNullException(nameof(reposiotorio));
        }

        public async Task<long> Handle(SalvarPlanoAEEReestruturacaoCommand request, CancellationToken cancellationToken)
            => await reposiotorio.SalvarAsync(request.Reestruturacao);
    }
}
