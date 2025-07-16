using Bogus;
using MediatR;
using Moq;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Xunit;
using System.Linq;
using System.Text.Json;

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

            // 1. Gera dados de PAAIs que estão no SGP, mas não mais no EOL (órfãos)
            var supervisoresOrfaos = new Faker<SupervisorEscolasDreDto>()
                .RuleFor(s => s.AtribuicaoSupervisorId, f => f.Random.Long(1, 1000))
                .RuleFor(s => s.SupervisorId, f => f.Random.Replace("#######"))
                .Generate(3);

            // 2. Gera dados de PAAIs que estão consistentes em ambas as bases
            var supervisoresValidos = new Faker<SupervisorEscolasDreDto>()
                .RuleFor(s => s.AtribuicaoSupervisorId, f => f.Random.Long(1001, 2000))
                .RuleFor(s => s.SupervisorId, f => f.Random.Replace("#######"))
                .Generate(5);

            // 3. Monta as listas para os mocks
            var atribuicoesNoSgp = supervisoresValidos.Concat(supervisoresOrfaos).ToList();
            var responsaveisNoEol = supervisoresValidos.Select(s => new UsuarioEolRetornoDto { CodigoRf = s.SupervisorId }).ToList();
            var idsQueDevemSerRemovidos = supervisoresOrfaos.Select(s => s.AtribuicaoSupervisorId).ToList();
            IEnumerable<long> idsRemovidosCapturados = null;

            var mensagem = new MensagemRabbit(JsonSerializer.Serialize(codigoDre));

            // Configura os mocks do MediatR
            mediatorMock.Setup(m => m.Send(It.Is<ObterSupervisoresPorDreAsyncQuery>(q => q.CodigoDre == codigoDre), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(atribuicoesNoSgp);

            mediatorMock.Setup(m => m.Send(It.Is<ObterFuncionariosPorPerfilDreQuery>(q => q.CodigoDre == codigoDre), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(responsaveisNoEol);

            // Captura os IDs enviados para o comando de remoção para verificação posterior
            mediatorMock.Setup(m => m.Send(It.IsAny<RemoverAtribuicoesResponsaveisCommand>(), It.IsAny<CancellationToken>()))
                        .Callback<IRequest<Unit>, CancellationToken>((cmd, token) => {
                            var command = cmd as RemoverAtribuicoesResponsaveisCommand;
                            idsRemovidosCapturados = command.AtribuicoesIds;
                        })
                        .ReturnsAsync(Unit.Value);

            // Act
            var resultado = await useCase.Executar(mensagem);

            // Assert
            Assert.True(resultado);
            // Verifica se o comando de remoção foi chamado
            mediatorMock.Verify(m => m.Send(It.IsAny<RemoverAtribuicoesResponsaveisCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            // Verifica se a lista de IDs capturada é exatamente a lista de órfãos
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
