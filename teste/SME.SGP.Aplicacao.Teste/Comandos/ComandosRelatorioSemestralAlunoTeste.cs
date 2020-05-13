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
    public class ComandosRelatorioSemestralAlunoTeste
    {
        private readonly ComandosRelatorioSemestralAluno comandosRelatorioSemestralAluno;
        private readonly Mock<IRepositorioRelatorioSemestralAluno> repositorioRelatorioSemestralAluno;
        private readonly Mock<IComandosRelatorioSemestral> comandosRelatorioSemestral;
        private readonly Mock<IConsultasRelatorioSemestral> consultasRelatorioSemestral;
        private readonly Mock<IComandosRelatorioSemestralAlunoSecao> comandosRelatorioSemestralAlunoSecao;
        private readonly Mock<IConsultasTurma> consultasTurma;
        private readonly Mock<IUnitOfWork> unitOfWork;

        public ComandosRelatorioSemestralAlunoTeste()
        {
            repositorioRelatorioSemestralAluno = new Mock<IRepositorioRelatorioSemestralAluno>();
            comandosRelatorioSemestral = new Mock<IComandosRelatorioSemestral>();
            consultasRelatorioSemestral = new Mock<IConsultasRelatorioSemestral>();
            comandosRelatorioSemestralAlunoSecao = new Mock<IComandosRelatorioSemestralAlunoSecao>();
            consultasTurma = new Mock<IConsultasTurma>();
            unitOfWork = new Mock<IUnitOfWork>();

            comandosRelatorioSemestralAluno = new ComandosRelatorioSemestralAluno(repositorioRelatorioSemestralAluno.Object,
                                                                                  comandosRelatorioSemestral.Object,
                                                                                  consultasRelatorioSemestral.Object,
                                                                                  comandosRelatorioSemestralAlunoSecao.Object,
                                                                                  consultasTurma.Object,
                                                                                  unitOfWork.Object);
        }

        [Fact]
        public async void Deve_Incluir_Relatorio()
        {
            //ARRANGE
            var dto = new RelatorioSemestralAlunoPersistenciaDto()
            {
                RelatorioSemestralAlunoId = 0,
                RelatorioSemestralId = 0,
                Secoes = new List<RelatorioSemestralAlunoSecaoDto>()
                {
                    new RelatorioSemestralAlunoSecaoDto(1, "", "", true, "Teste 1"),
                    new RelatorioSemestralAlunoSecaoDto(2, "", "", true, "Teste 2"),
                    new RelatorioSemestralAlunoSecaoDto(3, "", "", true, "Teste 3"),
                    new RelatorioSemestralAlunoSecaoDto(4, "", "", true, "Teste 4"),
                    new RelatorioSemestralAlunoSecaoDto(5, "", "", false, "Teste 5"),
                }
            };

            consultasTurma.Setup(a => a.ObterPorCodigo(It.IsAny<string>()))
                .Returns(Task.FromResult<Turma>(new Turma() { Id = 1 }));

            //ACT
            var auditoria = await comandosRelatorioSemestralAluno.Salvar("123", "321", 1, dto);

            //ASSERT
            Assert.NotNull(auditoria);

            comandosRelatorioSemestral.Verify(a => a.SalvarAsync(It.IsAny<RelatorioSemestral>()), Times.Once);
            repositorioRelatorioSemestralAluno.Verify(a => a.SalvarAsync(It.IsAny<RelatorioSemestralAluno>()), Times.Once);
            comandosRelatorioSemestralAlunoSecao.Verify(a => a.SalvarAsync(It.IsAny<RelatorioSemestralAlunoSecao>()), Times.Exactly(5));
            unitOfWork.Verify(a => a.PersistirTransacao(), Times.Once);
        }

        [Fact]
        public async void Deve_Incluir_Relatorio_Com_Relatorio_Turma()
        {
            //ARRANGE
            var dto = new RelatorioSemestralAlunoPersistenciaDto()
            {
                RelatorioSemestralAlunoId = 0,
                RelatorioSemestralId = 1,
                Secoes = new List<RelatorioSemestralAlunoSecaoDto>()
                {
                    new RelatorioSemestralAlunoSecaoDto(1, "", "", true, "Teste alteracao 1"),
                    new RelatorioSemestralAlunoSecaoDto(2, "", "", true, "Teste alteracao 2"),
                    new RelatorioSemestralAlunoSecaoDto(3, "", "", true, "Teste alteracao 3"),
                    new RelatorioSemestralAlunoSecaoDto(4, "", "", true, "Teste alteracao 4"),
                    new RelatorioSemestralAlunoSecaoDto(5, "", "", false, "Teste alteracao 5"),
                }
            };

            consultasRelatorioSemestral.Setup(c => c.ObterPorIdAsync(It.IsAny<long>()))
                .Returns(Task.FromResult(new RelatorioSemestral() { Id = 1, Semestre = 1, TurmaId = 1 }));

            //ACT
            var auditoria = await comandosRelatorioSemestralAluno.Salvar("123", "321", 1, dto);

            //ASSERT
            Assert.NotNull(auditoria);

            repositorioRelatorioSemestralAluno.Verify(a => a.SalvarAsync(It.IsAny<RelatorioSemestralAluno>()), Times.Once);
            comandosRelatorioSemestralAlunoSecao.Verify(a => a.SalvarAsync(It.IsAny<RelatorioSemestralAlunoSecao>()), Times.Exactly(5));
            unitOfWork.Verify(a => a.PersistirTransacao(), Times.Once);
        }

        [Fact]
        public async void Deve_Alterar_Relatorio()
        {
            //ARRANGE
            var dto = new RelatorioSemestralAlunoPersistenciaDto()
            {
                RelatorioSemestralAlunoId = 1,
                RelatorioSemestralId = 1,
                Secoes = new List<RelatorioSemestralAlunoSecaoDto>()
                {
                    new RelatorioSemestralAlunoSecaoDto(1, "", "", true, "Teste alteracao 1"),
                    new RelatorioSemestralAlunoSecaoDto(2, "", "", true, "Teste alteracao 2"),
                    new RelatorioSemestralAlunoSecaoDto(3, "", "", true, "Teste alteracao 3"),
                    new RelatorioSemestralAlunoSecaoDto(4, "", "", true, "Teste alteracao 4"),
                    new RelatorioSemestralAlunoSecaoDto(5, "", "", false, "Teste alteracao 5"),
                }
            };

            repositorioRelatorioSemestralAluno.Setup(a => a.ObterCompletoPorIdAsync(It.IsAny<long>()))
                .Returns(Task.FromResult(new RelatorioSemestralAluno()
                {
                    Id = 1,
                    RelatorioSemestralId = 1,
                    Secoes = new List<RelatorioSemestralAlunoSecao>()
                    {
                        new RelatorioSemestralAlunoSecao() { SecaoRelatorioSemestralId = 1, Valor = "Teste 1"},
                        new RelatorioSemestralAlunoSecao() { SecaoRelatorioSemestralId = 2, Valor = "Teste 2"},
                        new RelatorioSemestralAlunoSecao() { SecaoRelatorioSemestralId = 3, Valor = "Teste 3"},
                        new RelatorioSemestralAlunoSecao() { SecaoRelatorioSemestralId = 4, Valor = "Teste 4"},
                        new RelatorioSemestralAlunoSecao() { SecaoRelatorioSemestralId = 5, Valor = "Teste 5"},
                    }
                }));

            //ACT
            var auditoria = await comandosRelatorioSemestralAluno.Salvar("123", "321", 1, dto);

            //ASSERT
            Assert.NotNull(auditoria);

            repositorioRelatorioSemestralAluno.Verify(a => a.SalvarAsync(It.IsAny<RelatorioSemestralAluno>()), Times.Once);
            comandosRelatorioSemestralAlunoSecao.Verify(a => a.SalvarAsync(It.IsAny<RelatorioSemestralAlunoSecao>()), Times.Exactly(5));
            unitOfWork.Verify(a => a.PersistirTransacao(), Times.Once);
        }
    }
}
