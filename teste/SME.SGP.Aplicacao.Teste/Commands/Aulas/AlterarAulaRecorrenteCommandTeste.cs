using Xunit;

namespace SME.SGP.Aplicacao.Teste.Commands.Aulas
{
    public class AlterarAulaRecorrenteCommandTeste
    {
        [Fact(DisplayName = "AlterarAulaRecorrenteCommand - Deve verificar se a classe possui construtor sem parâmetros")]
        public void DeveVerificarSeAlterarAulaRecorrenteCommandPossuiContrutorSemParametros()
        {
            var classeCommand = typeof(AlterarAulaRecorrenteCommand);

            Assert.Contains(classeCommand.GetConstructors(), c => c.GetParameters().Length == 0);
        }
    }
}
