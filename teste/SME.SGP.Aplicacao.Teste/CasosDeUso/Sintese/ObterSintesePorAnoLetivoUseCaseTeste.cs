using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Sintese
{
    public class ObterSintesePorAnoLetivoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IRepositorioSintese> _repositorioSinteseMock;
        private readonly ObterSintesePorAnoLetivoUseCase _useCase;

        public ObterSintesePorAnoLetivoUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _repositorioSinteseMock = new Mock<IRepositorioSintese>();
            _useCase = new ObterSintesePorAnoLetivoUseCase(_mediatorMock.Object, _repositorioSinteseMock.Object);
        }

        [Fact]
        public void Executar_Quando_Repositorio_Nulo_Deve_Lancar_Argument_Null_Exception_()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterSintesePorAnoLetivoUseCase(_mediatorMock.Object, null));
        }

        [Fact]
        public async Task Executar_Quando_Repositorio_Retorna_Nulo_Deve_Lancar_Negocio_Exception_()
        {
            var anoLetivo = 2025;
            _repositorioSinteseMock.Setup(r => r.ObterPorData(It.IsAny<DateTime>()))
                                   .Returns((IEnumerable<Dominio.Sintese>)null);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(anoLetivo));
            Assert.Equal("Não foi possível obter as sínteses", exception.Message);
        }

        [Fact]
        public async Task Executar_Quando_Repositorio_Retorna_Vazio_Deve_Lancar_Negocio_Exception_()
        {
            var anoLetivo = 2025;
            _repositorioSinteseMock.Setup(r => r.ObterPorData(It.IsAny<DateTime>()))
                                   .Returns(new List<Dominio.Sintese>());

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(anoLetivo));
            Assert.Equal("Não foi possível obter as sínteses", exception.Message);
        }

        [Fact]
        public async Task Executar_Quando_Repositorio_Retorna_Dados_Deve_Mapear_E_Retornar_Com_Sucesso_()
        {
            var anoLetivo = 2025;
            var dataReferencia = new DateTime(anoLetivo, 6, 28);
            var sintesesDominio = new List<Dominio.Sintese>
            {
                new Dominio.Sintese { Id = 1, Descricao = "Frequente" },
                new Dominio.Sintese { Id = 2, Descricao = "Não frequente" }
            };

            _repositorioSinteseMock.Setup(r => r.ObterPorData(dataReferencia))
                                   .Returns(sintesesDominio);

            var resultado = await _useCase.Executar(anoLetivo);

            _repositorioSinteseMock.Verify(r => r.ObterPorData(dataReferencia), Times.Once);
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Equal(SinteseEnum.Frequente, resultado.First().Id);
            Assert.Equal("Frequente", resultado.First().Valor);
            Assert.Equal(SinteseEnum.NaoFrequente, resultado.Last().Id);
            Assert.Equal("Não frequente", resultado.Last().Valor);
        }
    }
}
