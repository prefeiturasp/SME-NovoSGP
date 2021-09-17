using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarResultadoInsatisfatorioCommandHandler : IRequestHandler<NotificarResultadoInsatisfatorioCommand, bool>
    {
        private readonly IMediator mediator;

        public NotificarResultadoInsatisfatorioCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(NotificarResultadoInsatisfatorioCommand request, CancellationToken cancellationToken)
        {
            DateTime dataNotificacao = DateTime.Now.AddDays(request.Dias).Date;
            var periodoFechamentoBimestres = await mediator.Send(new ObterPeriodosEscolaresPorModalidadeDataFechamentoQuery((int)request.ModalidadeTipoCalendario, dataNotificacao));

            var percentualReprovacao = double.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.PercentualAlunosInsuficientes, DateTime.Today.Year)));
            var mediaBimestre = double.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.MediaBimestre, DateTime.Today.Year)));
            var componentes = await mediator.Send(new ObterComponentesCurricularesQuery());

            List<NotificarResultadoInsatisfatorioDto> listaNotificacoes = new List<NotificarResultadoInsatisfatorioDto>();

            foreach (var periodoFechamentoBimestre in periodoFechamentoBimestres)
            {
                if (periodoFechamentoBimestre.PeriodoFechamento.UeId != null)
                {
                    var alunosComNotaLancada = await mediator.Send(new ObterAlunosComNotaLancadaPorPeriodoEscolarUEQuery(periodoFechamentoBimestre.PeriodoFechamento.UeId.GetValueOrDefault(), periodoFechamentoBimestre.PeriodoEscolarId));

                    if (alunosComNotaLancada != null)
                    {
                        var turmasAlunosComNotaLancada = alunosComNotaLancada.GroupBy(a => a.TurmaId);
                        foreach (var turmaId in turmasAlunosComNotaLancada)
                        {
                            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(turmaId.Key));
                            if (turma.UeId == periodoFechamentoBimestre.PeriodoFechamento.UeId)
                            {
                                var turmaNotificacao = new NotificarResultadoInsatisfatorioDto();
                                turmaNotificacao.TurmaNome = $"{turma.ModalidadeCodigo.ShortName()}-{turma.Nome}";
                                turmaNotificacao.TurmaModalidade = turma.ModalidadeCodigo.Name();

                                var componentesCurricularesAlunosComNotaLancada = alunosComNotaLancada.Where(a => a.TurmaId == turmaId.Key).GroupBy(a => a.ComponenteCurricularId);
                                foreach (var componenteCurricularId in componentesCurricularesAlunosComNotaLancada)
                                {
                                    var alunosComNota = alunosComNotaLancada.Where(a => a.TurmaId == turmaId.Key && a.ComponenteCurricularId == componenteCurricularId.Key);

                                    var alunosRetorno = VerificaAlunosResultadoInsatisfatorio(alunosComNota, percentualReprovacao, turma, componenteCurricularId.Key, mediaBimestre);

                                    if (alunosRetorno.Any())
                                    {
                                        var componenteNotificacao = new NotificarResultadoInsatisfatorioCCDto();
                                        var obterComponenteCurricular = componentes.FirstOrDefault(c => long.Parse(c.Codigo) == componenteCurricularId.Key);

                                        var aluno = alunosRetorno.FirstOrDefault();
                                        var professoresTurmaDisciplina = await mediator.Send(new ProfessoresTurmaDisciplinaQuery(turma.CodigoTurma, obterComponenteCurricular.Codigo, dataNotificacao));

                                        componenteNotificacao.ComponenteCurricularNome = obterComponenteCurricular.Descricao;
                                        if (professoresTurmaDisciplina != null && professoresTurmaDisciplina.Any())
                                        {
                                            var professorTurma = professoresTurmaDisciplina.FirstOrDefault();
                                            componenteNotificacao.Professor = $"{professorTurma.NomeProfessor} ({professorTurma.CodigoRf})";
                                        }
                                        componenteNotificacao.Justificativa = alunosRetorno.FirstOrDefault().Justificativa;

                                        turmaNotificacao.ComponentesCurriculares.Add(componenteNotificacao);
                                    }

                                }

                                if (turmaNotificacao.ComponentesCurriculares.Any())
                                {
                                    turmaNotificacao.ComponentesCurriculares = turmaNotificacao.ComponentesCurriculares.OrderBy(c => c.ComponenteCurricularNome).ToList();
                                    listaNotificacoes.Add(turmaNotificacao);
                                }
                            }
                        }
                    }
                }

                if (listaNotificacoes.Any())
                {
                    await EnviarNotificacoes(listaNotificacoes.OrderBy(n => n.TurmaNome).ToList(), periodoFechamentoBimestre);
                    listaNotificacoes.Clear();
                }
            }


            return true;
        }

        private async Task EnviarNotificacoes(List<NotificarResultadoInsatisfatorioDto> listaNotificacoes, PeriodoFechamentoBimestre periodoFechamentoBimestre)
        {
            var titulo = $"Turmas com resultados insatisfatórios no {periodoFechamentoBimestre.PeriodoEscolar.Bimestre}º bimestre";
            var mensagem = new StringBuilder($"As turmas e componentes curriculares abaixo da <b>{periodoFechamentoBimestre.PeriodoFechamento.Ue.TipoEscola.ShortName()} {periodoFechamentoBimestre.PeriodoFechamento.Ue.Nome} ({periodoFechamentoBimestre.PeriodoFechamento.Ue.Dre.Abreviacao})</b> tiveram mais de 50% dos estudantes com resultado insatisfatório no <b>{periodoFechamentoBimestre.PeriodoEscolar.Bimestre}º bimestre</b>:");

            mensagem.Append("<table style='margin-left: auto; margin-right: auto; margin-top: 10px' border='2' cellpadding='5'>");
            foreach (var turmasPorModalidade in listaNotificacoes.GroupBy(c => c.TurmaModalidade))
            {
                mensagem.Append(ObterHeaderModalidade(turmasPorModalidade.Key));

                foreach (var turma in turmasPorModalidade)
                {
                    mensagem.Append(MontarLinhaDaTurma(turma));
                }
            }
            mensagem.Append("</table>");

            await mediator.Send(new EnviarNotificacaoCommand(titulo, mensagem.ToString(), NotificacaoCategoria.Aviso, NotificacaoTipo.Fechamento, ObterCargosGestaoEscola(),
                periodoFechamentoBimestre.PeriodoFechamento.Ue.Dre.CodigoDre,
                periodoFechamentoBimestre.PeriodoFechamento.Ue.CodigoUe));
        }

        private List<AlunosFechamentoNotaDto> VerificaAlunosResultadoInsatisfatorio(IEnumerable<AlunosFechamentoNotaDto> alunosComNota, double percentualReprovacao, Turma turma, long componenteCurricularId, double mediaBimestre)
        {
            List<AlunosFechamentoNotaDto> alunos = new List<AlunosFechamentoNotaDto>();

            if (alunosComNota.FirstOrDefault().EhConceito)
            {
                var alunosTurmaComNotaAbaixo = alunosComNota.Where(a => a.EhConceito && a.TurmaId == turma.Id && a.ComponenteCurricularId == componenteCurricularId && !a.NotaConceitoAprovado);
                var totalAlunosRI = ((alunosTurmaComNotaAbaixo.Count() / (double)alunosComNota.Count()) * 100);
                if (totalAlunosRI > percentualReprovacao)
                {
                    alunos = alunosTurmaComNotaAbaixo.ToList();
                }
            }
            else
            {
                var alunosTurmaComNotaAbaixo = alunosComNota.Where(a => !a.EhConceito && a.TurmaId == turma.Id && a.ComponenteCurricularId == componenteCurricularId && a.Nota < mediaBimestre);
                var totalAlunosRI = ((alunosTurmaComNotaAbaixo.Count() / (double)alunosComNota.Count()) * 100);
                if (totalAlunosRI > percentualReprovacao)
                {
                    alunos = alunosTurmaComNotaAbaixo.ToList();
                }
            }

            return alunos;
        }

        private async Task<IEnumerable<long>> ObterCargosGestaoEscola(Ue ue)
        {
            var usuarios = await mediator.Send(new ObterFuncionariosDreOuUePorPerfisQuery(ue.CodigoUe, new List<Guid> { Perfis.PERFIL_AD, Perfis.PERFIL_CP, Perfis.PERFIL_DIRETOR }));

            var listaUsuarios = new List<long>();
            foreach (var usuario in usuarios.Distinct())
            {
                if (usuario != "")
                    listaUsuarios.Add(await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(usuario)));
            }

            return listaUsuarios.Distinct();
        }

        private string MontarLinhaDaTurma(NotificarResultadoInsatisfatorioDto dto)
        {
            var mensagem = new StringBuilder();

            var primeiroComponente = true;
            foreach (var componenteCurricular in dto.ComponentesCurriculares)
            {
                mensagem.Append("<tr style='padding:4px;'>");
                if (primeiroComponente)
                {
                    mensagem.Append($"<td style='padding:4px;' rowspan='{dto.ComponentesCurriculares.Count()}'>{dto.TurmaNome}</td>");
                    primeiroComponente = false;
                }
                mensagem.Append($"<td style='padding:4px;'>{componenteCurricular.ComponenteCurricularNome}</td>");
                mensagem.Append($"<td style='padding:4px;'>{componenteCurricular.Justificativa}</td>");
                mensagem.Append($"<td style='padding:4px;'>{componenteCurricular.Professor}</td>");
                mensagem.Append("</tr>");
            }
            return mensagem.ToString();
        }

        private string ObterHeaderModalidade(string modalidade)
        {
            return $@"<tr style='padding:4px;'>
	                    <td style='padding:4px;text-align:center;' colspan='4'>{modalidade}</td>
                       </tr>
                    <tr style='padding:4px;'>
                      <td style='padding:4px;'>Turma</td>
                      <td style='padding:4px;'>Componente curricular</td>
                      <td style='padding:4px;'>Justificativa</td>
                      <td style='padding:4px;'>Professor</td>
                    </tr>";
        }

        private Cargo[] ObterCargosGestaoEscola()
         => new[] { Cargo.CP, Cargo.AD, Cargo.Diretor };
    }
}
