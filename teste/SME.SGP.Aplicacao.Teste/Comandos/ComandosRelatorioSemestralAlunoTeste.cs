using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Comandos
{
    //public class ComandosRelatorioSemestralAlunoTeste
    //{
    //    private readonly ComandosRelatorioSemestralPAPAluno comandosRelatorioSemestralAluno;
    //    private readonly Mock<IRepositorioRelatorioSemestralPAPAluno> repositorioRelatorioSemestralAluno;
    //    private readonly Mock<IRepositorioPeriodoEscolar> repositorioPeriodoEscolar;
    //    private readonly Mock<IComandosRelatorioSemestralTurmaPAP> comandosRelatorioSemestral;
    //    private readonly Mock<IConsultasRelatorioSemestralTurmaPAP> consultasRelatorioSemestral;
    //    private readonly Mock<IComandosRelatorioSemestralPAPAlunoSecao> comandosRelatorioSemestralAlunoSecao;
    //    private readonly Mock<IConsultasTurma> consultasTurma;
    //    private readonly Mock<IUnitOfWork> unitOfWork;

    //    public ComandosRelatorioSemestralAlunoTeste()
    //    {
    //        repositorioRelatorioSemestralAluno = new Mock<IRepositorioRelatorioSemestralPAPAluno>();
    //        repositorioPeriodoEscolar = new Mock<IRepositorioPeriodoEscolar>();
    //        comandosRelatorioSemestral = new Mock<IComandosRelatorioSemestralTurmaPAP>();
    //        consultasRelatorioSemestral = new Mock<IConsultasRelatorioSemestralTurmaPAP>();
    //        comandosRelatorioSemestralAlunoSecao = new Mock<IComandosRelatorioSemestralPAPAlunoSecao>();
    //        consultasTurma = new Mock<IConsultasTurma>();
    //        unitOfWork = new Mock<IUnitOfWork>();

    //        comandosRelatorioSemestralAluno = new ComandosRelatorioSemestralPAPAluno(repositorioRelatorioSemestralAluno.Object,
    //                                                                              repositorioPeriodoEscolar.Object,
    //                                                                              comandosRelatorioSemestral.Object,
    //                                                                              consultasRelatorioSemestral.Object,
    //                                                                              comandosRelatorioSemestralAlunoSecao.Object,
    //                                                                              consultasTurma.Object,
    //                                                                              unitOfWork.Object);
    //    }

    //    //[Fact]
    //    //public async void Deve_Incluir_Relatorio()
    //    //{
    //    //    //ARRANGE
    //    //    var dto = new RelatorioSemestralAlunoPersistenciaDto()
    //    //    {
    //    //        RelatorioSemestralAlunoId = 0,
    //    //        RelatorioSemestralId = 0,
    //    //        Secoes = new List<RelatorioSemestralAlunoSecaoDto>()
    //    //        {
    //    //            new RelatorioSemestralAlunoSecaoDto(1, "", "", true, "Teste 1"),
    //    //            new RelatorioSemestralAlunoSecaoDto(2, "", "", true, "Teste 2"),
    //    //            new RelatorioSemestralAlunoSecaoDto(3, "", "", true, "Teste 3"),
    //    //            new RelatorioSemestralAlunoSecaoDto(4, "", "", true, "Teste 4"),
    //    //            new RelatorioSemestralAlunoSecaoDto(5, "", "", false, "Teste 5"),
    //    //        }
    //    //    };

    //    //    consultasTurma.Setup(a => a.ObterPorCodigo(It.IsAny<string>()))
    //    //        .Returns(Task.FromResult<Turma>(new Turma() { Id = 1 }));

    //    //    //ACT
    //    //    var auditoria = await comandosRelatorioSemestralAluno.Salvar("123", "321", 1, dto);

    //    //    //ASSERT
    //    //    Assert.NotNull(auditoria);

    //    //    comandosRelatorioSemestral.Verify(a => a.SalvarAsync(It.IsAny<RelatorioSemestralTurmaPAP>()), Times.Once);
    //    //    repositorioRelatorioSemestralAluno.Verify(a => a.SalvarAsync(It.IsAny<RelatorioSemestralPAPAluno>()), Times.Once);
    //    //    comandosRelatorioSemestralAlunoSecao.Verify(a => a.SalvarAsync(It.IsAny<RelatorioSemestralPAPAlunoSecao>()), Times.Exactly(5));
    //    //    unitOfWork.Verify(a => a.PersistirTransacao(), Times.Once);
    //    //}

    //    //[Fact]
    //    //public async void Deve_Incluir_Relatorio_Com_Relatorio_Turma()
    //    //{
    //    //    //ARRANGE
    //    //    var dto = new RelatorioSemestralAlunoPersistenciaDto()
    //    //    {
    //    //        RelatorioSemestralAlunoId = 0,
    //    //        RelatorioSemestralId = 1,
    //    //        Secoes = new List<RelatorioSemestralAlunoSecaoDto>()
    //    //        {
    //    //            new RelatorioSemestralAlunoSecaoDto(1, "", "", true, "Teste alteracao 1"),
    //    //            new RelatorioSemestralAlunoSecaoDto(2, "", "", true, "Teste alteracao 2"),
    //    //            new RelatorioSemestralAlunoSecaoDto(3, "", "", true, "Teste alteracao 3"),
    //    //            new RelatorioSemestralAlunoSecaoDto(4, "", "", true, "Teste alteracao 4"),
    //    //            new RelatorioSemestralAlunoSecaoDto(5, "", "", false, "Teste alteracao 5"),
    //    //        }
    //    //    };

    //    //    consultasRelatorioSemestral.Setup(c => c.ObterPorIdAsync(It.IsAny<long>()))
    //    //        .Returns(Task.FromResult(new RelatorioSemestralTurmaPAP() { Id = 1, Semestre = 1, TurmaId = 1 }));

    //    //    //ACT
    //    //    var auditoria = await comandosRelatorioSemestralAluno.Salvar("123", "321", 1, dto);

    //    //    //ASSERT
    //    //    Assert.NotNull(auditoria);

    //    //    repositorioRelatorioSemestralAluno.Verify(a => a.SalvarAsync(It.IsAny<RelatorioSemestralPAPAluno>()), Times.Once);
    //    //    comandosRelatorioSemestralAlunoSecao.Verify(a => a.SalvarAsync(It.IsAny<RelatorioSemestralPAPAlunoSecao>()), Times.Exactly(5));
    //    //    unitOfWork.Verify(a => a.PersistirTransacao(), Times.Once);
    //    //}

    //    //[Fact]
    //    //public async void Deve_Alterar_Relatorio()
    //    //{
    //    //    //ARRANGE
    //    //    var dto = new RelatorioSemestralAlunoPersistenciaDto()
    //    //    {
    //    //        RelatorioSemestralAlunoId = 1,
    //    //        RelatorioSemestralId = 1,
    //    //        Secoes = new List<RelatorioSemestralAlunoSecaoDto>()
    //    //        {
    //    //            new RelatorioSemestralAlunoSecaoDto(1, "", "", true, "Teste alteracao 1"),
    //    //            new RelatorioSemestralAlunoSecaoDto(2, "", "", true, "Teste alteracao 2"),
    //    //            new RelatorioSemestralAlunoSecaoDto(3, "", "", true, "Teste alteracao 3"),
    //    //            new RelatorioSemestralAlunoSecaoDto(4, "", "", true, "Teste alteracao 4"),
    //    //            new RelatorioSemestralAlunoSecaoDto(5, "", "", false, "Teste alteracao 5"),
    //    //        }
    //    //    };

    //    //    repositorioRelatorioSemestralAluno.Setup(a => a.ObterCompletoPorIdAsync(It.IsAny<long>()))
    //    //        .Returns(Task.FromResult(new RelatorioSemestralPAPAluno()
    //    //        {
    //    //            Id = 1,
    //    //            RelatorioSemestralTurmaPAPId = 1,
    //    //            Secoes = new List<RelatorioSemestralPAPAlunoSecao>()
    //    //            {
    //    //                new RelatorioSemestralPAPAlunoSecao() { SecaoRelatorioSemestralPAPId = 1, Valor = "Teste 1"},
    //    //                new RelatorioSemestralPAPAlunoSecao() { SecaoRelatorioSemestralPAPId = 2, Valor = "Teste 2"},
    //    //                new RelatorioSemestralPAPAlunoSecao() { SecaoRelatorioSemestralPAPId = 3, Valor = "Teste 3"},
    //    //                new RelatorioSemestralPAPAlunoSecao() { SecaoRelatorioSemestralPAPId = 4, Valor = "Teste 4"},
    //    //                new RelatorioSemestralPAPAlunoSecao() { SecaoRelatorioSemestralPAPId = 5, Valor = "Teste 5"},
    //    //            }
    //    //        }));

    //    //    //ACT
    //    //    var auditoria = await comandosRelatorioSemestralAluno.Salvar("123", "321", 1, dto);

    //    //    //ASSERT
    //    //    Assert.NotNull(auditoria);

    //    //    repositorioRelatorioSemestralAluno.Verify(a => a.SalvarAsync(It.IsAny<RelatorioSemestralPAPAluno>()), Times.Once);
    //    //    comandosRelatorioSemestralAlunoSecao.Verify(a => a.SalvarAsync(It.IsAny<RelatorioSemestralPAPAlunoSecao>()), Times.Exactly(5));
    //    //    unitOfWork.Verify(a => a.PersistirTransacao(), Times.Once);
    //    //}
    //}
}
