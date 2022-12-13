using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public abstract class AprovacaoNotaConselhoCommandBase<T> : AsyncRequestHandler<T> where T : IRequest
    {
        protected readonly IMediator mediator;
        protected List<WFAprovacaoNotaConselho> WFAprovacoes;
        protected List<Ue> Ues;
        protected List<TurmasDoAlunoDto> Alunos;
        protected List<ComponenteCurricularDescricaoDto> ComponentesCurriculares;
        protected List<Usuario> Usuarios;
        protected List<Conceito> Conceitos;

        public AprovacaoNotaConselhoCommandBase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected abstract string ObterTexto(Ue ue, Turma turma, PeriodoEscolar periodoEscolar);

        protected abstract string ObterTitulo(Ue ue, Turma turma);

        protected async Task IniciarAprovacao(IEnumerable<WFAprovacaoNotaConselho> wfAprovacoes)
        {
            WFAprovacoes = wfAprovacoes.ToList();
            await CarregarTodasUes();
            await CarregarTodosAlunos();
            await CarregarTodosComponentes();
            await CarregarTodosUsuarios();
            await CarregarConceitos();
        }

        protected async Task<string> ObterMensagem(Ue ue, Turma turma, List<WFAprovacaoNotaConselho> aprovacoesPorTurma)
        {
            var periodoEscolar = aprovacoesPorTurma.FirstOrDefault().ConselhoClasseNota.ConselhoClasseAluno.ConselhoClasse.FechamentoTurma.PeriodoEscolar;
            var descricao = new StringBuilder(ObterTexto(ue, turma, periodoEscolar));

            descricao.Append(await ObterTabelaDosAlunos(aprovacoesPorTurma, turma));

            return descricao.ToString();
        }

        private async Task<string> ObterTabelaDosAlunos(List<WFAprovacaoNotaConselho> aprovacoesPorTurma, Turma turma)
        {
            var descricao = new StringBuilder();
            descricao.AppendLine("<table style='margin-left: auto; margin-right: auto; margin-top: 10px' border='2' cellpadding='5'>");
            descricao.AppendLine("<tbody>");
            descricao.AppendLine("<tr>");
            descricao.AppendLine("<td style='padding: 3px;'><strong>Componente curricular</strong></td>");
            descricao.AppendLine("<td style='padding: 3px;'><strong>Estudante</strong></td>");
            descricao.AppendLine("<td style='padding: 3px;'><strong>Valor anterior</strong></td>");
            descricao.AppendLine("<td style='padding: 3px;'><strong>Novo valor</strong></td>");
            descricao.AppendLine("<td style='padding: 3px;'><strong>Usuário que alterou</strong></td>");
            descricao.AppendLine("<td style='padding: 3px;'><strong>Data da alteração</strong></td>");
            descricao.AppendLine("</tr>");

            foreach (var aprovaco in aprovacoesPorTurma)
            {
                descricao.AppendLine(await ObterLinhaDoAluno(aprovaco, turma));
            }

            descricao.AppendLine("<tbody>");
            descricao.AppendLine("</table>");

            return descricao.ToString();
        }

        private async Task<string> ObterLinhaDoAluno(WFAprovacaoNotaConselho aprovacao, Turma turma)
        {
            var aluno = Alunos.Find(aluno => aluno.CodigoAluno.ToString() == aprovacao.ConselhoClasseNota.ConselhoClasseAluno.AlunoCodigo && aluno.CodigoTurma.ToString() == turma.CodigoTurma);
            var componenteCurricular = ComponentesCurriculares.Find(componente => componente.Id == aprovacao.ConselhoClasseNota.ComponenteCurricularCodigo);
            var usuario = Usuarios.Find(usuario => usuario.Id == aprovacao.UsuarioSolicitanteId);
            var notas = await ObterValoresNotasNovoAnterior(aprovacao);

            return $@"<tr>
                           <td style='padding: 3px;'>{componenteCurricular.Descricao}</td>
                           <td style='padding: 3px;'>{aluno.NumeroAlunoChamada} - {aluno.NomeAluno} ({aluno.CodigoAluno})</td>
                           <td style='padding: 3px;'>{notas.Item1}</td>
                           <td style='padding: 3px;'>{notas.Item2}</td>
                           <td style='padding: 3px;'>{usuario.Nome} ({usuario.CodigoRf})</td>
                           <td style='padding: 3px;'>{aprovacao.CriadoEm.ToString("dd/MM/yyy HH:mm")}</td>
                      </tr>";
        }

        private async Task<(string, string)> ObterValoresNotasNovoAnterior(WFAprovacaoNotaConselho aprovacao)
        {
            var valorAnterior = string.Empty;
            var valorNovo = string.Empty;

            if (aprovacao.ConceitoId.HasValue || aprovacao.ConselhoClasseNota.ConceitoId.HasValue)
            {
                if (aprovacao.ConselhoClasseNota.ConceitoId.HasValue)
                    valorAnterior = Conceitos.FirstOrDefault(a => a.Id == aprovacao.ConselhoClasseNota.ConceitoId)?.Valor;

                if (aprovacao.ConceitoId.HasValue)
                    valorNovo = Conceitos.FirstOrDefault(a => a.Id == aprovacao.ConceitoId)?.Valor;
            }
            else
            {
                valorAnterior = aprovacao.ConselhoClasseNota.Nota?.ToString() ?? string.Empty;
                valorNovo = aprovacao.Nota?.ToString() ?? string.Empty;
            }

            if (string.IsNullOrEmpty(valorAnterior))
            {
                valorAnterior = await ObterNotaConceitoFechamentoAluno(aprovacao.ConselhoClasseNota.ConselhoClasseAluno.ConselhoClasse.FechamentoTurmaId,
                                                                                                          aprovacao.ConselhoClasseNota.ConselhoClasseAluno.AlunoCodigo,
                                                                                                          aprovacao.ConselhoClasseNota.ComponenteCurricularCodigo);
            }

            return (valorAnterior, valorNovo);
        }

        private async Task<string> ObterNotaConceitoFechamentoAluno(long fechamentoTurmaId, string codigoAluno, long componenteCurricularId)
        {
            var fechamentoNotas = await mediator.Send(new ObterPorFechamentoTurmaAlunoDisciplinaQuery(fechamentoTurmaId,
                                                                                                      codigoAluno,
                                                                                                      componenteCurricularId));
            if (fechamentoNotas != null && fechamentoNotas.Any())
            {
                var fechamentoNota = fechamentoNotas.FirstOrDefault();
                if (fechamentoNota.Nota.HasValue)
                    return fechamentoNota.Nota.ToString();
                else
                if (fechamentoNota.ConceitoId.HasValue)
                    return Conceitos.FirstOrDefault(conceito => conceito.Id == fechamentoNota.ConceitoId)?.Valor;
            }

            return string.Empty;
        }

        private async Task CarregarTodasUes()
        {
            var ueIds = WFAprovacoes.Select(wf => wf.ConselhoClasseNota.ConselhoClasseAluno.ConselhoClasse.FechamentoTurma.Turma.UeId).Distinct().ToArray();

            Ues = (await ObterUes(ueIds)).ToList();
        }

        private async Task CarregarTodosAlunos()
        {
            var codigos = WFAprovacoes.Select(wf => long.Parse(wf.ConselhoClasseNota.ConselhoClasseAluno.AlunoCodigo)).ToArray();
            var anoLetivo = WFAprovacoes.FirstOrDefault().ConselhoClasseNota.ConselhoClasseAluno.ConselhoClasse.FechamentoTurma.Turma.AnoLetivo;

            Alunos = (await ObterAlunos(codigos, anoLetivo)).ToList();
        }

        private async Task CarregarTodosComponentes()
        {
            var codigos = WFAprovacoes.Select(wf => wf.ConselhoClasseNota.ComponenteCurricularCodigo).Distinct().ToArray();

            ComponentesCurriculares = (await ObterComponentes(codigos)).ToList();
        }

        private async Task CarregarTodosUsuarios()
        {
            var ids = WFAprovacoes.Select(wf => wf.UsuarioSolicitanteId).Distinct().ToArray();

            Usuarios = (await ObterUsuarios(ids)).ToList();
        }

        private async Task CarregarConceitos()
        {
            Conceitos = (await mediator.Send(new ObterConceitosValoresQuery())).ToList();
        }

        private async Task<IEnumerable<Ue>> ObterUes(long[] ueIds)
            => await mediator.Send(new ObterUesPorIdsQuery(ueIds));

        private async Task<IEnumerable<ComponenteCurricularDescricaoDto>> ObterComponentes(long[] codigos)
            => await mediator.Send(new ObterDescricaoComponentesCurricularesPorIdsQuery(codigos));

        private async Task<IEnumerable<TurmasDoAlunoDto>> ObterAlunos(long[] codigos, int anoLetivo)
            => await mediator.Send(new ObterAlunosEolPorCodigosEAnoQuery(codigos, anoLetivo));

        private async Task<IEnumerable<Usuario>> ObterUsuarios(long[] ids)
            => await mediator.Send(new ObterUsuarioPorIdsSemPerfilQuery(ids));
    }
}
