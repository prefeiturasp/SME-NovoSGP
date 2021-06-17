using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarSincronizacaoDadosFrequenciaUseCase : AbstractUseCase, IExecutarSincronizacaoDadosFrequenciaUseCase
    {
        public ExecutarSincronizacaoDadosFrequenciaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtro = param.ObterObjetoMensagem<FiltroMigracaoFrequenciaDto>();

            var turmas = await mediator.Send(new ObterTurmasIdFrequenciasExistentesPorAnosLetivosQuery(filtro.Anos));
            foreach (var turma in turmas)
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.SincronizarDadosTurmasFrequenciaMigracao, new FiltroMigracaoFrequenciaTurmaDto(turma), Guid.NewGuid(), null));

            return true;
        }
    }
}
