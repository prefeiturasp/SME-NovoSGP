using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDescricaoComponenteCurricularPorIdQueryHandler : IRequestHandler<ObterDescricaoComponenteCurricularPorIdQuery, string>
    {
        private readonly IRepositorioComponenteCurricular repositorioComponenteCurricular;

        public ObterDescricaoComponenteCurricularPorIdQueryHandler(IRepositorioComponenteCurricular repositorioComponenteCurricular)
        {
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
        }

        public async Task<string> Handle(ObterDescricaoComponenteCurricularPorIdQuery request, CancellationToken cancellationToken)
            => await repositorioComponenteCurricular.ObterDescricaoPorId(request.Id);
    }
}
