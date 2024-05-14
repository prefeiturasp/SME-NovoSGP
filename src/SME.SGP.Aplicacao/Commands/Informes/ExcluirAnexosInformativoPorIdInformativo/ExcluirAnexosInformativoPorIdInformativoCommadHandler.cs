using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirAnexosInformativoPorIdInformativoCommadHandler : IRequestHandler<ExcluirAnexosInformativoPorIdInformativoCommad, bool>
    {
        private readonly IRepositorioInformativoAnexo repositorio;
        private readonly IMediator mediator;

        public ExcluirAnexosInformativoPorIdInformativoCommadHandler(IRepositorioInformativoAnexo repositorio, IMediator mediator)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExcluirAnexosInformativoPorIdInformativoCommad request, CancellationToken cancellationToken)
        {
            var anexosInformativo = (await mediator.Send(new ObterAnexosPorInformativoIdQuery(request.InformativoId))).ToList();

            if (anexosInformativo.PossuiRegistros())
            {
                var arquivosExclusao =
                    await mediator.Send(
                        new ObterArquivosPorIdsQuery(anexosInformativo.Select(c => c.ArquivoId).ToArray()));
                await repositorio.RemoverPorInformativoIdAsync(request.InformativoId);
                foreach (var arquivo in arquivosExclusao)
                {
                    await mediator.Send(new ExcluirArquivoRepositorioPorIdCommand(arquivo.Id));
                    var extencao = Path.GetExtension(arquivo.Nome);
                    var filtro = new FiltroExcluirArquivoArmazenamentoDto { ArquivoNome = $"{arquivo.Codigo}{extencao}" };
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RemoverArquivoArmazenamento, filtro, Guid.NewGuid()));
                }
                return true;
            }
            return false;
        }
    }
}
