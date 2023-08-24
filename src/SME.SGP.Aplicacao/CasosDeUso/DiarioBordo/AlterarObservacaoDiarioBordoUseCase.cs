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
            var usuarioId = await mediator.Send(ObterUsuarioLogadoIdQuery.Instance);
            usuariosIdNotificacao = usuariosIdNotificacao?.Any() ?? false
                ? usuariosIdNotificacao
                : await ObterUsuariosQueForamNotificadosNoUltimoEnvioAsync(observacaoId);

            return await mediator.Send(new AlterarObservacaoDiarioBordoCommand(observacao, observacaoId, usuarioId, usuariosIdNotificacao));
        }

        private async Task<IEnumerable<long>> ObterUsuariosQueForamNotificadosNoUltimoEnvioAsync(long observacaoId)
        {
            var diarioBordoObs = await mediator.Send(new ObterDiarioBordoObservacaoPorObservacaoIdQuery(observacaoId));

            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            if (!diarioBordoObs.UsuarioCodigoRfDiarioBordo.Equals(usuarioLogado.CodigoRf))
            {
                var retorno = await mediator.Send(new ObterUsuarioNotificarDiarioBordoObservacaoQuery(ObterProfessorTitular(diarioBordoObs)));
                return retorno.Select(s => s.UsuarioId);
            }
            else
                return default;
        }

        private List<ProfessorTitularDisciplinaEol> ObterProfessorTitular(DiarioBordoObservacaoDto diarioBordo)
        {
            return new List<ProfessorTitularDisciplinaEol> { new ProfessorTitularDisciplinaEol { ProfessorRf = diarioBordo.UsuarioCodigoRfDiarioBordo, ProfessorNome = diarioBordo.UsuarioNomeDiarioBordo } };
        }
    }
}
