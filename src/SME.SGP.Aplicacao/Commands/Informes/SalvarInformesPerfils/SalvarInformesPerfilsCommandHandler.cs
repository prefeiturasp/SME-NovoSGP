using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarInformesPerfilsCommandHandler : IRequestHandler<SalvarInformesPerfilsCommand, long>
    {
        private readonly IRepositorioInformativoPerfil repositorio;

        public SalvarInformesPerfilsCommandHandler(IRepositorioInformativoPerfil repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<long> Handle(SalvarInformesPerfilsCommand request, CancellationToken cancellationToken)
        {
            var perfilInformes = new InformativoPerfil()
            {
                InformativoId = request.InformesId,
                CodigoPerfil = request.PerfilId
            };

            return await repositorio.SalvarAsync(perfilInformes);
        }
    }
}
