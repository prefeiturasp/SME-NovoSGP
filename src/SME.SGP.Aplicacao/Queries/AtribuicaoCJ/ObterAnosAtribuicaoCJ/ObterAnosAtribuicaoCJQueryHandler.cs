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
    public class ObterAnosAtribuicaoCJQueryHandler : IRequestHandler<ObterAnosAtribuicaoCJQuery, IEnumerable<int>>
    {
        private readonly IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ;

        public ObterAnosAtribuicaoCJQueryHandler(IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ)
        {
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new ArgumentNullException(nameof(repositorioAtribuicaoCJ));
        }

        public async Task<IEnumerable<int>> Handle(ObterAnosAtribuicaoCJQuery request, CancellationToken cancellationToken)
            => await repositorioAtribuicaoCJ.ObterAnosDisponiveis(request.ProfessorRF, request.ConsideraHistorico);
    }
}
