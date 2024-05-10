using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirOcorrenciaCommandHandler : IRequestHandler<ExcluirOcorrenciaCommand, RetornoBaseDto>
    {
        private readonly IRepositorioOcorrencia repositorioOcorrencia;
        private readonly IMediator mediator;

        public ExcluirOcorrenciaCommandHandler(IRepositorioOcorrencia repositorioOcorrencia,  IMediator mediator)
        {
            this.repositorioOcorrencia = repositorioOcorrencia ?? throw new ArgumentNullException(nameof(repositorioOcorrencia));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<RetornoBaseDto> Handle(ExcluirOcorrenciaCommand request, CancellationToken cancellationToken)
        {
            var retorno = new RetornoBaseDto();

            try
            {
                var ocorrencia = await repositorioOcorrencia.ObterPorIdAsync(request.Id);
                if (ocorrencia is null)
                {
                    retorno.Mensagens.Add($"Não possível localizar a ocorrência {request.Id}.");
                    return retorno;
                }

                if (ocorrencia.Excluido) return retorno;

                ocorrencia.Excluir();
                await repositorioOcorrencia.SalvarAsync(ocorrencia);
                if (!string.IsNullOrEmpty(ocorrencia?.Descricao))
                {
                    await mediator.Send(new DeletarArquivoDeRegistroExcluidoCommand(ocorrencia.Descricao, TipoArquivo.Ocorrencia.Name()));
                }

                await mediator.Send(new ExcluirOcorrenciaServidorPorIdOcorrenciaCommand(ocorrencia.Id));
            }
            catch(Exception ex)
            {
                retorno.Mensagens.Add(ex.InnerException?.Message ?? ex.Message);
            }

            return retorno;
        }
    }
}
