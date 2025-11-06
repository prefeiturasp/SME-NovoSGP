using MediatR;
using Moq;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Testes
{
    public class AlterarAnotacaoFrequenciaAlunoUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly AlterarAnotacaoFrequenciaAlunoUseCase useCase;

        public AlterarAnotacaoFrequenciaAlunoUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new AlterarAnotacaoFrequenciaAlunoUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DeveLancarExcecao_QuandoAnotacaoNaoExiste()
        {
            // Arrange
            var dto = new AlterarAnotacaoFrequenciaAlunoDto { Id = 1 };
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnotacaoFrequenciaAlunoPorIdQuery>(), default))
                        .ReturnsAsync((AnotacaoFrequenciaAluno)null!);

            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));
        }

        [Fact]
        public async Task Executar_DeveChamarValidarAtribuicao_QuandoUsuarioNaoProfessorNemGestor()
        {
            // Arrange
            var dto = new AlterarAnotacaoFrequenciaAlunoDto
            {
                Id = 1,
                Anotacao = "nova-anotacao.png",
                MotivoAusenciaId = 2
            };

            var anotacao = new AnotacaoFrequenciaAluno { AulaId = 10, Anotacao = "antiga-anotacao.png" };
            var aula = new Aula { DataAula = DateTime.Today, DisciplinaId = "123", TurmaId = "TURMA1" };
            var usuario = CriarUsuarioProfessorRegular();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnotacaoFrequenciaAlunoPorIdQuery>(), default))
                        .ReturnsAsync(anotacao);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAulaPorIdQuery>(), default))
                        .ReturnsAsync(aula);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), default))
                        .ReturnsAsync(usuario);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery>(), default))
                        .ReturnsAsync(true);
            mediatorMock.Setup(m => m.Send(It.IsAny<MoverArquivosTemporariosCommand>(), default))
                        .ReturnsAsync("nova-anotacao-final.png");
            mediatorMock.Setup(m => m.Send(It.IsAny<RemoverArquivosExcluidosCommand>(), default))
                        .ReturnsAsync(true);
            mediatorMock.Setup(m => m.Send(It.IsAny<AlterarAnotacaoFrequenciaAlunoCommand>(), default))
                        .ReturnsAsync(true);

            // Act
            var resultado = await useCase.Executar(dto);

            // Assert
            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery>(), default), Times.Once);
        }

        [Fact]
        public async Task Executar_DeveLancarExcecao_SeUsuarioNaoTemPermissao()
        {
            // Arrange
            var dto = new AlterarAnotacaoFrequenciaAlunoDto { Id = 1 };
            var anotacao = new AnotacaoFrequenciaAluno { AulaId = 99, Anotacao = "a.png" };
            var aula = new Aula { DataAula = DateTime.Today, DisciplinaId = "999", TurmaId = "TURMA99" };
            var usuario = CriarUsuarioProfessorRegular();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnotacaoFrequenciaAlunoPorIdQuery>(), default)).ReturnsAsync(anotacao);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAulaPorIdQuery>(), default)).ReturnsAsync(aula);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), default)).ReturnsAsync(usuario);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery>(), default)).ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));
        }

        [Fact]
        public async Task Executar_NaoDeveChamarValidarAtribuicao_QuandoUsuarioEhProfessorCj()
        {
            // Arrange
            var dto = new AlterarAnotacaoFrequenciaAlunoDto
            {
                Id = 1,
                Anotacao = "nova-anotacao.png",
                MotivoAusenciaId = 2
            };

            var anotacao = new AnotacaoFrequenciaAluno { AulaId = 10, Anotacao = "antiga-anotacao.png" };
            var aula = new Aula { DataAula = DateTime.Today, DisciplinaId = "123", TurmaId = "TURMA1" };
            var usuario = CriarUsuarioProfessorCj();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnotacaoFrequenciaAlunoPorIdQuery>(), default))
                        .ReturnsAsync(anotacao);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAulaPorIdQuery>(), default))
                        .ReturnsAsync(aula);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), default))
                        .ReturnsAsync(usuario);
            mediatorMock.Setup(m => m.Send(It.IsAny<MoverArquivosTemporariosCommand>(), default))
                        .ReturnsAsync("nova-anotacao-final.png");
            mediatorMock.Setup(m => m.Send(It.IsAny<RemoverArquivosExcluidosCommand>(), default))
                        .ReturnsAsync(true);
            mediatorMock.Setup(m => m.Send(It.IsAny<AlterarAnotacaoFrequenciaAlunoCommand>(), default))
                        .ReturnsAsync(true);

            // Act
            var resultado = await useCase.Executar(dto);

            // Assert
            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery>(), default), Times.Never);
        }

        [Fact]
        public async Task Executar_NaoDeveChamarValidarAtribuicao_QuandoUsuarioEhGestorEscolar()
        {
            // Arrange
            var dto = new AlterarAnotacaoFrequenciaAlunoDto
            {
                Id = 1,
                Anotacao = "nova-anotacao.png",
                MotivoAusenciaId = 2
            };

            var anotacao = new AnotacaoFrequenciaAluno { AulaId = 10, Anotacao = "antiga-anotacao.png" };
            var aula = new Aula { DataAula = DateTime.Today, DisciplinaId = "123", TurmaId = "TURMA1" };
            var usuario = CriarUsuarioGestorEscolar();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnotacaoFrequenciaAlunoPorIdQuery>(), default))
                        .ReturnsAsync(anotacao);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAulaPorIdQuery>(), default))
                        .ReturnsAsync(aula);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), default))
                        .ReturnsAsync(usuario);
            mediatorMock.Setup(m => m.Send(It.IsAny<MoverArquivosTemporariosCommand>(), default))
                        .ReturnsAsync("nova-anotacao-final.png");
            mediatorMock.Setup(m => m.Send(It.IsAny<RemoverArquivosExcluidosCommand>(), default))
                        .ReturnsAsync(true);
            mediatorMock.Setup(m => m.Send(It.IsAny<AlterarAnotacaoFrequenciaAlunoCommand>(), default))
                        .ReturnsAsync(true);

            // Act
            var resultado = await useCase.Executar(dto);

            // Assert
            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery>(), default), Times.Never);
        }

        [Fact]
        public async Task Executar_NaoDeveMoverArquivos_QuandoAnotacaoNovaEstaVazia()
        {
            // Arrange
            var dto = new AlterarAnotacaoFrequenciaAlunoDto
            {
                Id = 1,
                Anotacao = "",
                MotivoAusenciaId = 2
            };

            var anotacao = new AnotacaoFrequenciaAluno { AulaId = 10, Anotacao = "antiga-anotacao.png" };
            var aula = new Aula { DataAula = DateTime.Today, DisciplinaId = "123", TurmaId = "TURMA1" };
            var usuario = CriarUsuarioProfessorRegular();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnotacaoFrequenciaAlunoPorIdQuery>(), default))
                        .ReturnsAsync(anotacao);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAulaPorIdQuery>(), default))
                        .ReturnsAsync(aula);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), default))
                        .ReturnsAsync(usuario);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery>(), default))
                        .ReturnsAsync(true);
            mediatorMock.Setup(m => m.Send(It.IsAny<RemoverArquivosExcluidosCommand>(), default))
                        .ReturnsAsync(true);
            mediatorMock.Setup(m => m.Send(It.IsAny<AlterarAnotacaoFrequenciaAlunoCommand>(), default))
                        .ReturnsAsync(true);

            // Act
            var resultado = await useCase.Executar(dto);

            // Assert
            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<MoverArquivosTemporariosCommand>(), default), Times.Never);
        }

        [Fact]
        public async Task Executar_NaoDeveRemoverArquivos_QuandoAnotacaoAntigaEstaVazia()
        {
            // Arrange
            var dto = new AlterarAnotacaoFrequenciaAlunoDto
            {
                Id = 1,
                Anotacao = "nova-anotacao.png",
                MotivoAusenciaId = 2
            };

            var anotacao = new AnotacaoFrequenciaAluno { AulaId = 10, Anotacao = "" };
            var aula = new Aula { DataAula = DateTime.Today, DisciplinaId = "123", TurmaId = "TURMA1" };
            var usuario = CriarUsuarioProfessorRegular();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnotacaoFrequenciaAlunoPorIdQuery>(), default))
                        .ReturnsAsync(anotacao);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAulaPorIdQuery>(), default))
                        .ReturnsAsync(aula);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), default))
                        .ReturnsAsync(usuario);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery>(), default))
                        .ReturnsAsync(true);
            mediatorMock.Setup(m => m.Send(It.IsAny<MoverArquivosTemporariosCommand>(), default))
                        .ReturnsAsync("nova-anotacao-final.png");
            mediatorMock.Setup(m => m.Send(It.IsAny<AlterarAnotacaoFrequenciaAlunoCommand>(), default))
                        .ReturnsAsync(true);

            // Act
            var resultado = await useCase.Executar(dto);

            // Assert
            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<RemoverArquivosExcluidosCommand>(), default), Times.Never);
        }

        [Fact]
        public async Task Executar_DeveExecutarComSucesso_CenarioCompleto()
        {
            // Arrange
            var dto = new AlterarAnotacaoFrequenciaAlunoDto
            {
                Id = 1,
                Anotacao = "nova-anotacao.png",
                MotivoAusenciaId = 2
            };

            var anotacao = new AnotacaoFrequenciaAluno { AulaId = 10, Anotacao = "antiga-anotacao.png" };
            var aula = new Aula { DataAula = DateTime.Today, DisciplinaId = "123", TurmaId = "TURMA1" };
            var usuario = CriarUsuarioProfessorRegular();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnotacaoFrequenciaAlunoPorIdQuery>(), default))
                        .ReturnsAsync(anotacao);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAulaPorIdQuery>(), default))
                        .ReturnsAsync(aula);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), default))
                        .ReturnsAsync(usuario);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery>(), default))
                        .ReturnsAsync(true);
            mediatorMock.Setup(m => m.Send(It.IsAny<MoverArquivosTemporariosCommand>(), default))
                        .ReturnsAsync("nova-anotacao-final.png");
            mediatorMock.Setup(m => m.Send(It.IsAny<RemoverArquivosExcluidosCommand>(), default))
                        .ReturnsAsync(true);
            mediatorMock.Setup(m => m.Send(It.IsAny<AlterarAnotacaoFrequenciaAlunoCommand>(), default))
                        .ReturnsAsync(true);

            // Act
            var resultado = await useCase.Executar(dto);

            // Assert
            Assert.True(resultado);

            // Verificar que todos os comandos foram chamados
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterAnotacaoFrequenciaAlunoPorIdQuery>(), default), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterAulaPorIdQuery>(), default), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), default), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<MoverArquivosTemporariosCommand>(), default), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<RemoverArquivosExcluidosCommand>(), default), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<AlterarAnotacaoFrequenciaAlunoCommand>(), default), Times.Once);
        }

        // Métodos auxiliares para criar usuários com perfis específicos
        private Usuario CriarUsuarioProfessorRegular()
        {
            var usuario = new Usuario();
            usuario.DefinirPerfilAtual(Dominio.Perfis.PERFIL_PROFESSOR);
            usuario.DefinirPerfis(new List<PrioridadePerfil>
            {
                new PrioridadePerfil { CodigoPerfil = Dominio.Perfis.PERFIL_PROFESSOR, Tipo = TipoPerfil.UE }
            });
            return usuario;
        }

        private Usuario CriarUsuarioProfessorCj()
        {
            var usuario = new Usuario();
            usuario.DefinirPerfilAtual(Dominio.Perfis.PERFIL_CJ);
            usuario.DefinirPerfis(new List<PrioridadePerfil>
            {
                new PrioridadePerfil { CodigoPerfil = Dominio.Perfis.PERFIL_CJ, Tipo = TipoPerfil.UE }
            });
            return usuario;
        }

        private Usuario CriarUsuarioGestorEscolar()
        {
            var usuario = new Usuario();
            usuario.DefinirPerfilAtual(Dominio.Perfis.PERFIL_CP);
            usuario.DefinirPerfis(new List<PrioridadePerfil>
            {
                new PrioridadePerfil { CodigoPerfil = Dominio.Perfis.PERFIL_CP, Tipo = TipoPerfil.UE }
            });
            return usuario;
        }
    }
}