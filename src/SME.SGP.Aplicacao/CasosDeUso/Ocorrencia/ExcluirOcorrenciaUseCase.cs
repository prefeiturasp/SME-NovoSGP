using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirOcorrenciaUseCase : AbstractUseCase, IExcluirOcorrenciaUseCase
    {
        public ExcluirOcorrenciaUseCase(IMediator mediator) : base(mediator)
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
                var retornoExclusao = await mediator.Send(new ExcluirOcorrenciaCommand { Id = id });
                if (retornoExclusao.ExistemErros)
                    retorno.Mensagens.AddRange(retornoExclusao.Mensagens);
            }

            return retorno;
        }
    }
}
