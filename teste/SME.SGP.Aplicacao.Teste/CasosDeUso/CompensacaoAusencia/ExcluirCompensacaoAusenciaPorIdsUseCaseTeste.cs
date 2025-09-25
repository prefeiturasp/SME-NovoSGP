using FluentValidation.TestHelper;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.CompensacaoAusencia
{
    public class ExcluirCompensacaoAusenciaPorIdsUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ExcluirCompensacaoAusenciaPorIdsUseCase useCase;
        private readonly ExcluirCompensacaoAusenciaPorIdsCommandValidator validator
       = new ExcluirCompensacaoAusenciaPorIdsCommandValidator();

        public ExcluirCompensacaoAusenciaPorIdsUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ExcluirCompensacaoAusenciaPorIdsUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Chamar_Mediator_Com_Command_Correto_E_Retornar_True()
        {
            var ids = new long[] { 10, 20, 30 };
            var filtro = new FiltroCompensacaoAusenciaDto(ids);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ExcluirCompensacaoAusenciaPorIdsCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.Is<ExcluirCompensacaoAusenciaPorIdsCommand>(
                c => c.CompensacaoAusenciaIds.SequenceEqual(ids)), It.IsAny<CancellationToken>()), Times.Once);
        }  

        [Fact]
        public void Validator_Deve_Ser_Invalido_Quando_Ids_For_Nulo()
        {
            var command = new ExcluirCompensacaoAusenciaPorIdsCommand(null);

            var resultado = validator.TestValidate(command);

            resultado.ShouldHaveValidationErrorFor(c => c.CompensacaoAusenciaIds)
                .WithErrorMessage("Os ids das compensações de ausências deevem ser informados para efetuar a exclusão de compensações que não tem compensações aluno e aula.");
        }

        [Fact]
        public void Validator_Deve_Ser_Valido_Quando_Ids_Preenchidos()
        {
            var command = new ExcluirCompensacaoAusenciaPorIdsCommand(new long[] { 1, 2, 3 });

            var resultado = validator.TestValidate(command);

            resultado.ShouldNotHaveAnyValidationErrors();
        }

    }
}
