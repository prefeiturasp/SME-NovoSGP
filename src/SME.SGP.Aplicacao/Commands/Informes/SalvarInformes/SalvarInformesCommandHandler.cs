using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarInformesCommandHandler : IRequestHandler<SalvarInformesCommand, Informativo>
    {
        private readonly IRepositorioInformativo repositorio;

        public SalvarInformesCommandHandler(IRepositorioInformativo repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<Informativo> Handle(SalvarInformesCommand request, CancellationToken cancellationToken)
        {
            var informes = new Informativo()
            {
                DreId = request.InformesDto.DreId,
                UeId = request.InformesDto.UeId,
                DataEnvio = DateTimeExtension.HorarioBrasilia().Date,
                Texto = request.InformesDto.Texto,
                Titulo = request.InformesDto.Titulo
            };

            await this.repositorio.SalvarAsync(informes);

            return informes;
        }
    }
}
