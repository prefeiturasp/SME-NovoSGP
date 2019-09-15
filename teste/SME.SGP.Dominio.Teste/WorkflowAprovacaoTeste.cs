using System.Linq;
using Xunit;

namespace SME.SGP.Dominio.Teste
{
    public class WorkflowAprovacaoTeste
    {
        [Fact]
        public void Deve_Retornar_Niveis_Status()
        {
            var workflowAprovacao = new WorkflowAprovacao();
            workflowAprovacao.Adicionar(new WorkflowAprovacaoNivel() { Nivel = 1, Cargo = Cargo.AD });
            workflowAprovacao.Adicionar(new WorkflowAprovacaoNivel() { Nivel = 1, Cargo = Cargo.Diretor });
            workflowAprovacao.Adicionar(new WorkflowAprovacaoNivel() { Nivel = 2, Cargo = Cargo.Diretor });
            workflowAprovacao.Adicionar(new WorkflowAprovacaoNivel() { Nivel = 3, Cargo = Cargo.Diretor });
            workflowAprovacao.Adicionar(new WorkflowAprovacaoNivel() { Nivel = 5, Cargo = Cargo.Diretor });
            workflowAprovacao.Adicionar(new WorkflowAprovacaoNivel() { Nivel = 10, Cargo = Cargo.Diretor });
            workflowAprovacao.Adicionar(new WorkflowAprovacaoNivel() { Nivel = 13, Cargo = Cargo.Diretor });

            Assert.Equal(6, workflowAprovacao.ObtemNiveisUnicosEStatus().Count());
        }
    }
}