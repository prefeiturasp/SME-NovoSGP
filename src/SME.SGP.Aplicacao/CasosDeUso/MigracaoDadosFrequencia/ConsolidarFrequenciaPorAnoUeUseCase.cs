using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidarFrequenciaPorAnoUeUseCase : AbstractUseCase, IConsolidarFrequenciaPorAnoUeUseCase
    {
        public ConsolidarFrequenciaPorAnoUeUseCase(IMediator mediator) : base(mediator) { }

        public async Task<bool> Executar(int anoLetivo, long ueId)
        {
            try
            {
                string textoBaseMsgNegocioException = "Não foi possível localizar";
                var ue = await mediator.Send(new ObterUePorIdQuery(ueId));
                if (ue == null)
                    throw new NegocioException($"{textoBaseMsgNegocioException} a Ue:{ueId}");

                bool consideraNovasModalidades = false;
                var modadlidadesQueSeraoIgnoradas = await mediator.Send(new ObterNovasModalidadesPorAnoQuery(anoLetivo, consideraNovasModalidades));
                var modalidadesUe = await mediator.Send(new ObterModalidadesPorUeEAnoLetivoQuery(ue.CodigoUe, anoLetivo, modadlidadesQueSeraoIgnoradas));
                if (modalidadesUe == null)
                    throw new NegocioException($"{textoBaseMsgNegocioException} as modalidades da Ue:{ueId}");

                var turmas = await mediator.Send(new ObterTurmasPorUeModalidadesAnoQuery(ue.Id, modalidadesUe.Select(x => (Modalidade)x.Id).ToArray(), anoLetivo));
                if (turmas == null)
                    throw new NegocioException($"{textoBaseMsgNegocioException} as turmas da Ue:{ueId}");

                foreach (var turma in turmas)
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.SincronizarDadosTurmasFrequenciaMigracao, new FiltroMigracaoFrequenciaTurmaDto(turma.CodigoTurma), Guid.NewGuid(), null));

                return true;
            }
            catch (Exception e)
            {
                throw new NegocioException($"Erro ao consolidar as frequências da Ue:{ueId} -- {e.Message}");
            }
        }

    }
}
