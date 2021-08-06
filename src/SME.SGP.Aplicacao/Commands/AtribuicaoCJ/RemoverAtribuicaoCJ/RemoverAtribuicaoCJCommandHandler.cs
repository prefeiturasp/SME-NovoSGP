using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RemoverAtribuicaoCJCommandHandler : IRequestHandler<RemoverAtribuicaoCJCommand, bool>
    {
        private readonly IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ;

        public RemoverAtribuicaoCJCommandHandler(IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ)
        {
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new ArgumentNullException(nameof(repositorioAtribuicaoCJ));
        }

        public async Task<bool> Handle(RemoverAtribuicaoCJCommand request, CancellationToken cancellationToken)
            => await repositorioAtribuicaoCJ.RemoverRegistros(request.DreId, request.UeId, request.TurmaId, request.ProfessorRf);

    }
}
