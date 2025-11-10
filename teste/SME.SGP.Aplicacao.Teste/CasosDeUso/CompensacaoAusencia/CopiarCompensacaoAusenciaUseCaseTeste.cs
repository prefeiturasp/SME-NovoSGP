using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.CompensacaoAusencia
{
    public class CopiarCompensacaoAusenciaUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IRepositorioCompensacaoAusencia> repoCompensacaoMock;
        private readonly Mock<IRepositorioCompensacaoAusenciaDisciplinaRegencia> repoDiscRegenciaMock;
        private readonly Mock<ISalvarCompensacaoAusenciaUseCase> salvarUseCaseMock;

        public CopiarCompensacaoAusenciaUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            repoCompensacaoMock = new Mock<IRepositorioCompensacaoAusencia>();
            repoDiscRegenciaMock = new Mock<IRepositorioCompensacaoAusenciaDisciplinaRegencia>();
            salvarUseCaseMock = new Mock<ISalvarCompensacaoAusenciaUseCase>();
        }

        private CopiarCompensacaoAusenciaUseCase CriarUseCase()
        {
            return new CopiarCompensacaoAusenciaUseCase(
                mediatorMock.Object,
                repoCompensacaoMock.Object,
                repoDiscRegenciaMock.Object,
                salvarUseCaseMock.Object);
        }

        [Fact]
        public async Task Executar_Compensacao_Origem_Nao_Encontrada_Deve_Lancar_Negocio_Exception()
        {
            repoCompensacaoMock.Setup(r => r.ObterPorId(It.IsAny<long>())).Returns((SME.SGP.Dominio.CompensacaoAusencia)null);

            var useCase = CriarUseCase();
            var dto = new CompensacaoAusenciaCopiaDto
            {
                CompensacaoOrigemId = 123,
                TurmasIds = new List<string> { "T1" },
                Bimestre = 1
            };

            var ex = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));
            Assert.Contains("Compensação de origem não localizada", ex.Message);
        }

        [Fact]
        public async Task Executar_Sem_Turmas_Retorna_String_Vazia()
        {
            var origem = new SME.SGP.Dominio.CompensacaoAusencia { Id = 1, Nome = "Origem", DisciplinaId = "D1" };
            repoCompensacaoMock.Setup(r => r.ObterPorId(origem.Id)).Returns(origem);

            var useCase = CriarUseCase();
            var dto = new CompensacaoAusenciaCopiaDto
            {
                CompensacaoOrigemId = origem.Id,
                TurmasIds = new List<string>(), 
                Bimestre = 1
            };

            var resultado = await useCase.Executar(dto);

            Assert.Equal(string.Empty, resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<IRequest<Turma>>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Turma_Unica_Copia_Com_Sucesso_Retorna_Mensagem_Singular()
        {
            var origem = new SME.SGP.Dominio.CompensacaoAusencia { Id = 1, Nome = "Origem", DisciplinaId = "D1", Descricao = "desc" };
            repoCompensacaoMock.Setup(r => r.ObterPorId(origem.Id)).Returns(origem);

            var turma = new Turma { CodigoTurma = "T1", Nome = "Turma A" };
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(turma);

            repoDiscRegenciaMock.Setup(r => r.ObterPorCompensacao(origem.Id)).ReturnsAsync(Enumerable.Empty<CompensacaoAusenciaDisciplinaRegencia>());

            salvarUseCaseMock.Setup(s => s.Executar(It.IsAny<int>(), It.IsAny<CompensacaoAusenciaDto>())).Returns(Task.CompletedTask);

            var useCase = CriarUseCase();
            var dto = new CompensacaoAusenciaCopiaDto
            {
                CompensacaoOrigemId = origem.Id,
                TurmasIds = new[] { "T1" },
                Bimestre = 2
            };

            var resultado = await useCase.Executar(dto);

            Assert.Equal("A cópia para a turma Turma A foi realizada com sucesso", resultado);

            salvarUseCaseMock.Verify(s => s.Executar(0, It.Is<CompensacaoAusenciaDto>(
                c => c.TurmaId == "T1" &&
                     c.Bimestre == 2 &&
                     c.DisciplinaId == origem.DisciplinaId &&
                     c.Atividade == origem.Nome
            )), Times.Once);
        }

        [Fact]
        public async Task Executar_Multiplas_Turmas_Copia_Com_Sucesso_Retorna_Mensagem_Plural()
        {
            var origem = new SME.SGP.Dominio.CompensacaoAusencia { Id = 1, Nome = "Origem", DisciplinaId = "D1" };
            repoCompensacaoMock.Setup(r => r.ObterPorId(origem.Id)).Returns(origem);

            var turmaA = new Turma { CodigoTurma = "T1", Nome = "Turma A" };
            var turmaB = new Turma { CodigoTurma = "T2", Nome = "Turma B" };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((ObterTurmaPorCodigoQuery q, CancellationToken ct) =>
                        {
                            return q.TurmaCodigo == "T1" ? turmaA : turmaB;
                        });

            repoDiscRegenciaMock.Setup(r => r.ObterPorCompensacao(origem.Id)).ReturnsAsync(Enumerable.Empty<CompensacaoAusenciaDisciplinaRegencia>());
            salvarUseCaseMock.Setup(s => s.Executar(It.IsAny<int>(), It.IsAny<CompensacaoAusenciaDto>())).Returns(Task.CompletedTask);

            var useCase = CriarUseCase();
            var dto = new CompensacaoAusenciaCopiaDto
            {
                CompensacaoOrigemId = origem.Id,
                TurmasIds = new[] { "T1", "T2" },
                Bimestre = 1
            };

            var resultado = await useCase.Executar(dto);

            Assert.Equal("A cópia para as turmas Turma A, Turma B foi realizada com sucesso", resultado);
            salvarUseCaseMock.Verify(s => s.Executar(0, It.IsAny<CompensacaoAusenciaDto>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Executar_Parcialmente_Com_Erro_Deve_Lancar_Negocio_Exception_Com_Resumo_DeErro_E_Sucesso()
        {
            var origem = new SME.SGP.Dominio.CompensacaoAusencia { Id = 99, Nome = "Origem", DisciplinaId = "D1" };
            repoCompensacaoMock.Setup(r => r.ObterPorId(origem.Id)).Returns(origem);

            var turmaOk = new Turma { CodigoTurma = "T_OK", Nome = "Turma OK" };
            var turmaErro = new Turma { CodigoTurma = "T_ERR", Nome = "Turma ERRO" };

            mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaPorCodigoQuery>(q => q.TurmaCodigo == "T_OK"), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(turmaOk);
            mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaPorCodigoQuery>(q => q.TurmaCodigo == "T_ERR"), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(turmaErro);

            salvarUseCaseMock
                .Setup(s => s.Executar(It.IsAny<long>(), It.Is<CompensacaoAusenciaDto>(d => d.TurmaId == "T_OK")))
                .Returns(Task.CompletedTask);

            salvarUseCaseMock
                .Setup(s => s.Executar(It.IsAny<long>(), It.Is<CompensacaoAusenciaDto>(d => d.TurmaId == "T_ERR")))
                .ThrowsAsync(new Exception("Erro simulado na turma ERRO"));

            var useCase = CriarUseCase();
            var dto = new CompensacaoAusenciaCopiaDto
            {
                CompensacaoOrigemId = origem.Id,
                TurmasIds = new[] { "T_OK", "T_ERR" },
                Bimestre = 3
            };

            var ex = await Assert.ThrowsAsync<SME.SGP.Dominio.NegocioException>(() => useCase.Executar(dto));

            Assert.Contains("Turma OK", ex.Message);   
            Assert.Contains("Turma ERRO", ex.Message);  
        }

        [Fact]
        public async Task Executar_Disciplinas_Regencia_Sao_Repasadas_Para_Salvar()
        {
            var origem = new SME.SGP.Dominio.CompensacaoAusencia { Id = 77, Nome = "Origem", DisciplinaId = "D1" };
            repoCompensacaoMock.Setup(r => r.ObterPorId(origem.Id)).Returns(origem);

            var turma = new Turma { CodigoTurma = "T77", Nome = "Turma 77" };
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(turma);

            var regencia = new CompensacaoAusenciaDisciplinaRegencia
            {
                CompensacaoAusenciaId = origem.Id,
                DisciplinaId = "Reg1"
            };
            repoDiscRegenciaMock.Setup(r => r.ObterPorCompensacao(origem.Id))
                                .ReturnsAsync(new[] { regencia });

            CompensacaoAusenciaDto capturedDto = null;
            salvarUseCaseMock
                .Setup(s => s.Executar(It.IsAny<long>(), It.IsAny<CompensacaoAusenciaDto>()))
                .Callback<long, CompensacaoAusenciaDto>((i, dto) => capturedDto = dto)
                .Returns(Task.CompletedTask);

            var useCase = CriarUseCase();
            var dto = new CompensacaoAusenciaCopiaDto
            {
                CompensacaoOrigemId = origem.Id,
                TurmasIds = new[] { "T77" },
                Bimestre = 3
            };

            var resultado = await useCase.Executar(dto);

            Assert.Equal("A cópia para a turma Turma 77 foi realizada com sucesso", resultado);
            Assert.NotNull(capturedDto);
            Assert.NotNull(capturedDto.DisciplinasRegenciaIds);
            Assert.Contains("Reg1", capturedDto.DisciplinasRegenciaIds);
        }
    }
}
