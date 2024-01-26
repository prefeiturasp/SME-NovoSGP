using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AulaPossuiAvaliacaoQueryHandler : IRequestHandler<AulaPossuiAvaliacaoQuery, bool>
    {
        private readonly IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa;

        public AulaPossuiAvaliacaoQueryHandler(IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa)
        {
            this.repositorioAtividadeAvaliativa = repositorioAtividadeAvaliativa ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativa));
        }

        public async Task<bool> Handle(AulaPossuiAvaliacaoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioAtividadeAvaliativa.VerificarSeExisteAvaliacao(request.Aula.DataAula.Date, request.Aula.UeId, request.Aula.TurmaId, request.CodigoRf, request.Aula.DisciplinaId);
        }
    }
}
