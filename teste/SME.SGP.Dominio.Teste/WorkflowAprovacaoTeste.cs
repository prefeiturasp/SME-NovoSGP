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

            Assert.True(nivel != null && nivel.FirstOrDefault().Nivel == 5);
            nivel.FirstOrDefault().Adicionar(new Notificacao() { Id = 10, Mensagem = "Mensagem de teste" });

            var nivelDaNotificacao = workflowAprovacao.ObterNivelPorNotificacaoId(10);

            Assert.True(nivelDaNotificacao != null && nivelDaNotificacao.Nivel == 5);
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