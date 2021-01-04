using MediatR;
using Sentry;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirOcorrenciasPorCodigoUseCase : AbstractUseCase, IExcluirOcorrenciasPorCodigoUseCase
    {
        public ExcluirOcorrenciasPorCodigoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<RetornoBaseDto> Executar(IEnumerable<long> ids)
        {
            var retorno = new RetornoBaseDto();
            if (!ids?.Any() ?? true)
            {
                retorno.Mensagens.Add("Devem ser informadas ao menos uma ocorrência para exclusão.");
                return retorno;
            }

            foreach (var id in ids)
            {
                try
                {
                    var retornoExclusao = await mediator.Send(new ExcluirOcorrenciaCommand { Id = id });
                    if (retornoExclusao.ExistemErros)
                        retorno.Mensagens.AddRange(retornoExclusao.Mensagens);
                }
                catch (Exception ex)
                {
                    SentrySdk.CaptureException(ex);
                    retorno.Mensagens.Add(ex.InnerException?.Message ?? ex.Message);
                }
            }

            return retorno;
        }
    }
}