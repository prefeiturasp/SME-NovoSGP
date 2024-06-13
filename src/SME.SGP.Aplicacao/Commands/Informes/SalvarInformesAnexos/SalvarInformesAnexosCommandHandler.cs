using MediatR;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarInformesAnexosCommandHandler : IRequestHandler<SalvarInformesAnexosCommand, bool>
    {
        private readonly IRepositorioInformativoAnexo repositorio;
        private readonly IMediator mediator;

        public SalvarInformesAnexosCommandHandler(IRepositorioInformativoAnexo repositorio, IMediator mediator)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(SalvarInformesAnexosCommand request, CancellationToken cancellationToken)
        {
            var arquivos = (await mediator.Send(new ObterArquivosPorCodigosQuery(request.CodigosAnexo))).ToList();
            foreach (var informativoAnexo in arquivos.Select(arquivo => new InformativoAnexo
            {
                ArquivoId = arquivo.Id,
                InformativoId = request.InformativoId
            }))
            {
                await repositorio.SalvarAsync(informativoAnexo);
            }

            return true;
        }
    }
}
