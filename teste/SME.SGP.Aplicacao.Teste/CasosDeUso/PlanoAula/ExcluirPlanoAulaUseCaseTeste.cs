using MediatR;
using Moq;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PlanoAula
{
    public class ExcluirPlanoAulaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ExcluirPlanoAulaUseCase _useCase;
        private readonly Usuario _usuario;

        public ExcluirPlanoAulaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _usuario = new Usuario();
            _useCase = new ExcluirPlanoAulaUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Usuario_Nao_Professor_Cj_Mas_Possui_Permissao_Deve_Excluir_E_Retornar_True_()
        {
            long aulaId = 123;
            long disciplinaId = 100;
            var aula = new Dominio.Aula { TurmaId = "T1", DisciplinaId = disciplinaId.ToString(), DataAula = DateTime.Now };

            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(_usuario);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAulaPorIdQuery>(q => q.AulaId == aulaId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(aula);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery>(
                q => q.ComponenteCurricularId == disciplinaId &&
                     q.CodigoTurma == aula.TurmaId &&
                     q.Data == aula.DataAula &&
                     q.Usuario == _usuario), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _mediatorMock.Setup(m => m.Send(It.Is<ExcluirPlanoAulaDaAulaCommand>(c => c.AulaId == aulaId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            var resultado = await _useCase.Executar(aulaId);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ExcluirPlanoAulaDaAulaCommand>(c => c.AulaId == aulaId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Usuario_Sem_Permissao_Deve_Lancar_Negocio_Exception_()
        {
            long aulaId = 123;
            long disciplinaId = 100;
            var aula = new Dominio.Aula { TurmaId = "T1", DisciplinaId = disciplinaId.ToString(), DataAula = DateTime.Now };
            string mensagemEsperada = "Você não pode fazer alterações ou inclusões nesta turma, componente e data.";

            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(_usuario);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAulaPorIdQuery>(q => q.AulaId == aulaId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(aula);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery>(
                q => q.ComponenteCurricularId == disciplinaId &&
                     q.CodigoTurma == aula.TurmaId &&
                     q.Data == aula.DataAula &&
                     q.Usuario == _usuario), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(aulaId));

            Assert.Equal(mensagemEsperada, exception.Message);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ExcluirPlanoAulaDaAulaCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Disciplina_Id_Invalido_Deve_Lancar_Format_Exception_()
        {
            long aulaId = 123;
            var aula = new Dominio.Aula { TurmaId = "T1", DisciplinaId = "abc", DataAula = DateTime.Now };

            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(_usuario);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAulaPorIdQuery>(q => q.AulaId == aulaId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(aula);

            await Assert.ThrowsAsync<FormatException>(() => _useCase.Executar(aulaId));

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery>(), It.IsAny<CancellationToken>()), Times.Never);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ExcluirPlanoAulaDaAulaCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Aula_Nao_Encontrada_Deve_Lancar_Null_Reference_Exception_()
        {
            long aulaId = 123;

            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(_usuario);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAulaPorIdQuery>(q => q.AulaId == aulaId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((Dominio.Aula)null);

            await Assert.ThrowsAsync<NullReferenceException>(() => _useCase.Executar(aulaId));
        }

        [Fact]
        public async Task Executar_Quando_Usuario_Nao_Encontrado_Deve_Lancar_Null_Reference_Exception_()
        {
            long aulaId = 123;
            var aula = new Dominio.Aula { TurmaId = "T1", DisciplinaId = "100", DataAula = DateTime.Now };

            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                         .ReturnsAsync((Usuario)null);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAulaPorIdQuery>(q => q.AulaId == aulaId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(aula);

            await Assert.ThrowsAsync<NullReferenceException>(() => _useCase.Executar(aulaId));
        }
    }
}
