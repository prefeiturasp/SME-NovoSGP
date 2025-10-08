using Moq;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIdep;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.PainelEducacional
{
    public class ObterIdepQueryHandlerTeste
    {
        private readonly Mock<IRepositorioIdepPainelEducacionalConsulta> _repositorioIdepConsulta;
        private readonly ObterIdepQueryHandler _handler;

        public ObterIdepQueryHandlerTeste()
        {
            _repositorioIdepConsulta = new Mock<IRepositorioIdepPainelEducacionalConsulta>();
            _handler = new ObterIdepQueryHandler(_repositorioIdepConsulta.Object);
        }

        [Fact]
        public void Deve_Construir_Handler_Corretamente()
        {
            var handler = new ObterIdepQueryHandler(_repositorioIdepConsulta.Object);

            Assert.NotNull(handler);
        }

        [Fact]
        public void Construtor_Deve_Lancar_Excecao_Se_Repositorio_For_Nulo()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterIdepQueryHandler(null));
        }

        [Fact]
        public async Task Handle_Deve_Chamar_Repositorio_ObterTodosIdep()
        {
            var query = new ObterIdepQuery();
            var cancellationToken = CancellationToken.None;

            await _handler.Handle(query, cancellationToken);

            _repositorioIdepConsulta.Verify(x => x.ObterTodosIdep(), Times.Once);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Resultado_Do_Repositorio()
        {
            var query = new ObterIdepQuery();
            var cancellationToken = CancellationToken.None;
            var dadosEsperados = new List<PainelEducacionalConsolidacaoIdep>
            {
                new PainelEducacionalConsolidacaoIdep(),
                new PainelEducacionalConsolidacaoIdep()
            };

            _repositorioIdepConsulta
                .Setup(x => x.ObterTodosIdep())
                .ReturnsAsync(dadosEsperados);

            var resultado = await _handler.Handle(query, cancellationToken);

            Assert.NotNull(resultado);
            Assert.Equal(dadosEsperados, resultado);
            Assert.Equal(2, resultado.Count());
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Lista_Vazia_Se_Repositorio_Retornar_Nulo()
        {
            var query = new ObterIdepQuery();
            var cancellationToken = CancellationToken.None;

            _repositorioIdepConsulta
                .Setup(x => x.ObterTodosIdep())
                .ReturnsAsync((IEnumerable<PainelEducacionalConsolidacaoIdep>)null);

            var resultado = await _handler.Handle(query, cancellationToken);

            Assert.Null(resultado);
        }

        [Fact]
        public async Task Handle_Deve_Repassar_CancellationToken()
        {
            var query = new ObterIdepQuery();
            var cancellationToken = new CancellationToken(true);

            _repositorioIdepConsulta
                .Setup(x => x.ObterTodosIdep())
                .ReturnsAsync(new List<PainelEducacionalConsolidacaoIdep>());

            await _handler.Handle(query, cancellationToken);

            _repositorioIdepConsulta.Verify(x => x.ObterTodosIdep(), Times.Once);
        }
    }
}