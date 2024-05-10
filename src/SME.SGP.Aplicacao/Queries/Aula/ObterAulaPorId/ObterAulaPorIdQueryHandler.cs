using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulaPorIdQueryHandler: IRequestHandler<ObterAulaPorIdQuery, Aula>
    {
        private readonly IRepositorioAula repositorioAula;

        public ObterAulaPorIdQueryHandler(IRepositorioAula repositorioAula)
        {
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
        }

        public async Task<Aula> Handle(ObterAulaPorIdQuery request, CancellationToken cancellationToken)
            => await repositorioAula.ObterPorIdAsync(request.AulaId);
    }
}
