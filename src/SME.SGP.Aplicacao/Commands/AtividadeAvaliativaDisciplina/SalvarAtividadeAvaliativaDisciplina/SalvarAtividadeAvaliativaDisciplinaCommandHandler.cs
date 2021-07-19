using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarAtividadeAvaliativaDisciplinaCommandHandler : AsyncRequestHandler<SalvarAtividadeAvaliativaDisciplinaCommand>
    {
        private readonly IRepositorioAtividadeAvaliativaDisciplina repositorioAtividadeAvaliativaDisciplina;

        public SalvarAtividadeAvaliativaDisciplinaCommandHandler(IRepositorioAtividadeAvaliativaDisciplina repositorioAtividadeAvaliativaDisciplina)
        {
            this.repositorioAtividadeAvaliativaDisciplina = repositorioAtividadeAvaliativaDisciplina ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativaDisciplina));
        }

        protected override async Task Handle(SalvarAtividadeAvaliativaDisciplinaCommand request, CancellationToken cancellationToken)
        {
            await repositorioAtividadeAvaliativaDisciplina.SalvarAsync(new Dominio.AtividadeAvaliativaDisciplina(request.AtividadeAvaliativaId, request.ComponenteCurricularId));
        }
    }
}
