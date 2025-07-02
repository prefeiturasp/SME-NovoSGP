using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.AnotacaoFrequenciaAluno
{
    public class AlterarAnotacaoFrequenciaAlunoUseCaseTeste
    {
        [Fact]
        public async Task Executar_DeveRetornarTrue_QuandoAlteracaoForBemSucedida()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();

            var idAnotacao = 1L;
            var aulaId = 100L;

            var anotacaoExistente = new SME.SGP.Dominio.AnotacaoFrequenciaAluno
            {
                Id = idAnotacao,
                AulaId = aulaId,
                Anotacao = "Anotação Original",
                MotivoAusenciaId = 1
            };

            var aula = new Aula
            {
                Id = aulaId,
                TurmaId = "1234",
                DisciplinaId = "5678",
                DataAula = DateTime.Today
            };

            var usuario = new Usuario
            {
                PerfilAtual = Perfis.PERFIL_DIRETOR  // Pra pular validação de permissão
            };

            var novoTextoAnotacao = "Nova Anotação";

            var dto = new AlterarAnotacaoFrequenciaAlunoDto
            {
                Id = idAnotacao,
                MotivoAusenciaId = 2,
                Anotacao = novoTextoAnotacao
            };

            // Setup: ObterAnotacaoFrequenciaAlunoPorIdQuery
            mediatorMock
                .Setup(m => m.Send(It.Is<ObterAnotacaoFrequenciaAlunoPorIdQuery>(q => q.Id == idAnotacao), It.IsAny<CancellationToken>()))
                .ReturnsAsync(anotacaoExistente);

            // Setup: ObterAulaPorIdQuery
            mediatorMock
                .Setup(m => m.Send(It.Is<ObterAulaPorIdQuery>(q => q.AulaId == aulaId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aula);

            // Setup: ObterUsuarioLogadoQuery
            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);

            // Setup: MoverArquivosTemporariosCommand (não precisa mudar o texto neste exemplo)
            mediatorMock
                .Setup(m => m.Send(It.IsAny<MoverArquivosTemporariosCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(novoTextoAnotacao);

            // Setup: RemoverArquivosExcluidosCommand
            mediatorMock
                .Setup(m => m.Send(It.IsAny<RemoverArquivosExcluidosCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Setup: AlterarAnotacaoFrequenciaAlunoCommand
            mediatorMock
                .Setup(m => m.Send(It.IsAny<AlterarAnotacaoFrequenciaAlunoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var useCase = new AlterarAnotacaoFrequenciaAlunoUseCase(mediatorMock.Object);

            // Act
            var resultado = await useCase.Executar(dto);

            // Assert
            Assert.True(resultado);

            // Verifica se os comandos principais foram enviados
            mediatorMock.Verify(m => m.Send(It.IsAny<AlterarAnotacaoFrequenciaAlunoCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
