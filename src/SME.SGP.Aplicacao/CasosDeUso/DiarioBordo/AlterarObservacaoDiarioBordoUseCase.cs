using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarObservacaoDiarioBordoUseCase : IAlterarObservacaoDiarioBordoUseCase
    {
        private readonly IMediator mediator;

        public AlterarObservacaoDiarioBordoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<AuditoriaDto> Executar(string observacao, long observacaoId, IEnumerable<long> usuariosIdNotificacao)
        {
            var usuarioId = await mediator.Send(new ObterUsuarioLogadoIdQuery());
            usuariosIdNotificacao = usuariosIdNotificacao?.Any() ?? false
                ? usuariosIdNotificacao
                : await ObterUsuariosQueForamNotificadosNoUltimoEnvioAsync(observacaoId);

            return await mediator.Send(new AlterarObservacaoDiarioBordoCommand(observacao, observacaoId, usuarioId, usuariosIdNotificacao));
        }

        private async Task<IEnumerable<long>> ObterUsuariosQueForamNotificadosNoUltimoEnvioAsync(long observacaoId)
        {
            var turma = await mediator.Send(new ObterTurmaDiarioBordoAulaPorObservacaoIdQuery(observacaoId));
            if (turma is null)
                throw new NegocioException("A turma informada não foi encontrada.");

            var professoresTurmaObrigatoriosReceberNotificacao = await mediator.Send(new ObterProfessoresTitularesDaTurmaCompletosQuery(turma.CodigoTurma));
            if (!professoresTurmaObrigatoriosReceberNotificacao?.Any() ?? true)
                throw new NegocioException("Nenhum professor para a turma informada foi encontrada.");

            var usuariosNotificados = await mediator.Send(new ObterUsuarioNotificarDiarioBordoObservacaoQuery(turma, professoresTurmaObrigatoriosReceberNotificacao, observacaoId));
            return usuariosNotificados?.Select(x => x.UsuarioId).ToList();
        }
    }
}
