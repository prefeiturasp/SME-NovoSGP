using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarMigracaoRelatorioPapPorAnoEletivoUseCase : AbstractUseCase, IExecutarMigracaoRelatorioPapPorAnoEletivoUseCase
    {
        private const string SEMESTRE = "S";
        public ExecutarMigracaoRelatorioPapPorAnoEletivoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var anoSemestre = param.ObterObjetoMensagem<AnoSemestreDto>();
            var idsAlunoPAP = await mediator.Send(new ObterRelatorioSemestralAlunoPAPPorAnoSemestreQuery(anoSemestre.AnoLetivo, anoSemestre.Semestre));
            var IdPeriodoPAP = await mediator.Send(new ObterIdPeriodoRelatorioPAPQuery(anoSemestre.AnoLetivo, anoSemestre.Semestre, SEMESTRE));
            
            foreach (var id in idsAlunoPAP)
            {
                var dto = new MigradorRelatorioPAPDto()
                {
                    IdRelatorioSemestralAlunoPAP = id,
                    IdPeriodoRelatorioPAP = IdPeriodoPAP
                };

                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ExecutarMigracaoRelatorioSemestralPAPPorId, dto, Guid.NewGuid(), null));
            }

            return true;
        }
    }
}
