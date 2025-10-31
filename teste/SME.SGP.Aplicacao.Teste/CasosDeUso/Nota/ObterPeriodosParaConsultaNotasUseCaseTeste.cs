using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Nota
{
    public class ObterPeriodosParaConsultaNotasUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterPeriodosParaConsultaNotasUseCase _useCase;

        public ObterPeriodosParaConsultaNotasUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterPeriodosParaConsultaNotasUseCase(_mediatorMock.Object);
        }

        [Fact]
        public void Constructor_NullMediator_Deve_Lancar_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterPeriodosParaConsultaNotasUseCase(null));
        }

        [Fact]
        public async Task Executar_Sem_Periodos_Retorna_NegocioException()
        {
            var filtro = new ObterPeriodosParaConsultaNotasFiltroDto
            {
                AnoLetivo = 2025,
                Modalidade = Modalidade.EducacaoInfantil,
                Semestre = 1
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterPeriodoCalendarioBimestrePorAnoLetivoModalidadeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<PeriodoCalendarioBimestrePorAnoLetivoModalidadeDto>)null);

            var ex = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(filtro));
            Assert.Equal("Não foram encontrados períodos escolares para esta turma.", ex.Message);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterPeriodoCalendarioBimestrePorAnoLetivoModalidadeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PeriodoCalendarioBimestrePorAnoLetivoModalidadeDto>());

            ex = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(filtro));
            Assert.Equal("Não foram encontrados períodos escolares para esta turma.", ex.Message);
        }

        [Fact]
        public async Task Executar_Com_Periodos_Retorna_Lista_Periodos_Com_Bimestre_Atual()
        {
            var dataAtual = DateTimeExtension.HorarioBrasilia();
            var periodo1 = new PeriodoCalendarioBimestrePorAnoLetivoModalidadeDto
            {
                PeriodoEscolarId = 1,
                Bimestre = 1,
                PeriodoInicio = dataAtual.AddDays(-10),
                PeriodoFim = dataAtual.AddDays(10)
            };
            var periodo2 = new PeriodoCalendarioBimestrePorAnoLetivoModalidadeDto
            {
                PeriodoEscolarId = 2,
                Bimestre = 2,
                PeriodoInicio = dataAtual.AddDays(11),
                PeriodoFim = dataAtual.AddDays(30)
            };

            var periodos = new List<PeriodoCalendarioBimestrePorAnoLetivoModalidadeDto> { periodo1, periodo2 };

            var filtro = new ObterPeriodosParaConsultaNotasFiltroDto
            {
                AnoLetivo = 2025,
                Modalidade = Modalidade.EducacaoInfantil,
                Semestre = 1
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterPeriodoCalendarioBimestrePorAnoLetivoModalidadeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(periodos);

            var resultado = await _useCase.Executar(filtro);

            Assert.NotNull(resultado);
            var lista = resultado.ToList();
            Assert.Equal(2, lista.Count);

            var bimestreAtualDto = lista.FirstOrDefault(p => p.EhBimestreAtual);
            Assert.NotNull(bimestreAtualDto);
            Assert.Equal(periodo1.Bimestre, bimestreAtualDto.Bimestre);
            Assert.True(bimestreAtualDto.EhBimestreAtual);
        }

        [Fact]
        public async Task Obter_Bimestre_Atual_Data_Nao_Esta_Em_Nenhum_Periodo_Deve_Retornar_1()
        {
            var dataAtual = DateTimeExtension.HorarioBrasilia();

            var periodos = new List<PeriodoCalendarioBimestrePorAnoLetivoModalidadeDto>
            {
                new PeriodoCalendarioBimestrePorAnoLetivoModalidadeDto
                {
                    PeriodoEscolarId = 1,
                    Bimestre = 1,
                    PeriodoInicio = dataAtual.AddDays(-20),
                    PeriodoFim = dataAtual.AddDays(-10)
                },
                new PeriodoCalendarioBimestrePorAnoLetivoModalidadeDto
                {
                    PeriodoEscolarId = 2,
                    Bimestre = 2,
                    PeriodoInicio = dataAtual.AddDays(10),
                    PeriodoFim = dataAtual.AddDays(20)
                }
            };

            var metodo = typeof(ObterPeriodosParaConsultaNotasUseCase).GetMethod("ObterBimestreAtual", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var resultado = (int)metodo.Invoke(_useCase, new object[] { periodos });

            Assert.Equal(1, resultado);
        }

        [Fact]
        public async Task Obter_Bimestre_Atual_Data_Dentro_De_Um_Periodo_Deve_Retornar_Bimestre_Do_Periodo()
        {
            var dataAtual = DateTimeExtension.HorarioBrasilia();

            var periodo = new PeriodoCalendarioBimestrePorAnoLetivoModalidadeDto
            {
                PeriodoEscolarId = 1,
                Bimestre = 3,
                PeriodoInicio = dataAtual.AddDays(-5),
                PeriodoFim = dataAtual.AddDays(5)
            };

            var outrosPeriodos = new List<PeriodoCalendarioBimestrePorAnoLetivoModalidadeDto> { periodo };

            var metodo = typeof(ObterPeriodosParaConsultaNotasUseCase).GetMethod("ObterBimestreAtual", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var resultado = (int)metodo.Invoke(_useCase, new object[] { outrosPeriodos });

            Assert.Equal(3, resultado);
        }
    }
}
