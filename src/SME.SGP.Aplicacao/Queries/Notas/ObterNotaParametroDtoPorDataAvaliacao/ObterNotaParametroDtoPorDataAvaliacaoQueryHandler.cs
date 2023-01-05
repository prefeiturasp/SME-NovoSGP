using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos.Notas;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNotaParametroDtoPorDataAvaliacaoQueryHandler : IRequestHandler<ObterNotaParametroDtoPorDataAvaliacaoQuery, NotaParametroDto>
    {
        private readonly IRepositorioNotaParametro repositorio;

        public ObterNotaParametroDtoPorDataAvaliacaoQueryHandler(IRepositorioNotaParametro repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }
        
        public Task<NotaParametroDto> Handle(ObterNotaParametroDtoPorDataAvaliacaoQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterDtoPorDataAvaliacao(request.DataAvaliacao);
        }
    }
}
