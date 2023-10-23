using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExisteInformePorIdQueryHandler : IRequestHandler<ExisteInformePorIdQuery, bool>
    {
        private readonly IRepositorioInformativo repositorio;

        public ExisteInformePorIdQueryHandler(IRepositorioInformativo repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<bool> Handle(ExisteInformePorIdQuery request, CancellationToken cancellationToken)
        {
            return repositorio.Exists(request.Id);
        }
    }
}
