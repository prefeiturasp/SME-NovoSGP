using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.AcompanhamentoAluno
{
    public class ObterAcompanhamentoAlunoUseCaseTeste
    {
        [Fact]
        public async Task Deve_Obter_Acompanhamento_Aluno_Turma_Semestre()
        {
            var mediatorMock = new Mock<IMediator>();
            var useCase = new ObterAcompanhamentoAlunoUseCase(mediatorMock.Object);

            var filtro = new FiltroAcompanhamentoTurmaAlunoSemestreDto(1, "12345", 1, 10);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaCodigoPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("TURMA01");

            var turma = new Turma
            {
                Id = 1,
                CodigoTurma = "TURMA01",
                AnoLetivo = 2024,
                ModalidadeCodigo = Modalidade.Fundamental,
                Semestre = 1
            };
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            var acompanhamentoSemestre = new AcompanhamentoAlunoSemestre
            {
                AcompanhamentoAlunoId = 100,
                Id = 200,
                Observacoes = "Observação teste",
                PercursoIndividual = "Percurso teste"
            };
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAcompanhamentoPorAlunoTurmaESemestreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(acompanhamentoSemestre);

            var periodosEscolares = new List<PeriodoEscolar>
            {
                new PeriodoEscolar { PeriodoInicio = new DateTime(2024, 2, 1), PeriodoFim = new DateTime(2024, 6, 30), Bimestre = 1 },
                new PeriodoEscolar { PeriodoInicio = new DateTime(2024, 7, 1), PeriodoFim = new DateTime(2024, 12, 15), Bimestre = 2 }
            };
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(periodosEscolares);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoCalendarioIdPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1L);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaEmPeriodoDeFechamentoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterDescricoesRegistrosIndividuaisPorPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<RegistroIndividualResumoDto>());

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ParametrosSistema { Valor = "5" });

            var resultado = await useCase.Executar(filtro);

            Assert.NotNull(resultado);
            Assert.Equal(acompanhamentoSemestre.AcompanhamentoAlunoId, resultado.AcompanhamentoAlunoId);        
            Assert.Equal(acompanhamentoSemestre.Observacoes, resultado.Observacoes);
            Assert.Equal(acompanhamentoSemestre.PercursoIndividual, resultado.PercursoIndividual);
            Assert.Equal(5, resultado.QuantidadeFotos);
            Assert.True(resultado.PodeEditar);
        }
    }
}