using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarInformesModalidadeCommandHandler : IRequestHandler<SalvarInformesModalidadeCommand, long>
    {
        private readonly IRepositorioInformativoModalidade repositorio;

        public SalvarInformesModalidadeCommandHandler(IRepositorioInformativoModalidade repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<long> Handle(SalvarInformesModalidadeCommand request, CancellationToken cancellationToken)
        {
            var informesModalidade = new InformativoModalidade()
            {
                InformativoId = request.InformesId,
                Modalidade = request.Modalidade
            };

            return repositorio.SalvarAsync(informesModalidade);
        }
    }
}
