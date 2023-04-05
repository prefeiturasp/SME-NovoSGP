using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra;
using StackExchange.Redis;

namespace SME.SGP.Aplicacao
{
    public class ObterFaltasNaoCompensadaUseCase : AbstractUseCase, IObterFaltasNaoCompensadaUseCase
    {
        public ObterFaltasNaoCompensadaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<RegistroFaltasNaoCompensadaDto>> Executar(FiltroFaltasNaoCompensadasDto param)
        {
            var retorno = new List<RegistroFaltasNaoCompensadaDto>();

            var consulta = await mediator.Send(new ObterAusenciaParaCompensacaoQuery(param));
            retorno = param.CompensacaoId == 0 ? MaperarDto(consulta.ToList(), param, retorno) : consulta.ToList();
            return retorno;
        }

        List<RegistroFaltasNaoCompensadaDto> MaperarDto(List<RegistroFaltasNaoCompensadaDto> registroFaltasNaoCompensada, FiltroFaltasNaoCompensadasDto filtro, List<RegistroFaltasNaoCompensadaDto> retorno)
        {
            var listaSugestao = (Enumerable.Reverse(registroFaltasNaoCompensada).ToList().Where(z => !z.Sugestao).Take(filtro.QuantidadeCompensar)).ToList();
            var lista = (registroFaltasNaoCompensada.Except(listaSugestao)).ToList();
            listaSugestao.ForEach(sugestao =>
            {
                sugestao.Sugestao = true;
                retorno.Add(sugestao);
            });
            retorno.AddRange(lista);
            return retorno;
        }
    }
}