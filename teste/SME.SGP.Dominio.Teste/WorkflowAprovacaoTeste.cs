using System.Linq;
using Xunit;

namespace SME.SGP.Dominio.Teste
{
    public class WorkflowAprovacaoTeste
    {
        [Fact]
        public void Deve_Retornar_Niveis_Status()
        {
            WorkflowAprovacao workflowAprovacao = GeraWorkflow();

            Assert.Equal(6, workflowAprovacao.ObtemNiveisUnicosEStatus().Count());
        }

        [Fact]
        public void Deve_Retornar_Nivel_Por_Notificacao()
        {
            WorkflowAprovacao workflowAprovacao = GeraWorkflow();
            var nivel = workflowAprovacao.ObtemNiveis(5);

            Assert.True(nivel.NaoEhNulo() && nivel.FirstOrDefault().Nivel == 5);
            nivel.FirstOrDefault().Adicionar(new Notificacao() { Id = 10, Mensagem = "Mensagem de teste" });

            var nivelDaNotificacao = workflowAprovacao.ObterNivelPorNotificacaoId(10);

            Assert.True(nivelDaNotificacao.NaoEhNulo() && nivelDaNotificacao.Nivel == 5);
        }

        [Fact]
        public void Deve_Retornar_Notificacao_Pendente_Do_Usuario_No_Nivel_Atual()
        {
            const string rfUsuario = "7813961";
            var workflow = new WorkflowAprovacao();
            var nivelAtual = new WorkflowAprovacaoNivel
            {
                Nivel = 1,
                Status = WorkflowAprovacaoNivelStatus.AguardandoAprovacao
            };
            var notificacao = new Notificacao
            {
                Id = 10,
                Categoria = NotificacaoCategoria.Workflow_Aprovacao,
                Status = NotificacaoStatus.Pendente,
                Usuario = new Usuario { CodigoRf = rfUsuario }
            };
            nivelAtual.Adicionar(notificacao);
            workflow.Adicionar(nivelAtual);

            var notificacaoPendente = workflow.ObterNotificacaoPendentePorUsuario(rfUsuario);

            Assert.Equal(notificacao.Id, notificacaoPendente.Id);
        }

        [Theory]
        [InlineData(WorkflowAprovacaoNivelStatus.SemStatus, NotificacaoStatus.Pendente, "7813961")]
        [InlineData(WorkflowAprovacaoNivelStatus.AguardandoAprovacao, NotificacaoStatus.Aceita, "7813961")]
        [InlineData(WorkflowAprovacaoNivelStatus.AguardandoAprovacao, NotificacaoStatus.Pendente, "6875289")]
        public void Nao_Deve_Retornar_Notificacao_Quando_Usuario_Ou_Fluxo_Nao_Corresponder(
            WorkflowAprovacaoNivelStatus statusNivel,
            NotificacaoStatus statusNotificacao,
            string rfConsultado)
        {
            var workflow = new WorkflowAprovacao();
            var nivel = new WorkflowAprovacaoNivel { Nivel = 1, Status = statusNivel };
            nivel.Adicionar(new Notificacao
            {
                Id = 10,
                Categoria = NotificacaoCategoria.Workflow_Aprovacao,
                Status = statusNotificacao,
                Usuario = new Usuario { CodigoRf = "7813961" }
            });
            workflow.Adicionar(nivel);

            var notificacaoPendente = workflow.ObterNotificacaoPendentePorUsuario(rfConsultado);

            Assert.Null(notificacaoPendente);
        }

        private static WorkflowAprovacao GeraWorkflow()
        {
            var workflowAprovacao = new WorkflowAprovacao();
            workflowAprovacao.Adicionar(new WorkflowAprovacaoNivel() { Nivel = 1, Cargo = Cargo.AD });
            workflowAprovacao.Adicionar(new WorkflowAprovacaoNivel() { Nivel = 1, Cargo = Cargo.Diretor });
            workflowAprovacao.Adicionar(new WorkflowAprovacaoNivel() { Nivel = 2, Cargo = Cargo.Diretor });
            workflowAprovacao.Adicionar(new WorkflowAprovacaoNivel() { Nivel = 3, Cargo = Cargo.Diretor });
            workflowAprovacao.Adicionar(new WorkflowAprovacaoNivel() { Nivel = 5, Cargo = Cargo.Diretor });
            workflowAprovacao.Adicionar(new WorkflowAprovacaoNivel() { Nivel = 10, Cargo = Cargo.Diretor });
            workflowAprovacao.Adicionar(new WorkflowAprovacaoNivel() { Nivel = 13, Cargo = Cargo.Diretor });
            return workflowAprovacao;
        }
    }
}