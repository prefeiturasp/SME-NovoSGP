using MediatR;
using Moq;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ExcluirAtribuicaoEsporadicaTeste
{
    public class ExcluirAtribuicaoEsporadicaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ExcluirAtribuicaoEsporadicaUseCase _useCase;

        public ExcluirAtribuicaoEsporadicaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExcluirAtribuicaoEsporadicaUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_ShouldReturnTrueAndExcludeAtribuicao_WhenNoActiveCJAtribuicaoExists()
        {
            long atribuicaoId = 1;
            var atribuicaoEsporadica = new AtribuicaoEsporadica
            {
                Id = 1,
                DataInicio = DateTime.Now,
                DataFim = DateTime.Now.AddDays(5),
                ProfessorRf = "professorRf",
                UeId = "ueId",
                DreId = "dreId",
                AnoLetivo = 2024
            };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAtribuicaoEsporadicaPorIdQuery>(q => q.Id == atribuicaoId), default))
                .ReturnsAsync(atribuicaoEsporadica);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAtribuicoesPorTurmaEProfessorQuery>(), default))
                .ReturnsAsync(new List<Dominio.AtribuicaoCJ>()); 

            _mediatorMock.Setup(m => m.Send(It.IsAny<SalvarAtribuicaoEsporadicaCommand>(), default))
                .ReturnsAsync(1L);

            var result = await _useCase.Executar(atribuicaoId);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterAtribuicaoEsporadicaPorIdQuery>(q => q.Id == atribuicaoId), default), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterAtribuicoesPorTurmaEProfessorQuery>(q =>
                q.UeId == atribuicaoEsporadica.UeId &&
                q.UsuarioRf == atribuicaoEsporadica.ProfessorRf &&
                q.DreCodigo == atribuicaoEsporadica.DreId &&
                q.AnoLetivo == atribuicaoEsporadica.AnoLetivo
            ), default), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<SalvarAtribuicaoEsporadicaCommand>(cmd => cmd.AtribuicaoEsporadica.Excluido), default), Times.Once);

            Assert.True(result);
            Assert.True(atribuicaoEsporadica.Excluido);
        }

        [Fact]
        public async Task Executar_ShouldThrowNegocioException_WhenAtribuicaoEsporadicaNotFound()
        {
            long atribuicaoId = 1;
            AtribuicaoEsporadica atribuicaoEsporadica = null;

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAtribuicaoEsporadicaPorIdQuery>(q => q.Id == atribuicaoId), default))
                .ReturnsAsync(atribuicaoEsporadica);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(atribuicaoId));

            Assert.Equal("Não foi possível localizar esta atribuição esporádica.", exception.Message);
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarAtribuicaoEsporadicaCommand>(), default), Times.Never);
        }

        [Fact]
        public async Task Executar_ShouldThrowNegocioException_WhenActiveCJAtribuicaoExists()
        {
            long atribuicaoId = 1;
            var atribuicaoEsporadica = new AtribuicaoEsporadica
            {
                Id = 1,
                DataInicio = DateTime.Now,
                DataFim = DateTime.Now.AddDays(5),
                ProfessorRf = "professorRf",
                UeId = "ueId",
                DreId = "dreId",
                AnoLetivo = 2024
            };
            var activeCjAtribuicoes = new List<Dominio.AtribuicaoCJ> { new Dominio.AtribuicaoCJ() };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAtribuicaoEsporadicaPorIdQuery>(q => q.Id == atribuicaoId), default))
                .ReturnsAsync(atribuicaoEsporadica);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAtribuicoesPorTurmaEProfessorQuery>(), default))
                .ReturnsAsync(activeCjAtribuicoes);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(atribuicaoId));

            Assert.Equal("Este professor possui atribuição CJ Ativa no período informado.", exception.Message);
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarAtribuicaoEsporadicaCommand>(), default), Times.Never);
            Assert.False(atribuicaoEsporadica.Excluido);
        }
    }
}
