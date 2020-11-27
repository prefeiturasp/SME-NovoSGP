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

            var periodoFechamentoBimestres = await mediator.Send(new ObterPeriodosEscolaresPorModalidadeDataFechamentoQuery((int)request.ModalidadeTipoCalendario, DateTime.Now.AddDays(request.Dias).Date));

            var percentualReprovacao = double.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.PercentualAlunosInsuficientes, DateTime.Today.Year)));
            var mediaBimestre = double.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.MediaBimestre, DateTime.Today.Year)));
            var componentes = await mediator.Send(new ObterComponentesCurricularesQuery());

            List<NotificarResultadoInsatisfatorioDto> listaNotificacoes = new List<NotificarResultadoInsatisfatorioDto>();

            foreach(var periodoFechamentoBimestre in periodoFechamentoBimestres)
            {
                if(periodoFechamentoBimestre.PeriodoFechamento.UeId != null)
                {
                    var alunosComNotaLancada = await mediator.Send(new ObterAlunosComNotaLancadaPorPeriodoEscolarUEQuery(periodoFechamentoBimestre.PeriodoFechamento.UeId.GetValueOrDefault(), periodoFechamentoBimestre.PeriodoEscolarId));

                    if(alunosComNotaLancada != null)
                    {
                        foreach(var turmaId in alunosComNotaLancada.GroupBy(a => a.TurmaId))
                        {
                            
                            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(turmaId.Key));

                            var turmaNotificacao = new NotificarResultadoInsatisfatorioDto();
                            turmaNotificacao.TurmaNome = turma.Nome;
                            turmaNotificacao.TurmaModalidade = turma.ModalidadeCodigo.Name();

                            foreach (var componenteCurricularId in alunosComNotaLancada.Where(a => a.TurmaId == turmaId.Key).GroupBy(a => a.ComponenteCurricularId))
                            {
                                var alunosTurma = await mediator.Send(new ObterAlunosPorTurmaEAnoLetivoQuery(turma.CodigoTurma));
                                var alunosComNota = alunosComNotaLancada.Where(a => a.TurmaId == turmaId.Key && a.ComponenteCurricularId == componenteCurricularId.Key);

                                var alunosRetorno = VerificaAlunosResultadoInsatisfatorio(alunosComNota, percentualReprovacao, turma, componenteCurricularId.Key, mediaBimestre);

                                if(alunosRetorno.Any())
                                {
                                    var componenteNotificacao = new NotificarResultadoInsatisfatorioCCDto();
                                    var obterComponenteCurricular = componentes.FirstOrDefault(c => long.Parse(c.Codigo) == componenteCurricularId.Key);

                                    componenteNotificacao.ComponenteCurricularNome = obterComponenteCurricular.Descricao;
                                    componenteNotificacao.Professor = $"{alunosRetorno.FirstOrDefault().ProfessorNome} ({alunosRetorno.FirstOrDefault().ProfessorRf})";
                                    componenteNotificacao.Justificativa = alunosRetorno.FirstOrDefault().Justificativa;

                                    turmaNotificacao.ComponentesCurriculares.Add(componenteNotificacao);
                                }

                            }

                            if(turmaNotificacao.ComponentesCurriculares.Any())
                                listaNotificacoes.Add(turmaNotificacao);
                        }
                    }
                }

                if (listaNotificacoes.Any())
                    await EnviarNotificacoes(listaNotificacoes, periodoFechamentoBimestre);
            }
                    
           
            return true;
        }

        private async Task EnviarNotificacoes(List<NotificarResultadoInsatisfatorioDto> listaNotificacoes, PeriodoFechamentoBimestre periodoFechamentoBimestre)
        {
            var titulo = $"Turmas com resultados insatisfatórios no {periodoFechamentoBimestre.PeriodoEscolar.Bimestre}º bimestre";
            var mensagem = new StringBuilder($"As turmas e componentes curriculares abaixo da <b>{periodoFechamentoBimestre.PeriodoFechamento.Ue.TipoEscola.ShortName()} {periodoFechamentoBimestre.PeriodoFechamento.Ue.Nome} ({periodoFechamentoBimestre.PeriodoFechamento.Ue.Dre.Abreviacao}) tiveram mais de 50% dos estudantes com resultado insatisfatório no <b>{periodoFechamentoBimestre.PeriodoEscolar.Bimestre}º bimestre</b>:");

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

            await EnviarNotificacao(titulo, mensagem.ToString(), periodoFechamentoBimestre.PeriodoFechamento.Ue.Dre.CodigoDre, periodoFechamentoBimestre.PeriodoFechamento.Ue.CodigoUe);
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

        private async Task EnviarNotificacao(string titulo, string mensagem, string codigoDre, string codigoUe)
        {
            var cargos = new[] { Cargo.CP, Cargo.AD, Cargo.Diretor };
            await mediator.Send(new EnviarNotificacaoCommand(titulo, mensagem, NotificacaoCategoria.Aviso, NotificacaoTipo.Fechamento, cargos, codigoDre, codigoUe));
        }

        private string MontarLinhaDaTurma(NotificarResultadoInsatisfatorioDto dto)
        {
            var mensagem = new StringBuilder();

            mensagem.Append("<tr>");
            mensagem.Append($"<td rowspan='{dto.ComponentesCurriculares.Count()}'>{dto.TurmaNome}</td>");
            mensagem.Append("</tr>");
            foreach (var componenteCurricular in dto.ComponentesCurriculares)
            {
                mensagem.Append("<tr>");
                mensagem.Append($"<td>{componenteCurricular.ComponenteCurricularNome}</td>");
                mensagem.Append($"<td>{componenteCurricular.Justificativa}</td>");
                mensagem.Append($"<td>{componenteCurricular.Professor}</td>");
                mensagem.Append("</tr>");
            }
            return mensagem.ToString();
        }

        private string ObterHeaderModalidade(string modalidade)
        {
            return @$"<tr>
	                    <td colspan='4'>{modalidade}</td>
                    </tr>
                    <tr>
                      <td>Turma</td>
                      <td>Componente curricular</td>
                      <td>Justificativa</td>
                      <td>Professor</td>
                    </tr>";
        }
    }
}
