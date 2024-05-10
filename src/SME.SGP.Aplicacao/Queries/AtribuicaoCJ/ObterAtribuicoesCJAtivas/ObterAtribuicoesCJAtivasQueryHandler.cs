using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAtribuicoesCJAtivasQueryHandler : IRequestHandler<ObterAtribuicoesCJAtivasQuery, IEnumerable<AtribuicaoCJ>>
    {
        private readonly IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ;

        public ObterAtribuicoesCJAtivasQueryHandler(IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ)
        {
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new ArgumentNullException(nameof(repositorioAtribuicaoCJ));
        }
        public Task<IEnumerable<AtribuicaoCJ>> Handle(ObterAtribuicoesCJAtivasQuery request, CancellationToken cancellationToken)
         => Task.FromResult(repositorioAtribuicaoCJ.ObterAtribuicaoAtiva(request.CodigoRf, request.Historico));
    }
}
