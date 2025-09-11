using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PlanoAEE
{
    public class AtualizaPlanoAEETurmaAlunoTratarUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly AtualizaPlanoAEETurmaAlunoTratarUseCase useCase;

        public AtualizaPlanoAEETurmaAlunoTratarUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new AtualizaPlanoAEETurmaAlunoTratarUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Retornar_True_Quando_Command_For_Executado_Com_Sucesso()
        {
            var command = new SalvarPlanoAEETurmaAlunoCommand(1, "123456");
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(command) };

            mediator.Setup(m => m.Send(It.IsAny<SalvarPlanoAEETurmaAlunoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            mediator.Verify(m => m.Send(It.Is<SalvarPlanoAEETurmaAlunoCommand>(
                c => c.PlanoAEEId == 1 && c.AlunoCodigo == "123456"),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Mensagem_For_Invalida()
        {
            var mensagem = new MensagemRabbit { Mensagem = "json inválido" };

            await Assert.ThrowsAsync<Newtonsoft.Json.JsonReaderException>(() => useCase.Executar(mensagem));
        }

        [Fact]
        public async Task Deve_Retornar_False_Quando_Command_Falhar()
        {
            var command = new SalvarPlanoAEETurmaAlunoCommand(1, "123456");
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(command) };

            mediator.Setup(m => m.Send(It.IsAny<SalvarPlanoAEETurmaAlunoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var resultado = await useCase.Executar(mensagem);

            Assert.False(resultado);
        }

        [Fact]
        public async Task Deve_Tratar_Excecao_Do_Command()
        {
            var command = new SalvarPlanoAEETurmaAlunoCommand(1, "123456");
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(command) };

            mediator.Setup(m => m.Send(It.IsAny<SalvarPlanoAEETurmaAlunoCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro no command"));

            await Assert.ThrowsAsync<Exception>(() => useCase.Executar(mensagem));
        }

        [Fact]
        public async Task Nao_Deve_Aceitar_Command_Com_ID_Zero()
        {
            var command = new SalvarPlanoAEETurmaAlunoCommand(0, "");
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(command) };

            var resultado = await useCase.Executar(mensagem);

            Assert.False(resultado);
        }
    }
}

