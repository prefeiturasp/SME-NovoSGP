using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao 
{ 
    public class ObterNotaConselhoEmAprovacaoQueryHandler : IRequestHandler<ObterNotaConselhoEmAprovacaoQuery, double>
    {
        private readonly IRepositorioConselhoClasseNotaConsulta repositorioConselhoClasseNota;

        public ObterNotaConselhoEmAprovacaoQueryHandler(IRepositorioConselhoClasseNotaConsulta repositorioConselhoClasseNota)
        {
            this.repositorioConselhoClasseNota = repositorioConselhoClasseNota ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseNota));
        }

        public async Task<double> Handle(ObterNotaConselhoEmAprovacaoQuery request, CancellationToken cancellationToken)
            => await repositorioConselhoClasseNota.VerificaNotaConselhoEmAprovacao(request.ConselhoClasseNotaId);
    
    }
}
