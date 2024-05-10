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
    class ObterAtribuicaoCJPorDreUeTurmaRFQueryHandler : IRequestHandler<ObterAtribuicaoCJPorDreUeTurmaRFQuery, IEnumerable<AtribuicaoCJ>>
    {
        private readonly IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ;

        public ObterAtribuicaoCJPorDreUeTurmaRFQueryHandler(IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ)
        {
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new System.ArgumentNullException(nameof(repositorioAtribuicaoCJ));
        }

        public Task<IEnumerable<AtribuicaoCJ>> Handle(ObterAtribuicaoCJPorDreUeTurmaRFQuery request, CancellationToken cancellationToken)
         => repositorioAtribuicaoCJ.ObterAtribuicaoCJPorDreUeTurmaRF(request.TurmaId, request.DreCodigo, request.UeCodigo, request.ProfessorRf);
    }
}
