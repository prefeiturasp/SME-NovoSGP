using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarAtribuicaoEsporadicaCommandHandler : IRequestHandler<SalvarAtribuicaoEsporadicaCommand, long>
    {
      
        private readonly IRepositorioAtribuicaoEsporadica repositorio;

        public SalvarAtribuicaoEsporadicaCommandHandler(IRepositorioAtribuicaoEsporadica repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<long> Handle(SalvarAtribuicaoEsporadicaCommand request, CancellationToken cancellationToken)
            => await repositorio.SalvarAsync(request.AtribuicaoEsporadica);
    }
}
