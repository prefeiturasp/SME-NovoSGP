using MediatR;
using Moq;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Consultas
{
    public class ConsultasPendenciaFechamentoTeste
    {
        private readonly Mock<IRepositorioPendenciaFechamento> repositorioMock;
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IContextoAplicacao> contextoAplicacaoMock;
        private readonly ConsultasPendenciaFechamento consultas;

        public ConsultasPendenciaFechamentoTeste()
        {
            repositorioMock = new Mock<IRepositorioPendenciaFechamento>();
            mediatorMock = new Mock<IMediator>();
            contextoAplicacaoMock = new Mock<IContextoAplicacao>();

            consultas = new ConsultasPendenciaFechamento(
                contextoAplicacaoMock.Object,
                repositorioMock.Object,
                mediatorMock.Object);
        }

        [Fact]
        public async Task Listar_Deve_Retornar_Pendencias_Com_Nomes()
        {
            var filtro = new FiltroPendenciasFechamentosDto
            {
                TurmaCodigo = "TURMA1",
                Bimestre = 1,
                ComponenteCurricularId = 123
            };

            var pendencias = new List<PendenciaFechamentoResumoDto>
            {
                new PendenciaFechamentoResumoDto
                {
                    PendenciaId = 1,
                    Situacao = (int)SituacaoPendencia.Pendente,
                    DisciplinaId = 456
                }
            };

            var paginacaoResultado = new PaginacaoResultadoDto<PendenciaFechamentoResumoDto>
            {
                TotalRegistros = 1,
                Items = pendencias
            };

            repositorioMock
                .Setup(r => r.ListarPaginada(It.IsAny<Paginacao>(), filtro.TurmaCodigo, filtro.Bimestre, filtro.ComponenteCurricularId))
                .ReturnsAsync(paginacaoResultado);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesPorIdsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DisciplinaDto>
                {
                    new DisciplinaDto { CodigoComponenteCurricular = 456, Nome = "Matemática" }
                });

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterDisciplinasPorCodigoTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DisciplinaResposta>());

            var resultado = await consultas.Listar(filtro);

            Assert.NotNull(resultado);
            Assert.Single(resultado.Items);
            var pendencia = resultado.Items.First();
            Assert.Equal("Matemática", pendencia.ComponenteCurricular);
            Assert.Equal(nameof(SituacaoPendencia.Pendente), pendencia.SituacaoNome);
        }

        [Fact]
        public async Task Obter_Por_Pendencia_Id_Deve_Retornar_Pendencia_Com_Nome()
        {
            var pendenciaId = 1L;
            var pendencia = new PendenciaFechamentoCompletoDto
            {
                PendenciaId = pendenciaId,
                Situacao = (int)SituacaoPendencia.Resolvida,
                DisciplinaId = 456
            };

            repositorioMock
                .Setup(r => r.ObterPorPendenciaId(pendenciaId))
                .ReturnsAsync(pendencia);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterComponenteCurricularPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DisciplinaDto
                {
                    CodigoComponenteCurricular = 456,
                    Nome = "História"
                });

            var resultado = await consultas.ObterPorPendenciaId(pendenciaId);

            Assert.NotNull(resultado);
            Assert.Equal(pendenciaId, resultado.PendenciaId);
            Assert.Equal("História", resultado.ComponenteCurricular);
            Assert.Equal(nameof(SituacaoPendencia.Resolvida), resultado.SituacaoNome);
        }

        [Fact]
        public async Task Obter_Por_Pendencia_Id_Deve_Lancar_Excecao_Quando_Nao_Encontrar_Pendencia()
        {
            var pendenciaId = 999L;

            repositorioMock
                .Setup(r => r.ObterPorPendenciaId(pendenciaId))
                .ReturnsAsync((PendenciaFechamentoCompletoDto)null);

            var ex = await Assert.ThrowsAsync<NegocioException>(() => consultas.ObterPorPendenciaId(pendenciaId));
            Assert.Equal("Pendencia informada não localizada.", ex.Message);
        }

        [Fact]
        public async Task Obter_Por_Pendencia_Id_Deve_Lancar_Excecao_Quando_Componente_Nao_Encontrado()
        {
            var pendenciaId = 1L;
            var pendencia = new PendenciaFechamentoCompletoDto
            {
                PendenciaId = pendenciaId,
                Situacao = (int)SituacaoPendencia.Resolvida,
                DisciplinaId = 456
            };

            repositorioMock
                .Setup(r => r.ObterPorPendenciaId(pendenciaId))
                .ReturnsAsync(pendencia);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterComponenteCurricularPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((DisciplinaDto)null);

            var ex = await Assert.ThrowsAsync<NegocioException>(() => consultas.ObterPorPendenciaId(pendenciaId));
            Assert.Equal("Componente curricular informado não localizado.", ex.Message);
        }
    }
}
