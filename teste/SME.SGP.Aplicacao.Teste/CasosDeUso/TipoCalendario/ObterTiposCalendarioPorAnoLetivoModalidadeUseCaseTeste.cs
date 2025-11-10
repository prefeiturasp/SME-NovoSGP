using MediatR;
using Moq;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.TipoCalendario
{
    public class ObterTiposCalendarioPorAnoLetivoModalidadeUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterTiposCalendarioPorAnoLetivoModalidadeUseCase _useCase;

        public ObterTiposCalendarioPorAnoLetivoModalidadeUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterTiposCalendarioPorAnoLetivoModalidadeUseCase(_mediatorMock.Object);
        }

        [Fact]
        public void Executar_Quando_Construtor_Com_Mediator_Nulo_Deve_Lancar_Excecao()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterTiposCalendarioPorAnoLetivoModalidadeUseCase(null));
        }

        [Fact]
        public async Task Executar_Quando_Parametros_Validos_Deve_Filtrar_E_Mapear_Retorno_Com_Sucesso()
        {
            var anoLetivo = 2025;
            var modalidadesEmString = "1,3";
            var semestre = 1;
            var modalidadesComoArray = new[] { 1, 3 };

            var listaRetornoQuery = new List<Dominio.TipoCalendario>
            {
                new Dominio.TipoCalendario { Id = 1, Nome = "Valido", Situacao = true, Excluido = false, AnoLetivo = anoLetivo, Modalidade = ModalidadeTipoCalendario.FundamentalMedio },
                new Dominio.TipoCalendario { Id = 2, Nome = "Situacao Invalida", Situacao = false, Excluido = false, AnoLetivo = anoLetivo, Modalidade = ModalidadeTipoCalendario.Infantil },
                new Dominio.TipoCalendario { Id = 3, Nome = "Excluido", Situacao = true, Excluido = true, AnoLetivo = anoLetivo, Modalidade = ModalidadeTipoCalendario.EJA },
                new Dominio.TipoCalendario { Id = 4, Nome = "Ambos Invalidos", Situacao = false, Excluido = true, AnoLetivo = anoLetivo, Modalidade = ModalidadeTipoCalendario.CELP }
            };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTiposCalendarioPorAnoLetivoModalidadeQuery>(q =>
                    q.AnoLetivo == anoLetivo &&
                    q.Modalidades.Select(m => (int)m).SequenceEqual(modalidadesComoArray) &&
                    q.Semestre == semestre),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(listaRetornoQuery);

            var resultado = await _useCase.Executar(anoLetivo, modalidadesEmString, semestre);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterTiposCalendarioPorAnoLetivoModalidadeQuery>(q =>
                    q.AnoLetivo == anoLetivo &&
                    q.Modalidades.Select(m => (int)m).SequenceEqual(modalidadesComoArray) &&
                    q.Semestre == semestre),
                It.IsAny<CancellationToken>()), Times.Once);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal(1, resultado.First().Id);
            Assert.Equal("Valido", resultado.First().Nome);
        }
    }
}
