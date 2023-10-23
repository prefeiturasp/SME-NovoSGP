using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterInformesPorIdQueryHandler : IRequestHandler<ObterInformesPorIdQuery, Informativo>
    {
        private readonly IRepositorioInformativo repositorio;

        public ObterInformesPorIdQueryHandler(IRepositorioInformativo repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<Informativo> Handle(ObterInformesPorIdQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterInformes(request.Id);
        }
    }
}
