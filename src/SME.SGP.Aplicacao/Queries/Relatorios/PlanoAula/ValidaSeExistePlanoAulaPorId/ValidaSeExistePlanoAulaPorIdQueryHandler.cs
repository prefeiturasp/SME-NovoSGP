using System;
using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao.Queries
{
    public class ValidaSeExistePlanoAulaPorIdQueryHandler : IRequestHandler<ValidaSeExistePlanoAulaPorIdQuery, bool>
    {
        private readonly IRepositorioPlanoAula repositorioPlanoAula;

        public ValidaSeExistePlanoAulaPorIdQueryHandler(IRepositorioPlanoAula repositorioPlanoAula)
        {
            this.repositorioPlanoAula = repositorioPlanoAula ?? throw new ArgumentNullException(nameof(repositorioPlanoAula));
        }

        public async Task<bool> Handle(ValidaSeExistePlanoAulaPorIdQuery request, CancellationToken cancellationToken)
        {

            var planoAula = await repositorioPlanoAula.ObterPorIdAsync(request.Id);
            if(planoAula == null)
                throw new NegocioException("Não foi possível encontrar o plano de aula");

            return true;
        }
    }
}
