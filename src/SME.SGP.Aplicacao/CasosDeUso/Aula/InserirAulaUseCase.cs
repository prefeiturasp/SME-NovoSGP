using MediatR;
using SME.SGP.Aplicacao.Commands.Aulas;
using SME.SGP.Aplicacao.Commands.Aulas.InserirAula;
using SME.SGP.Aplicacao.Queries.Aula.ObterAulasPorDataTurmaDisciplinaProfessorRf;
using SME.SGP.Aplicacao.Queries.ComponentesCurriculares.ObterComponentesCurricularesDoProfessorNaTurma;
using SME.SGP.Aplicacao.Queries.Evento.ObterEhDiaLetivo;
using SME.SGP.Aplicacao.Queries.Usuario.ObterUsuarioPossuiPermissaoNaTurmaEDisciplina;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
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
                if (aulasExistentes != null && aulasExistentes.Any(c => c.TipoAula == inserirAulaCommand.TipoAula))
                    throw new NegocioException("Já existe uma aula criada neste dia para este componente curricular");

                //VALIDAR SE A AULA É PARA COMPONENTES QUE O PROFESSOR ESTÁ ATRIBUIDO
                if (usuarioLogado.EhProfessorCj())
                {
                    if (usuarioLogado.EhProfessorCj() && inserirAulaCommand.Quantidade > 2)
                        throw new NegocioException("Quantidade de aulas por dia/disciplina excedido.");

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

                    //VALIDAR SE O PROFESSOR POSSUI ATRIBUIÇÃO NA TURMA NESSA DATA
                    var usuarioPodePersistirTurmaNaData = await mediator.Send(new ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery(inserirAulaCommand.CodigoComponenteCurricular, inserirAulaCommand.CodigoTurma, inserirAulaCommand.DataAula, usuarioLogado));
                    if (!usuarioPodePersistirTurmaNaData)
                        throw new NegocioException("Você não pode fazer alterações ou inclusões nesta turma, disciplina e data.");
                }


                //VALIDAR DIA LETIVO
                var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(inserirAulaCommand.CodigoTurma));


                var ehDiaLetivo = await mediator.Send(new ObterDataEhDiaLetivoPorTipoCalendarioQuery(inserirAulaCommand.TipoCalendarioId, inserirAulaCommand.DataAula, turma.Ue.Dre.CodigoDre, turma.Ue.CodigoUe));
                if (!ehDiaLetivo)
                    throw new NegocioException("Não é possível cadastrar essa aula pois a data informada está fora do período letivo.");


                //VALIDAR PERIODO ABERTO
                var bimestreAula = await mediator.Send(new ObterBimestreAtualQuery()
                {
                    Turma = turma,
                    DataReferencia = inserirAulaCommand.DataAula
                });

                var aulaNoAnoLetivoAtual = inserirAulaCommand.DataAula.Year == DateTime.Now.Year;

                var periodoAberto = await mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, inserirAulaCommand.DataAula, bimestreAula, aulaNoAnoLetivoAtual));
                if (!periodoAberto)
                    throw new NegocioException("Não é possível cadastrar essa aula pois o período não está aberto.");


                //TODO validar aula reposição

                //VALIDAR GRADE
                if (inserirAulaCommand.EhRegencia)
                {
                    if (aulasExistentes != null && aulasExistentes.Any(c => c.TipoAula != TipoAula.Reposicao))
                    {
                        if (turma.ModalidadeCodigo == Modalidade.EJA)
                            throw new NegocioException("Para regência de EJA só é permitido a criação de 5 aulas por dia.");
                        else throw new NegocioException("Para regência de classe só é permitido a criação de 1 (uma) aula por dia.");
                    }
                }
                else
                {
                    //// Busca quantidade de aulas semanais da grade de aula
                    //int semana = UtilData.ObterSemanaDoAno(aula.DataAula);

                    //var gradeAulas= await mediator.Send(new ObterHorasGradePorComponenteQuery()
                    ////var gradeAulas = await consultasGrade.ObterGradeAulasTurmaProfessor(aula.TurmaId, Convert.ToInt64(aula.DisciplinaId), semana, aula.DataAula, usuario.CodigoRf);

                    //var quantidadeAulasRestantes = gradeAulas == null ? int.MaxValue : gradeAulas.QuantidadeAulasRestante;
                    //if ((gradeAulas != null) && (quantidadeAulasRestantes < aula.Quantidade))
                    //    throw new NegocioException("Quantidade de aulas superior ao limíte de aulas da grade.");
                }

                //


                var tipoCalendario = await mediator.Send(new ObterTipoCalendarioPorIdQuery { Id = inserirAulaCommand.TipoCalendarioId });
                return await mediator.Send(new InserirAulaUnicaCommand(usuarioLogado, inserirAulaCommand.DataAula, inserirAulaCommand.Quantidade, turma, inserirAulaCommand.CodigoComponenteCurricular, inserirAulaCommand.NomeComponenteCurricular, tipoCalendario, inserirAulaCommand.TipoAula, inserirAulaCommand.CodigoUe));
            }
            else
            {
                mediator.Enfileirar(new InserirAulaRecorrenteCommand());
                return new RetornoBaseDto("Serão cadastradas aulas recorrentes, em breve você receberá uma notificação com o resultado do processamento.");
            }
        }
    }
}
