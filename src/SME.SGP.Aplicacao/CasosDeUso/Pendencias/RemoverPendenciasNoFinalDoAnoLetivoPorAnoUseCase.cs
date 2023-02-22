using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RemoverPendenciasNoFinalDoAnoLetivoPorAnoUseCase : AbstractUseCase, IRemoverPendenciasNoFinalDoAnoLetivoPorAnoUseCase
    {
        public RemoverPendenciasNoFinalDoAnoLetivoPorAnoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            int.TryParse(param.Mensagem.ToString(), out int ano);

            ano = ano > 0 ? DateTime.Now.AddYears(-1).Year : ano;

            var idsDres = await mediator.Send(new ObterIdsDresQuery());

            foreach (var idDre in idsDres)
            {
                var filtro = new FiltroRemoverPendenciaFinalAnoLetivoDto(ano, idDre);
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpPendencias.RotaExecutarExclusaoPendenciasNoFinalDoAnoLetivoPorUe, filtro, Guid.NewGuid()));
            }

            return true;
        }
    }
}
