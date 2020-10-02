using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
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
