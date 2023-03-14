using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigoTurmaInfantilPorAnoUseCase : AbstractUseCase, IObterCodigoTurmaInfantilPorAnoUseCase
    {
        public ObterCodigoTurmaInfantilPorAnoUseCase(IMediator mediator) : base(mediator)
        {

        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            try
            {
                var filtro = param.ObterObjetoMensagem<FiltroCodigoTurmaInfantilPorAnoDto>();
                var turmasInfantil = await mediator.Send(new ObterTurmasComDevolutivaPorModalidadeInfantilEAnoQuery(filtro.AnoAtual,filtro.UeId));

                if (turmasInfantil.Count() > 0)
                {
                    await PublicarMensagemTurmaIdsDevolutivas(turmasInfantil, filtro.AnoAtual);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand("Erro ao executar", LogNivel.Critico, LogContexto.Devolutivas, ex.Message));
                return false;
            }
        }

        private async Task PublicarMensagemTurmaIdsDevolutivas(IEnumerable<Infra.Dtos.DevolutivaTurmaDTO> turmasInfantil,int anoAtual)
        {
            foreach (var turma in turmasInfantil)
            {
                turma.AnoAtual = anoAtual;
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidarDevolutivasPorTurmaInfantilAula, turma, Guid.NewGuid(), null));
            }
        }
    }
}
