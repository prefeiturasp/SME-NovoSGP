using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirOcorrenciaCommandHandler : IRequestHandler<ExcluirOcorrenciaCommand, RetornoBaseDto>
    {
        private readonly IRepositorioOcorrencia repositorioOcorrencia;

        public ExcluirOcorrenciaCommandHandler(IRepositorioOcorrencia repositorioOcorrencia)
        {
            this.repositorioOcorrencia = repositorioOcorrencia;
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
            }
            catch(Exception ex)
            {
                retorno.Mensagens.Add(ex.InnerException?.Message ?? ex.Message);
            }

            return retorno;
        }
    }
}
