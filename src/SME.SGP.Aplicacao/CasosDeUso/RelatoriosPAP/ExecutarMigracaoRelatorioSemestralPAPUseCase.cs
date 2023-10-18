using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarMigracaoRelatorioSemestralPAPUseCase : AbstractUseCase, IExecutarMigracaoRelatorioSemestralPAPUseCase
    {
        public ExecutarMigracaoRelatorioSemestralPAPUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var anosLetivos = new List<int>() { 2021, 2022, 2023 };
            var semestres = new List<int>() { 1, 2 };

            foreach (int ano in anosLetivos)
            {
                foreach (int semestre in semestres)
                {
                    var filtro = new AnoSemestreDto() { AnoLetivo = ano, Semestre = semestre };
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ExecutarMigracaoRelatorioSemestralPAPPorAnoLetivo, filtro, Guid.NewGuid(), null));
                }
            }

            return true;
        }
    }
}