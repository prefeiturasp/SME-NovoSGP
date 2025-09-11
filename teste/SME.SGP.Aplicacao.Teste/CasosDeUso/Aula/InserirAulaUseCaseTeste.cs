using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Aplicacao.Commands.Aulas.InserirAula;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Aula
{
    public class InserirAulaUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IPodeCadastrarAulaUseCase> podeCadastrarAulaUseCaseMock;
        private readonly InserirAulaUseCase inserirAulaUseCase;

        public InserirAulaUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            podeCadastrarAulaUseCaseMock = new Mock<IPodeCadastrarAulaUseCase>();
            inserirAulaUseCase = new InserirAulaUseCase(mediatorMock.Object, podeCadastrarAulaUseCaseMock.Object);
        }

        [Fact]
        public async Task Executar_DeveLancarExcecao_SeAulaReposicaoComRecorrencia()
        {
            // Arrange
            var dto = CriarDto(RecorrenciaAula.RepetirBimestreAtual);
            dto.TipoAula = TipoAula.Reposicao; // Aula de reposição com recorrência não única

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new Usuario());

            // Act & Assert
            var ex = await Assert.ThrowsAsync<NegocioException>(() => inserirAulaUseCase.Executar(dto));
            Assert.Contains("Não é possível cadastrar aula de reposição com recorrência", ex.Message);
        }

        [Fact]
        public async Task Executar_DeveChamarInserirAulaUnicaCommand_QuandoRecorrenciaUnica()
        {
            // Arrange
            var dto = CriarDto(RecorrenciaAula.AulaUnica);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new Usuario());

            podeCadastrarAulaUseCaseMock.Setup(p => p.Executar(It.IsAny<FiltroPodeCadastrarAulaDto>()))
                                        .ReturnsAsync(new CadastroAulaDto { PodeCadastrarAula = true });

            mediatorMock.Setup(m => m.Send(It.IsAny<InserirAulaUnicaCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new RetornoBaseDto("Aula única cadastrada"));

            // Act
            var resultado = await inserirAulaUseCase.Executar(dto);

            // Assert
            Assert.Equal("Aula única cadastrada", resultado.Mensagens[0]);
        }

        [Fact]
        public async Task Executar_DeveRetornarMensagemRecorrente_QuandoRecorrenciaNaoUnica()
        {
            // Arrange
            var dto = CriarDto(RecorrenciaAula.RepetirBimestreAtual);

            // Campos obrigatórios para comando recorrente
            dto.NomeComponenteCurricular = "Matemática";
            dto.CodigoUe = "UE123";
            dto.TipoCalendarioId = 1;
            dto.Quantidade = 5;

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new Usuario());

            podeCadastrarAulaUseCaseMock.Setup(p => p.Executar(It.IsAny<FiltroPodeCadastrarAulaDto>()))
                                        .ReturnsAsync(new CadastroAulaDto { PodeCadastrarAula = true });

            mediatorMock.Setup(m => m.Send(It.IsAny<IncluirFilaInserirAulaRecorrenteCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            // Act
            var resultado = await inserirAulaUseCase.Executar(dto);

            // Assert
            Assert.Contains(resultado.Mensagens, msg => msg.Contains("Serão cadastradas aulas recorrentes"));
        }

        [Fact]
        public async Task Executar_DeveRetornarErroComLog_SeFalharAulaRecorrente()
        {
            // Arrange
            var dto = CriarDto(RecorrenciaAula.RepetirBimestreAtual);

            // Campos obrigatórios para comando recorrente
            dto.NomeComponenteCurricular = "Matemática";
            dto.CodigoUe = "UE123";
            dto.TipoCalendarioId = 1;
            dto.Quantidade = 5;

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new Usuario());

            podeCadastrarAulaUseCaseMock.Setup(p => p.Executar(It.IsAny<FiltroPodeCadastrarAulaDto>()))
                                        .ReturnsAsync(new CadastroAulaDto { PodeCadastrarAula = true });

            mediatorMock.Setup(m => m.Send(It.IsAny<IncluirFilaInserirAulaRecorrenteCommand>(), It.IsAny<CancellationToken>()))
                        .ThrowsAsync(new Exception("Erro ao incluir na fila"));

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            // Act
            var resultado = await inserirAulaUseCase.Executar(dto);

            // Assert
            Assert.Contains(resultado.Mensagens, msg => msg.Contains("Ocorreu um erro ao solicitar a criação de aulas recorrentes"));
            Assert.Contains(resultado.Mensagens, msg => msg.Contains("Erro ao incluir na fila"));
        }

        [Fact]
        public async Task Executar_DeveLancarExcecao_SeNaoPodeCadastrar()
        {
            // Arrange
            var dto = CriarDto(RecorrenciaAula.AulaUnica);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new Usuario());

            // Configurar para NÃO permitir cadastrar aula
            podeCadastrarAulaUseCaseMock.Setup(p => p.Executar(It.IsAny<FiltroPodeCadastrarAulaDto>()))
                                        .ReturnsAsync(new CadastroAulaDto { PodeCadastrarAula = false });

            // Act & Assert
            var ex = await Assert.ThrowsAsync<NegocioException>(() => inserirAulaUseCase.Executar(dto));
            Assert.Contains("Não é possível cadastrar aula do tipo", ex.Message);
        }

        private PersistirAulaDto CriarDto(RecorrenciaAula recorrencia)
        {
            return new PersistirAulaDto
            {
                CodigoTurma = "123",
                CodigoComponenteCurricular = 456,
                DataAula = DateTime.Today,
                EhRegencia = false,
                TipoAula = TipoAula.Normal,
                RecorrenciaAula = recorrencia
            };
        }
    }
}
