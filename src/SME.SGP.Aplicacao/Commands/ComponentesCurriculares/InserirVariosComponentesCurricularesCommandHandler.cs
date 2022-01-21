using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands
{
    public class InserirVariosComponentesCurricularesCommandHandler : IRequestHandler<InserirVariosComponentesCurricularesCommand, bool>
    {
        private readonly IRepositorioComponenteCurricular repositorioComponenteCurricular;

        public InserirVariosComponentesCurricularesCommandHandler(IRepositorioComponenteCurricular repositorioComponenteCurricular)
        {
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
        }

        public async Task<bool> Handle(InserirVariosComponentesCurricularesCommand request, CancellationToken cancellationToken)
        {
            repositorioComponenteCurricular.SalvarVarias(request.ComponentesCurriculares);

            return true;
        }
    }
}
