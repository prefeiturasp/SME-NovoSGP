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
    public class RemoverAtribuicaoSupervisorCommandHandler : IRequestHandler<RemoverAtribuicaoSupervisorCommand, long>
    {
        private readonly IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre;
        public RemoverAtribuicaoSupervisorCommandHandler(IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre)
        {
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
        }
        public async Task<long> Handle(RemoverAtribuicaoSupervisorCommand request, CancellationToken cancellationToken)
        {
            request.SuperVisorEscolar.Excluir();
            return await repositorioSupervisorEscolaDre.SalvarAsync(request.SuperVisorEscolar);
        }
    }
}
