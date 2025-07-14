using Bogus;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class AtribuicaoCJControllerTeste
    {
        private readonly AtribuicaoCJController _controller;
        private readonly Faker _faker;
        private readonly Mock<IListarAtribuicoesCJPorFiltroUseCase> _listarAtribuicoesUseCase;
        private readonly Mock<ISalvarAtribuicaoCJUseCase> _salvarAtribuicaoUseCase;
        private readonly Mock<IObterProfessoresTitularesECjsUseCase> _obterProfessoresTitularesECjUseCase;
        public AtribuicaoCJControllerTeste()
        {
            _listarAtribuicoesUseCase = new Mock<IListarAtribuicoesCJPorFiltroUseCase>();
            _obterProfessoresTitularesECjUseCase = new Mock<IObterProfessoresTitularesECjsUseCase>();
            _salvarAtribuicaoUseCase = new Mock<ISalvarAtribuicaoCJUseCase>();
            _controller = new AtribuicaoCJController();
            _faker = new Faker("pt_BR");
        }

        #region ListaAtribuicoesCJ

        [Fact(DisplayName = "Deve chamar o caso de uso com lista de atribuições CJ")]
        public async Task ObterAtribuicoesCJ_DeveRetornarOk()
        {
            // Arrange
            var filtro = new AtribuicaoCJListaFiltroDto
            {
                UeId = "ue-001",
                AnoLetivo = 2025,
                UsuarioNome = "Maria Silva",
                UsuarioRf = "1234567",
                Historico = false
            };

            var resultadoEsperado = new List<AtribuicaoCJListaRetornoDto>();

            for (int i = 1; i <= 2; i++)
            {
                resultadoEsperado.Add(new AtribuicaoCJListaRetornoDto
                {
                    Disciplinas = new[] { $"Disciplina {i}A", $"Disciplina {i}B" },
                    DisciplinasId = new[] { 100L + i * 10, 100L + i * 10 + 1 },
                    Modalidade = "Ensino Fundamental",
                    ModalidadeId = 1,
                    Turma = $"5º Ano {Convert.ToChar(64 + i)}",
                    TurmaId = $"turma-00{i}",
                    ProfessorRf = $"00000{i}7"
                });
            }

            _listarAtribuicoesUseCase.Setup(x => x.Executar(It.IsAny<AtribuicaoCJListaFiltroDto>()))
           .ReturnsAsync(resultadoEsperado);

            var resultado = await _controller.Get(filtro, _listarAtribuicoesUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsAssignableFrom<IEnumerable<AtribuicaoCJListaRetornoDto>>(okResult.Value);
            Assert.Equal(2, retorno.ToList().Count);
        }
        #endregion

        #region ListaAnosLetivos

        [Fact(DisplayName = "Deve chamar o caso de uso com lista de anos letivos")]
        public async Task ObterAnosLetivos_DeveRetornarOk()
        {
            // Arrange
            var anosEsperados = new[] { 2023, 2024, 2025 };

            var mockUseCase = new Mock<IObterAnosLetivosAtribuicaoCJUseCase>();
            mockUseCase.Setup(x => x.Executar()).ReturnsAsync(anosEsperados);

            var controller = new AtribuicaoCJController();

            // Act
            var resultado = await controller.ObterAnosLetivosAtribuicao(mockUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var anosRetornados = Assert.IsAssignableFrom<int[]>(okResult.Value);
            Assert.Equal(anosEsperados.Length, anosRetornados.Length);

            for (int i = 0; i < anosEsperados.Length; i++)
                Assert.Equal(anosEsperados[i], anosRetornados[i]);
        }
        #endregion

        #region AtribuicaoDeProfessores

        [Fact(DisplayName = "Deve chamar o caso de uso com atribuição de professores")]
        public async Task ObterAtribuicaoDeProfessores_DeveRetornarOk()
        {
            // Arrange
            var ueId = "ue-123";
            var turmaId = "turma-456";
            var professorRf = "prof789";
            var modalidadeId = Modalidade.Fundamental;
            var anoLetivo = 2025;

            var resultadoEsperado = new AtribuicaoCJTitularesRetornoDto
            {
                CriadoEm = new DateTime(2025, 1, 1),
                CriadoPor = "Admin",
                CriadoRF = "admin123",
                AlteradoEm = new DateTime(2025, 6, 1),
                AlteradoPor = "Editor",
                AlteradoRF = "editor456",
                Itens = new List<AtribuicaoCJTitularesRetornoItemDto>()
            };

            for (int i = 1; i <= 3; i++)
            {
                resultadoEsperado.Itens.Add(new AtribuicaoCJTitularesRetornoItemDto
                {
                    Disciplina = $"Disciplina {i}",
                    ProfessorTitular = $"Professor {i}",
                    ProfessorTitularRf = $"prof{i:000}"
                });
            }

            _obterProfessoresTitularesECjUseCase.Setup(x => x.Executar(ueId, turmaId, professorRf, modalidadeId, anoLetivo))
                       .ReturnsAsync(resultadoEsperado);

            // Act
            var resultado = await _controller.ObterAtribuicaoDeProfessores(ueId, turmaId, professorRf, modalidadeId, anoLetivo, _obterProfessoresTitularesECjUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsType<AtribuicaoCJTitularesRetornoDto>(okResult.Value);

            Assert.Equal(resultadoEsperado.CriadoEm, retorno.CriadoEm);
            Assert.Equal(resultadoEsperado.CriadoPor, retorno.CriadoPor);
            Assert.Equal(resultadoEsperado.AlteradoEm, retorno.AlteradoEm);
            Assert.Equal(resultadoEsperado.AlteradoPor, retorno.AlteradoPor);
            Assert.Equal(resultadoEsperado.Itens.Count, retorno.Itens.Count);

            for (int i = 0; i < retorno.Itens.Count; i++)
            {
                Assert.Equal(resultadoEsperado.Itens[i].Disciplina, retorno.Itens[i].Disciplina);
            }
        }
        #endregion

        #region IncluirAtribuicaoCJ
        [Fact(DisplayName = "Deve chamar o caso de uso para incluir uma atribuição CJ")]
        public async Task DeveChamarCasoDeUso_ParaIncluirAtribuicaoCJ()
        {
            // Arrange
            var dto = CriarDto();
            _salvarAtribuicaoUseCase.Setup(x => x.Executar(dto)).Returns(Task.CompletedTask);

            // Act
            var resultado = await _controller.Post(dto, _salvarAtribuicaoUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkResult>(resultado);
            _salvarAtribuicaoUseCase.Verify(x => x.Executar(dto), Times.Once);
        }
        #endregion

        private AtribuicaoCJPersistenciaDto CriarDto()
        {
            return new AtribuicaoCJPersistenciaDto
            {
                DreId = "dre-123",
                Modalidade = Modalidade.Fundamental,
                TurmaId = "turma-456",
                UeId = "ue-789",
                UsuarioRf = "prof123",
                AnoLetivo = "2025",
                Historico = false,
                Disciplinas = new List<AtribuicaoCJPersistenciaItemDto>
                {
                    new AtribuicaoCJPersistenciaItemDto
                    {
                        DisciplinaId = 101,
                    },
                    new AtribuicaoCJPersistenciaItemDto
                    {
                        DisciplinaId = 102,
                    }
                }
            };
        }
    }
}
