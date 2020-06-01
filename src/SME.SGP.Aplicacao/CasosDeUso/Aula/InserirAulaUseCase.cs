using MediatR;
using SME.SGP.Aplicacao.Commands.Aula;
using SME.SGP.Aplicacao.Commands.Aula.InserirAula;
using SME.SGP.Aplicacao.Queries.Usuario.ObterUsuarioPossuiPermissaoNaTurmaEDisciplina;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.Aula
{
    public static class InserirAulaUseCase
    {
        public static async Task<RetornoBaseDto> Executar(IMediator mediator, InserirAulaCommand inserirAulaCommand)
        {

            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());


            var usuarioPodePersistirTurmaNaData = await mediator.Send(new ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery(inserirAulaCommand.DisciplinaId, inserirAulaCommand.DataAula, usuarioLogado));

            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery { TurmaCodigo = inserirAulaCommand.TurmaId });

            var tipoCalendario = await mediator.Send(new ObterTipoCalendarioPorIdQuery { Id = inserirAulaCommand.TipoCalendarioId });

            if (inserirAulaCommand.RecorrenciaAula == RecorrenciaAula.AulaUnica)
            {
                await mediator.Send(new InserirAulaUnicaCommand(usuarioLogado, inserirAulaCommand.DataAula,turma, inserirAulaCommand.DisciplinaId, usuarioLogado.CodigoRf,tipoCalendario));
            }
            else
            {

            }

            await mediator.Send(inserirAulaCommand);
            return null;
        }
    }
}
