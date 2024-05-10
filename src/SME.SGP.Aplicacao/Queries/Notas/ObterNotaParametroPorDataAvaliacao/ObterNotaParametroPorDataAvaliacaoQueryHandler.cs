using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ObterNotaParametroPorDataAvaliacaoQueryHandler : IRequestHandler<ObterNotaParametroPorDataAvaliacaoQuery,NotaParametro>
    {
        private readonly IRepositorioNotaParametro repositorioNotaParametro;

        public ObterNotaParametroPorDataAvaliacaoQueryHandler(IRepositorioNotaParametro notaParametro)
        {
            repositorioNotaParametro = notaParametro ?? throw new ArgumentNullException(nameof(notaParametro));
        }

        public async Task<NotaParametro> Handle(ObterNotaParametroPorDataAvaliacaoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioNotaParametro.ObterPorDataAvaliacao(request.DataAvaliacao);
        }
    }
}