using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarHistoricoNotaConselhoClasseCommandHandler : IRequestHandler<SalvarHistoricoNotaConselhoClasseCommand, bool>
    {
        private readonly IRepositorioHistoricoNotaConselhoClasse repositorioHistoricoNotaConselhoClasse;
        public SalvarHistoricoNotaConselhoClasseCommandHandler(IRepositorioHistoricoNotaConselhoClasse repositorioHistoricoNotaConselhoClasse)
        {
            this.repositorioHistoricoNotaConselhoClasse = repositorioHistoricoNotaConselhoClasse ?? throw new ArgumentNullException(nameof(repositorioHistoricoNotaConselhoClasse));
        }
        public Task<bool> Handle(SalvarHistoricoNotaConselhoClasseCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
