using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PlanejamentoAnual.ObterTurmasParaCopia
{
    public class ObterTurmasParaCopiaUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterTurmasParaCopiaUseCase useCase;

        public ObterTurmasParaCopiaUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterTurmasParaCopiaUseCase(mediatorMock.Object);
        }

        [Fact]
        public void Construtor_Quando_Mediator_Nulo_Deve_Lancar_ArgumentNullException_()
        {
            Action act = () => new ObterTurmasParaCopiaUseCase(null);
            Assert.Throws<ArgumentNullException>("mediator", act);
        }

        [Fact]
        public async Task Executar_Quando_Usuario_For_CP_Deve_Chamar_Query_CP_()
        {
            var turmaId = 123;
            var componenteCurricularId = 456L;
            var ensinoEspecial = false;
            var consideraHistorico = true;

            var usuario = new Usuario
            {
                PerfilAtual = Dominio.Perfis.PERFIL_CP,
                CodigoRf = "rf123"
            };

            var retornoEsperado = new List<TurmaParaCopiaPlanoAnualDto>
            {
                new TurmaParaCopiaPlanoAnualDto { TurmaId = 999 }
            };

            mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);

            mediatorMock.Setup(m => m.Send(It.Is<ObterTurmasParaCopiaPlanejamentoAnualUsuarioCPQuery>(q =>
                    q.TurmaId == turmaId &&
                    q.EnsinoEspecial == ensinoEspecial &&
                    q.ConsideraHistorico == consideraHistorico), It.IsAny<CancellationToken>()))
                .ReturnsAsync(retornoEsperado);

            var resultado = await useCase.Executar(turmaId, componenteCurricularId, ensinoEspecial, consideraHistorico);

            Assert.NotNull(resultado);
            Assert.Equal(retornoEsperado.First().TurmaId, resultado.First().TurmaId);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterTurmasParaCopiaPlanejamentoAnualUsuarioCPQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterTurmasEOLParaCopiaPorIdEComponenteCurricularIdQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Usuario_Nao_For_CP_Deve_Chamar_Query_EOL_()
        {
            var turmaId = 123;
            var componenteCurricularId = 456L;
            var ensinoEspecial = true;
            var consideraHistorico = false;
            var codigoRf = "rf789";

            var usuario = new Usuario
            {
                PerfilAtual = Dominio.Perfis.PERFIL_AD,
                CodigoRf = codigoRf
            };

            var retornoEsperado = new List<TurmaParaCopiaPlanoAnualDto>
            {
                new TurmaParaCopiaPlanoAnualDto { TurmaId = 888 }
            };

            mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);

            mediatorMock.Setup(m => m.Send(It.Is<ObterTurmasEOLParaCopiaPorIdEComponenteCurricularIdQuery>(q =>
                    q.TurmaId == turmaId &&
                    q.ComponenteCurricularId == componenteCurricularId &&
                    q.CodigoRF == codigoRf &&
                    q.ConsideraHistorico == consideraHistorico), It.IsAny<CancellationToken>()))
                .ReturnsAsync(retornoEsperado);

            var resultado = await useCase.Executar(turmaId, componenteCurricularId, ensinoEspecial, consideraHistorico);

            Assert.NotNull(resultado);
            Assert.Equal(retornoEsperado.First().TurmaId, resultado.First().TurmaId);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterTurmasParaCopiaPlanejamentoAnualUsuarioCPQuery>(), It.IsAny<CancellationToken>()), Times.Never);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterTurmasEOLParaCopiaPorIdEComponenteCurricularIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
