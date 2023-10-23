using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirInformesPerfilsPorIdInformesCommandHandler : IRequestHandler<ExcluirInformesPerfilsPorIdInformesCommad, bool>
    {
        private readonly IRepositorioInformativoPerfil repositorio;

        public ExcluirInformesPerfilsPorIdInformesCommandHandler(IRepositorioInformativoPerfil repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<bool> Handle(ExcluirInformesPerfilsPorIdInformesCommad request, CancellationToken cancellationToken)
        {
            return repositorio.RemoverPerfisPorInformesIdAsync(request.InformesId);
        }
    }
}
