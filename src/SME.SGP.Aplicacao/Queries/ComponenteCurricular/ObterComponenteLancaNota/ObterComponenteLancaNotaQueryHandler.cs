using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponenteLancaNotaQueryHandler : IRequestHandler<ObterComponenteLancaNotaQuery, bool>
    {
        private readonly IRepositorioComponenteCurricular repositorioComponenteCurricular;

        public ObterComponenteLancaNotaQueryHandler(IRepositorioComponenteCurricular repositorioComponenteCurricular)
        {
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
        }

        public async Task<bool> Handle(ObterComponenteLancaNotaQuery request, CancellationToken cancellationToken)
            => await repositorioComponenteCurricular.LancaNota(request.ComponenteCurricularId);
    }
}
