using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Informes.InformeFoiExcluido
{
    public class InformeFoiExcluidoQueryHandler : IRequestHandler<InformeFoiExcluidoQuery, bool>
    {
        private readonly IRepositorioInformativo repositorio;

        public InformeFoiExcluidoQueryHandler(IRepositorioInformativo repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<bool> Handle(InformeFoiExcluidoQuery request, CancellationToken cancellationToken)
        {
            return repositorio.InformeFoiExcluido(request.Id);
        }
    }
}
