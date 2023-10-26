using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class IdentificarFrequenciaAlunoPresencasMaiorTotalAulasUseCase : IIdentificarFrequenciaAlunoPresencasMaiorTotalAulasUseCase
    {
        private readonly IMediator mediator;

        public IdentificarFrequenciaAlunoPresencasMaiorTotalAulasUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var anoLetivoAtual = DateTimeExtension.HorarioBrasilia().Year;
            var idsUes = await mediator.Send(new ObterTodasUesIdsQuery());

            foreach (var ueIdAtual in idsUes)
            {
                var filtro = new FiltroIdentificarFrequenciaAlunoPresencasMaiorQuantidadeAulasDto() { UeId = ueIdAtual, AnoLetivo = anoLetivoAtual };

                await mediator
                    .Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.IdentificarFrequenciaAlunoPresencasMaiorTotalAulasPorUe, filtro, Guid.NewGuid()));
            }

            return true;
        }
    }
}
