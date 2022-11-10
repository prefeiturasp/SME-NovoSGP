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
        private List<TurmasDoAlunoDto> Alunos;
        private List<ComponenteCurricularDescricaoDto> ComponentesCurriculares;
        private List<Usuario> Usuarios;
        private List<Conceito> Conceitos;

        public AprovacaoNotaConselhoCommandBase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected abstract Task CarregueWFAprovacoes();

        protected abstract string ObterTexto(Ue ue, Turma turma, PeriodoEscolar periodoEscolar);

        protected abstract string ObterTitulo(Ue ue, Turma turma);

        protected async Task IniciarAprovacao()
        {
            await CarregueWFAprovacoes();
            await CarregarTodasUes();
            await CarregarTodosAlunos();
            await CarregarTodosComponentes();
            await CarregarTodosUsuarios();
            await CarregarConceitos();
        }

        protected string ObterDescricao(Ue ue, Turma turma, List<WFAprovacaoNotaConselho> aprovacoesPorTurma)
        {
            var periodoEscolar = aprovacoesPorTurma.FirstOrDefault().ConselhoClasseNota.ConselhoClasseAluno.ConselhoClasse.FechamentoTurma.PeriodoEscolar;
            var descricao = new StringBuilder(ObterTexto(ue, turma, periodoEscolar));

            descricao.Append(ObterTabelaDosAlunos(aprovacoesPorTurma));

            return descricao.ToString();
        }

        private string ObterTabelaDosAlunos(List<WFAprovacaoNotaConselho> aprovacoesPorTurma)
        {
            var descricao = new StringBuilder();
            descricao.AppendLine("<table style='margin-left: auto; margin-right: auto; margin-top: 10px' border='2' cellpadding='5'>");
            descricao.AppendLine("<tbody>");
            descricao.AppendLine("<tr>");
            descricao.AppendLine("<td><strong>Componente curricular</strong></td>");
            descricao.AppendLine("<td><strong>Estudante</strong></td>");
            descricao.AppendLine("<td style='text-align: center;'><strong>Valor anterior</strong></td>");
            descricao.AppendLine("<td style='text-align: center;'><strong>Novo valor</strong></td>");
            descricao.AppendLine("<td><strong>Usuário que alterou</strong></td>");
            descricao.AppendLine("<td><strong>Data da alteração</strong></td>");
            descricao.AppendLine("</tr>");

            foreach (var aprovaco in aprovacoesPorTurma)
            {
                descricao.AppendLine(ObterLinhaDoAluno(aprovaco));
            }

            descricao.AppendLine("<tbody>");
            descricao.AppendLine("</table>");

            return descricao.ToString();
        }

        private string ObterLinhaDoAluno(WFAprovacaoNotaConselho aprovacao)
        {
            var aluno = Alunos.Find(aluno => aluno.CodigoAluno.ToString() == aprovacao.ConselhoClasseNota.ConselhoClasseAluno.AlunoCodigo);
            var componenteCurricular = ComponentesCurriculares.Find(componente => componente.Id == aprovacao.ConselhoClasseNota.ComponenteCurricularCodigo);
            var usuario = Usuarios.Find(usuario => usuario.Id == aprovacao.UsuarioSolicitanteId);
            var notas = ObtenhaValoresNotasNovoAnterior(aprovacao);

            return $@"<tr>
                           <td>{componenteCurricular.Descricao}</td>
                           <td>{aluno.NumeroAlunoChamada} - {aluno.NomeAluno} ({aluno.CodigoAluno})</td>
                           <td>{notas.Item1}</td>
                           <td>{notas.Item2}</td>
                           <td>{usuario.Nome} ({usuario.CodigoRf})</td>
                           <td>{aprovacao.CriadoEm.ToString("dd/MM/yyy HH:mm")}</td>
                      </tr>";
        }

        private (string, string) ObtenhaValoresNotasNovoAnterior(WFAprovacaoNotaConselho aprovacao)
        {
            var valorAnterior = string.Empty;
            var valorNovo = string.Empty;

            if (aprovacao.ConceitoId.HasValue || aprovacao.ConselhoClasseNota.ConceitoId.HasValue)
            {
                if (aprovacao.ConselhoClasseNota.ConceitoId.HasValue)
                    valorAnterior = Conceitos.FirstOrDefault(a => a.Id == aprovacao.ConselhoClasseNota.ConceitoId)?.Descricao;

                if (aprovacao.ConceitoId.HasValue)
                    valorNovo = Conceitos.FirstOrDefault(a => a.Id == aprovacao.ConceitoId)?.Descricao;
            }
            else
            {
                valorAnterior = aprovacao.ConselhoClasseNota.Nota?.ToString() ?? string.Empty;
                valorNovo = aprovacao.Nota?.ToString() ?? string.Empty;
            }

            return (valorAnterior, valorNovo);
        }

        private async Task CarregarTodasUes()
        {
            var ueIds = WFAprovacoes.Select(wf => wf.ConselhoClasseNota.ConselhoClasseAluno.ConselhoClasse.FechamentoTurma.Turma.UeId).Distinct().ToArray();

            Ues = (await ObterUes(ueIds)).ToList();
        }

        private async Task CarregarTodosAlunos()
        {
            var codigos = WFAprovacoes.Select(wf => long.Parse(wf.ConselhoClasseNota.ConselhoClasseAluno.AlunoCodigo)).ToArray();

            Alunos = (await ObterAlunos(codigos)).ToList();
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

        private async Task<IEnumerable<TurmasDoAlunoDto>> ObterAlunos(long[] codigos)
            => await mediator.Send(new ObterAlunosEolPorCodigosQuery(codigos));

        private async Task<IEnumerable<Usuario>> ObterUsuarios(long[] ids)
            => await mediator.Send(new ObterUsuarioPorIdsSemPerfilQuery(ids));
    }
}
