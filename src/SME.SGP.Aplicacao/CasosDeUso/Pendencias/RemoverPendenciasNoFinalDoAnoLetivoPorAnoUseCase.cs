using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class RemoverPendenciasNoFinalDoAnoLetivoPorAnoUseCase : AbstractUseCase, IRemoverPendenciasNoFinalDoAnoLetivoPorAnoUseCase
    {
        public RemoverPendenciasNoFinalDoAnoLetivoPorAnoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            _ = int.TryParse(param?.Mensagem?.ToString(), out int ano);

            ano = ano == 0 ? DateTimeExtension.HorarioBrasilia().AddYears(-1).Year : ano;

            if (ano < 2014 || ano >= DateTimeExtension.HorarioBrasilia().Year)
                return false;

            var idsDres = await mediator.Send(ObterIdsDresQuery.Instance);

            foreach (var idDre in idsDres)
            {
                var filtro = new FiltroRemoverPendenciaFinalAnoLetivoDto(ano, idDre);
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpPendencias.RotaExecutarExclusaoPendenciasNoFinalDoAnoLetivoPorUe, filtro, Guid.NewGuid()));
            }

            return true;
        }
    }
}
