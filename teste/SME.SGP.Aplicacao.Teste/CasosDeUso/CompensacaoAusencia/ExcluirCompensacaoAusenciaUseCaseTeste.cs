using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.CompensacaoAusencia
{
    public class ExcluirCompensacaoAusenciaUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IUnitOfWork> unitOfWorkMock;
        private readonly ExcluirCompensacaoAusenciaUseCase useCase;

        public ExcluirCompensacaoAusenciaUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            unitOfWorkMock = new Mock<IUnitOfWork>();
            useCase = new ExcluirCompensacaoAusenciaUseCase(mediatorMock.Object, unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Excluir_Compensacoes_Com_Sucesso()
        {
            var compensacaoId = 1L;
            var compensacao = new SME.SGP.Dominio.CompensacaoAusencia
            {
                Id = compensacaoId,
                Descricao = "Compensacao Teste",
                TurmaId = 10,
                DisciplinaId = "D1",
                Bimestre = 1
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterCompensacaoAusenciaPorIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(compensacao);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterCompensacaoAusenciaAlunoPorCompensacaoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<CompensacaoAusenciaAluno>());

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterCompensacaoAusenciaDisciplinaRegenciaPorIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<CompensacaoAusenciaDisciplinaRegencia>());

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterCompensacaoAusenciaAlunoAulasPorCompensacaoIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<CompensacaoAusenciaAlunoAula>());

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new Turma
                        {
                            Id = 10,
                            CodigoTurma = "T1",
                            ModalidadeCodigo = Modalidade.Fundamental,
                            Semestre = 1,
                            AnoLetivo = 2025
                        });

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoCalendarioPorAnoLetivoEModalidadeQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new SME.SGP.Dominio.TipoCalendario { Id = 1 });

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new SME.SGP.Dominio.ParametrosSistema { Ativo = true });

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeriodoEscolarListaPorTipoCalendarioQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new PeriodoEscolarListaDto
                        {
                            Periodos = new List<PeriodoEscolarDto> { new PeriodoEscolarDto { Bimestre = 1, PeriodoInicio = DateTime.Today, PeriodoFim = DateTime.Today.AddMonths(3) } }
                        });

            var result = await useCase.Executar(new long[] { compensacaoId });

            Assert.True(result);
            Assert.True(compensacao.Excluido);
            mediatorMock.Verify(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<DeletarArquivoDeRegistroExcluidoCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Negocio_Exception_Quando_Erro_Ao_Excluir()
        {
            var compensacaoId = 1L;
            var compensacao = new SME.SGP.Dominio.CompensacaoAusencia
            {
                Id = compensacaoId,
                Descricao = "Compensacao Teste",
                TurmaId = 10,
                DisciplinaId = "D1",
                Bimestre = 1
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterCompensacaoAusenciaPorIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(compensacao);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterCompensacaoAusenciaAlunoPorCompensacaoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<CompensacaoAusenciaAluno>());

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new Turma { Id = 10, CodigoTurma = "T1", ModalidadeCodigo = Modalidade.Fundamental, Semestre = 1, AnoLetivo = 2025 });

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoCalendarioPorAnoLetivoEModalidadeQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new SME.SGP.Dominio.TipoCalendario { Id = 1 });

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new SME.SGP.Dominio.ParametrosSistema { Ativo = true });

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeriodoEscolarListaPorTipoCalendarioQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new PeriodoEscolarListaDto
                        {
                            Periodos = new List<PeriodoEscolarDto> { new PeriodoEscolarDto { Bimestre = 1, PeriodoInicio = DateTime.Today, PeriodoFim = DateTime.Today.AddMonths(3) } }
                        });

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaCommand>(), It.IsAny<CancellationToken>()))
                        .ThrowsAsync(new Exception("Erro ao salvar"));

            var exception = await Assert.ThrowsAsync<NegocioException>(async () =>
                await useCase.Executar(new long[] { compensacaoId })
            );

            Assert.Contains(compensacaoId.ToString(), exception.Message);
        }


        [Fact]
        public async Task Executar_Deve_Chamar_Fila_Calcular_Frequencia_Quando_Ha_Alunos()
        {
            var compensacaoId = 1L;
            var aluno = new CompensacaoAusenciaAluno { Id = 1, CompensacaoAusenciaId = compensacaoId, CodigoAluno = "A1" };
            var alunoAula = new CompensacaoAusenciaAlunoAula { CompensacaoAusenciaAlunoId = 1 };
            var compensacao = new SME.SGP.Dominio.CompensacaoAusencia
            {
                Id = compensacaoId,
                TurmaId = 10,
                DisciplinaId = "D1",
                Bimestre = 1,
                Alunos = new List<CompensacaoAusenciaAluno> { aluno }
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterCompensacaoAusenciaPorIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(compensacao);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterCompensacaoAusenciaAlunoPorCompensacaoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<CompensacaoAusenciaAluno> { aluno });
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterCompensacaoAusenciaAlunoAulasPorCompensacaoIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<CompensacaoAusenciaAlunoAula> { alunoAula });
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new Turma { Id = 10, CodigoTurma = "T1", ModalidadeCodigo = Modalidade.Fundamental, Semestre = 1, AnoLetivo = 2025 });
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoCalendarioPorAnoLetivoEModalidadeQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new SME.SGP.Dominio.TipoCalendario { Id = 1 });
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new SME.SGP.Dominio.ParametrosSistema { Ativo = true });
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeriodoEscolarListaPorTipoCalendarioQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new PeriodoEscolarListaDto
                        {
                            Periodos = new List<PeriodoEscolarDto> { new PeriodoEscolarDto { Bimestre = 1, PeriodoInicio = DateTime.Today, PeriodoFim = DateTime.Today.AddMonths(3) } }
                        });

            var result = await useCase.Executar(new long[] { compensacaoId });

            Assert.True(result);
            mediatorMock.Verify(m => m.Send(It.IsAny<IncluirFilaCalcularFrequenciaPorTurmaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
