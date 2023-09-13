using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarAlteracaoNotaFechamentoAgrupadaTurmaUseCase : AbstractUseCase, INotificarAlteracaoNotaFechamentoAgrupadaTurmaUseCase
    {
        protected const string MENSAGEM_DINAMICA_TABELA_POR_ALUNO = "<mensagemDinamicaTabelaPorAluno>";

        public NotificarAlteracaoNotaFechamentoAgrupadaTurmaUseCase(IMediator mediator) : base(mediator) {}

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var dados = param.ObterObjetoMensagem<IEnumerable<WfAprovacaoNotaFechamentoTurmaDto>>();

            var fechamentoTurmaDisciplinaId = dados.FirstOrDefault().FechamentoTurmaDisciplinaId;
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(dados.FirstOrDefault().TurmaId));
            var lancaNota = !dados.FirstOrDefault().WfAprovacao.ConceitoId.HasValue;
            var notaConceitoMensagem = lancaNota ? "nota(s)" : "conceito(s)";
            var mensagem = await MontaMensagemWfAprovacao(dados, lancaNota, turma);

            var wfAprovacao = new WorkflowAprovacaoDto
            {
                Ano = DateTime.Today.Year,
                NotificacaoCategoria = NotificacaoCategoria.Workflow_Aprovacao,
                EntidadeParaAprovarId = fechamentoTurmaDisciplinaId,
                Tipo = WorkflowAprovacaoTipo.AlteracaoNotaFechamento,
                TurmaId = turma.CodigoTurma,
                UeId = turma.Ue.CodigoUe,
                DreId = turma.Ue.Dre.CodigoDre,
                NotificacaoTitulo = $"Alteração em {notaConceitoMensagem} final - {turma.Ue.TipoEscola.ObterNomeCurto()} {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao}) - {turma.NomeComModalidade()} (ano anterior)",
                NotificacaoTipo = NotificacaoTipo.Notas,
                NotificacaoMensagem = mensagem
            };

            int? bimestre = dados.FirstOrDefault().Bimestre;

            if (bimestre.NaoEhNulo())
                wfAprovacao.AdicionarNivel(Cargo.CP);
            else
            {
                wfAprovacao.AdicionarNivel(Cargo.CP);
                wfAprovacao.AdicionarNivel(Cargo.Supervisor);
            }

            var idWorkflow = await mediator.Send(new SalvarWorkflowAprovacaoNivelUsuarioCommand(wfAprovacao));

            var workflowAprovacaoNotaFechamentoIds = dados.Select(d => d.WfAprovacao.Id);

            await mediator.Send(new AlterarWFAprovacaoNotaFechamentoPorWfAprovacaoIdCommand() { WorkflowAprovacaoId = idWorkflow, WorkflowAprovacaoFechamentoNotaIds = workflowAprovacaoNotaFechamentoIds.ToArray() });

            return true;
        }

        private async Task<string> MontaMensagemWfAprovacao(IEnumerable<WfAprovacaoNotaFechamentoTurmaDto> notasAprovacao, bool lancaNota, Turma turma)
        {
            int? bimestreNota = notasAprovacao.FirstOrDefault().Bimestre;
            var bimestre = (bimestreNota ?? 0) == 0 ? "bimestre final" : $"{bimestreNota}º bimestre";
            
            var notaConceitoMensagem = lancaNota ? "A(s) nota(s)" : "O(s) conceito(s)";

            var mensagem = new StringBuilder();
            mensagem.Append($"<p>{notaConceitoMensagem} do {bimestre} de {turma.AnoLetivo} da turma {turma.ModalidadeCodigo.ObterNomeCurto()}-{turma.Nome} da ");
            mensagem.Append($"{turma.Ue.TipoEscola.ObterNomeCurto()} {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao}) foram alteradas ");
            mensagem.AppendLine(MENSAGEM_DINAMICA_TABELA_POR_ALUNO);

            return mensagem.ToString();
        }
    }
}
