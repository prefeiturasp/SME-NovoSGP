using MediatR;
using SME.SGP.Aplicacao.Commands.Aulas;
using SME.SGP.Aplicacao.Commands.Aulas.InserirAula;
using SME.SGP.Aplicacao.Queries.Aula.ObterAulasPorDataTurmaDisciplinaProfessorRf;
using SME.SGP.Aplicacao.Queries.ComponentesCurriculares.ObterComponentesCurricularesDoProfessorNaTurma;
using SME.SGP.Aplicacao.Queries.Evento.ObterDataPossuiEventoLiberacaoExcepcional;
using SME.SGP.Aplicacao.Queries.Usuario.ObterUsuarioPossuiPermissaoNaTurmaEDisciplina;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.Aula
{
    public class InserirAulaUseCase
    {
        public async Task<RetornoBaseDto> Executar(IMediator mediator, InserirAulaCommand inserirAulaCommand)
        {
            if (inserirAulaCommand.RecorrenciaAula == RecorrenciaAula.AulaUnica)
            {
                var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());


                var aulasExistentes = await mediator.Send(new ObterAulasPorDataTurmaComponenteCurricularEProfessorQuery(inserirAulaCommand.DataAula, inserirAulaCommand.CodigoTurma, inserirAulaCommand.CodigoComponenteCurricular, usuarioLogado.CodigoRf));
                if (aulasExistentes != null)
                    throw new NegocioException("Já existe uma aula criada neste dia para este componente curricular");

                if (usuarioLogado.EhProfessorCj())
                {
                    var componentesCurricularesDoProfessorCJ = await mediator.Send(new ObterComponentesCurricularesDoProfessorCJNaTurmaQuery(usuarioLogado.Login));
                    if (componentesCurricularesDoProfessorCJ == null || !componentesCurricularesDoProfessorCJ.Any(c => c.TurmaId == inserirAulaCommand.CodigoTurma && c.DisciplinaId == inserirAulaCommand.CodigoComponenteCurricular))
                    {
                        throw new NegocioException("Você não pode criar aulas para essa Turma.");
                    }
                }
                else
                {
                    var componentesCurricularesDoProfessor = await mediator.Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(inserirAulaCommand.CodigoTurma, usuarioLogado.Login, usuarioLogado.PerfilAtual));
                    if (componentesCurricularesDoProfessor == null || !componentesCurricularesDoProfessor.Any(c => c.Codigo == inserirAulaCommand.CodigoComponenteCurricular))
                    {
                        throw new NegocioException("Você não pode criar aulas para essa Turma.");
                    }

                    var usuarioPodePersistirTurmaNaData = await mediator.Send(new ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery(inserirAulaCommand.CodigoComponenteCurricular, inserirAulaCommand.DataAula, usuarioLogado));
                    if (!usuarioPodePersistirTurmaNaData)
                        throw new NegocioException("Você não pode fazer alterações ou inclusões nesta turma, disciplina e data.");
                }
                var turma = await mediator.Send(new ObterTurmaPorCodigoQuery { TurmaCodigo = inserirAulaCommand.CodigoTurma });

                var ehDiaLetivo = await mediator.Send(new ObterTemEventoLetivoPorCalendarioEDiaQuery(inserirAulaCommand.TipoCalendarioId, inserirAulaCommand.DataAula, turma.Ue.Dre.CodigoDre, inserirAulaCommand.UeId));
                if (!ehDiaLetivo)
                {
                    var possuiLiberacaoExcepcional = await mediator.Send(new ObterDataPossuiEventoLiberacaoExcepcionalQuery(inserirAulaCommand.DataAula, inserirAulaCommand.TipoCalendarioId, inserirAulaCommand.UeId));
                    if (!possuiLiberacaoExcepcional)
                        throw new NegocioException("Não é possível cadastrar essa aula pois a data informada está fora do período letivo.");


                }



                var tipoCalendario = await mediator.Send(new ObterTipoCalendarioPorIdQuery { Id = inserirAulaCommand.TipoCalendarioId });
                await mediator.Send(new InserirAulaUnicaCommand(usuarioLogado, inserirAulaCommand.DataAula, inserirAulaCommand.Quantidade, turma, inserirAulaCommand.CodigoComponenteCurricular, inserirAulaCommand.NomeComponenteCurricular, tipoCalendario, inserirAulaCommand.TipoAula));
            }
            else
            {
                mediator.Enfileirar(new InserirAulaRecorrenteCommand());
            }






            return null;
        }
    }
}
