using Bogus;
using MediatR;
using Moq;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Responsavel
{
    public class RemoverAtribuicaoResponsaveisPAAIPorDreUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly RemoverAtribuicaoResponsaveisPAAIPorDreUseCase useCase;
        private readonly Faker faker;

        public RemoverAtribuicaoResponsaveisPAAIPorDreUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new RemoverAtribuicaoResponsaveisPAAIPorDreUseCase(mediatorMock.Object);
            faker = new Faker("pt_BR");
        }

        [Fact]
        public async Task Deve_Remover_Atribuicoes_Orfas_Quando_Existem_Divergencias()
        {
            // Arrange
            var codigoDre = faker.Random.AlphaNumeric(6);

            var supervisoresOrfaos = new Faker<SupervisorEscolasDreDto>()
                .RuleFor(s => s.AtribuicaoSupervisorId, f => f.Random.Long(1, 1000))
                .RuleFor(s => s.SupervisorId, f => f.Random.Replace("#######"))
                .Generate(3);

            var supervisoresValidos = new Faker<SupervisorEscolasDreDto>()
                .RuleFor(s => s.AtribuicaoSupervisorId, f => f.Random.Long(1001, 2000))
                .RuleFor(s => s.SupervisorId, f => f.Random.Replace("#######"))
                .Generate(5);

            var atribuicoesNoSgp = supervisoresValidos.Concat(supervisoresOrfaos).ToList();
            var responsaveisNoEol = supervisoresValidos.Select(s => new UsuarioEolRetornoDto { CodigoRf = s.SupervisorId }).ToList();
            var idsQueDevemSerRemovidos = supervisoresOrfaos.Select(s => s.AtribuicaoSupervisorId).ToList();
            IEnumerable<long> idsRemovidosCapturados = null;

            var mensagem = new MensagemRabbit(JsonSerializer.Serialize(codigoDre));

            mediatorMock.Setup(m => m.Send(It.Is<ObterSupervisoresPorDreAsyncQuery>(q => q.CodigoDre == codigoDre), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(atribuicoesNoSgp);

            mediatorMock.Setup(m => m.Send(It.Is<ObterFuncionariosPorPerfilDreQuery>(q => q.CodigoDre == codigoDre), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(responsaveisNoEol);

            // Corrigido: use os tipos específicos no Callback
            mediatorMock.Setup(m => m.Send(It.IsAny<RemoverAtribuicoesResponsaveisCommand>(), It.IsAny<CancellationToken>()))
                        .Callback<RemoverAtribuicoesResponsaveisCommand, CancellationToken>((cmd, token) =>
                        {
                            idsRemovidosCapturados = cmd.AtribuicoesIds;
                        })
                        .Returns(Task.CompletedTask);

            // Act
            var resultado = await useCase.Executar(mensagem);

            // Assert
            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<RemoverAtribuicoesResponsaveisCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.NotNull(idsRemovidosCapturados);
            Assert.Equal(idsQueDevemSerRemovidos.Count, idsRemovidosCapturados.Count());
            Assert.Empty(idsQueDevemSerRemovidos.Except(idsRemovidosCapturados));
        }

        [Fact]
        public async Task Nao_Deve_Remover_Nada_Quando_Nao_Existem_Divergencias()
        {
            // Arrange
            var codigoDre = faker.Random.AlphaNumeric(6);
            var mensagem = new MensagemRabbit(JsonSerializer.Serialize(codigoDre));

            var supervisoresValidos = new Faker<SupervisorEscolasDreDto>()
                .RuleFor(s => s.SupervisorId, f => f.Random.Replace("#######"))
                .Generate(5);

            var responsaveisNoEol = supervisoresValidos.Select(s => new UsuarioEolRetornoDto { CodigoRf = s.SupervisorId }).ToList();

            mediatorMock.Setup(m => m.Send(It.Is<ObterSupervisoresPorDreAsyncQuery>(q => q.CodigoDre == codigoDre), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(supervisoresValidos);

            mediatorMock.Setup(m => m.Send(It.Is<ObterFuncionariosPorPerfilDreQuery>(q => q.CodigoDre == codigoDre), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(responsaveisNoEol);

            // Act
            var resultado = await useCase.Executar(mensagem);

            // Assert
            Assert.True(resultado);
            // Garante que o comando de remoção nunca seja chamado
            mediatorMock.Verify(m => m.Send(It.IsAny<RemoverAtribuicoesResponsaveisCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Deve_Retornar_Sucesso_Sem_Processar_Quando_Nao_Existem_Atribuicoes_Iniciais()
        {
            // Arrange
            var codigoDre = faker.Random.AlphaNumeric(6);
            var mensagem = new MensagemRabbit(JsonSerializer.Serialize(codigoDre));

            // Retorna lista vazia na primeira consulta
            mediatorMock.Setup(m => m.Send(It.Is<ObterSupervisoresPorDreAsyncQuery>(q => q.CodigoDre == codigoDre), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<SupervisorEscolasDreDto>());

            // Act
            var resultado = await useCase.Executar(mensagem);

            // Assert
            Assert.True(resultado);
            // Garante que a segunda consulta (EOL) e o comando de remoção nunca sejam chamados
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterFuncionariosPorPerfilDreQuery>(), It.IsAny<CancellationToken>()), Times.Never);
            mediatorMock.Verify(m => m.Send(It.IsAny<RemoverAtribuicoesResponsaveisCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Deve_Retornar_Falha_Quando_CodigoDre_For_Invalido()
        {
            // Arrange
            // Mensagem com corpo nulo/inválido
            var mensagem = new MensagemRabbit(JsonSerializer.Serialize(string.Empty));

            // Act
            var resultado = await useCase.Executar(mensagem);

            // Assert
            Assert.False(resultado);
            mediatorMock.Verify(m => m.Send(It.Is<SalvarLogViaRabbitCommand>(c => c.Mensagem.Contains("Não foi possível obter o código da DRE")), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterSupervisoresPorDreAsyncQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
