using Bogus;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Nest;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class AtividadeAvaliativaControllerTeste
    {
        private readonly Mock<IComandosAtividadeAvaliativa> _comandosAtividadeAvaliativaMock;
        private readonly Mock<IConsultaAtividadeAvaliativa> _consultaAtividadeAvaliativaMock;
        private readonly AtividadeAvaliativaController _controller;
        private readonly Faker _faker;

        public AtividadeAvaliativaControllerTeste()
        {
            _comandosAtividadeAvaliativaMock = new Mock<IComandosAtividadeAvaliativa>();
            _consultaAtividadeAvaliativaMock = new Mock<IConsultaAtividadeAvaliativa>();
            _controller = new AtividadeAvaliativaController(_comandosAtividadeAvaliativaMock.Object, _consultaAtividadeAvaliativaMock.Object);
            _faker = new Faker("pt_BR");
        }

        # region AlterarAtividadeAvaliativa

        [Fact(DisplayName = "Deve chamar o caso de uso ao alterar atividade avaliativa com sucesso")]
        public async Task DeveChamarCasoDeUso_QuandoAlterarAtividadeAvaliativa()
        {
            // Arrange
            var id = _faker.Random.Long(1);
            var dto = CriarDto();

            _comandosAtividadeAvaliativaMock
                .Setup(x => x.Alterar(dto, id))
                .ReturnsAsync(new List<RetornoCopiarAtividadeAvaliativaDto>
                {
                    new RetornoCopiarAtividadeAvaliativaDto("Atividade Avaliativa alterada com sucesso", true)
                });

            // Act
            var resultado = await _controller.Alterar(dto, id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);

            var retorno = Assert.IsAssignableFrom<IEnumerable<RetornoCopiarAtividadeAvaliativaDto>>(okResult.Value);
            Assert.Contains(retorno, r => r.Mensagem.Contains("Atividade Avaliativa alterada com sucesso"));
        }

        #endregion

        #region ExcluirAitivdadeAvaliativa

        [Fact(DisplayName = "Deve chamar o caso de uso para excluir uma atividade avaliativa")]
        public async Task DeveChamarCasoDeUso_QuandoExcluirAtividadeAvaliativa()
        {
            // Arrange
            var id = _faker.Random.Long(1);

            _comandosAtividadeAvaliativaMock
                .Setup(x => x.Excluir(id))
                .Returns(Task.CompletedTask);

            // Act
            var resultado = await _controller.Excluir(id);

            // Assert
            _comandosAtividadeAvaliativaMock.Verify(x => x.Excluir(id), Times.Once);

            var okResult = Assert.IsType<OkResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
        }
        #endregion

        #region ListaPaginadaAtividadeAvaliativa

        [Fact(DisplayName = "Deve chamar o caso de uso para buscar atividades avaliativas de forma paginada")]
        public async Task DeveChamarCasoDeUso_ParaBuscarPaginada()
        {
            // Arrange
            var filtro = FiltroDto();

            var atividadeCompleta = new AtividadeAvaliativaCompletaDto
            {
                Id = 1,
                Nome = "Atividade Teste",
                DataAvaliacao = DateTime.Today,
                AlteradoEm = DateTime.Now.AddDays(-1),
                AlteradoPor = "Prof. Alterador",
                AlteradoRF = "RF123",
                Categoria = "Simulados",
                CriadoEm = DateTime.Now.AddDays(-10),
                CriadoPor = "Prof. Criador",
                CriadoRF = "RF321",
                DentroPeriodo = true,
                Importado = false,
                PodeEditarAvaliacao = true,
                AtividadesRegencia = new List<AtividadeAvaliativaRegenciaDto>
                {
                    new AtividadeAvaliativaRegenciaDto { }
                },
                CategoriaId = SME.SGP.Dominio.CategoriaAtividadeAvaliativa.Normal,
                Descricao = "Descrição teste",
                DreId = filtro.DreId,
                DisciplinasId = filtro.DisciplinasId,
                TurmaId = filtro.TurmaId,
                UeId = filtro.UeID,
                TipoAvaliacaoId = filtro.TipoAvaliacaoId ?? 0,
                EhRegencia = false,
            };

            var resultadoEsperado = new PaginacaoResultadoDto<AtividadeAvaliativaCompletaDto>
            {
                Items = new List<AtividadeAvaliativaCompletaDto> { atividadeCompleta },
                TotalPaginas = 1,
                TotalRegistros = 1
            };

            _consultaAtividadeAvaliativaMock
                .Setup(x => x.ListarPaginado(It.IsAny<FiltroAtividadeAvaliativaDto>()))
                .ReturnsAsync(resultadoEsperado);

            // Act
            var resultado = await _controller.Listar(filtro);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);

            var retorno = Assert.IsType<PaginacaoResultadoDto<AtividadeAvaliativaCompletaDto>>(okResult.Value);
            Assert.NotNull(retorno.Items);
            Assert.Single(retorno.Items);

            var item = retorno.Items.GetEnumerator();
            item.MoveNext();
            var primeiroItem = item.Current;

            Assert.Equal(atividadeCompleta.Id, primeiroItem.Id);
            Assert.Equal(atividadeCompleta.Nome, primeiroItem.Nome);
            Assert.Equal(atividadeCompleta.AlteradoPor, primeiroItem.AlteradoPor);
            Assert.Equal(atividadeCompleta.Categoria, primeiroItem.Categoria);
            Assert.True(primeiroItem.DentroPeriodo);
            Assert.True(primeiroItem.PodeEditarAvaliacao);

            Assert.Equal(resultadoEsperado.TotalPaginas, retorno.TotalPaginas);
            Assert.Equal(resultadoEsperado.TotalRegistros, retorno.TotalRegistros);
        }
        #endregion

        #region BuscarAtividadeAvaliativaPorID

        [Fact(DisplayName = "Deve chamar o caso de uso para buscar uma atividade avaliativa por ID")]
        public async Task DeveChamarCasoDeUso_ParaBuscarPorId()
        {
            // Arrange
            var id = _faker.Random.Long(1);

            var atividadeCompleta = new AtividadeAvaliativaCompletaDto
            {
                Id = id,
                Nome = "Atividade Teste",
                DataAvaliacao = DateTime.Today,
            };

            _consultaAtividadeAvaliativaMock
                .Setup(x => x.ObterPorIdAsync(id))
                .ReturnsAsync(atividadeCompleta);

            // Act
            var resultado = await _controller.ObterPorIdAsync(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);

            var retorno = Assert.IsType<AtividadeAvaliativaCompletaDto>(okResult.Value);
            Assert.Equal(id, retorno.Id);
        }
        #endregion

        #region IncluirAtividadeAvaliativa

        [Fact(DisplayName = "Deve chamar o caso de uso para incluir uma atividade avaliativa")]
        public async Task DeveChamarCasoDeUso_ParaIncluirAtividadeAvaliativa()
        {
            var dto = CriarDto();

            _comandosAtividadeAvaliativaMock
                .Setup(x => x.Inserir(dto))
                .ReturnsAsync(new List<RetornoCopiarAtividadeAvaliativaDto>
                {
                    new RetornoCopiarAtividadeAvaliativaDto("Atividade Avaliativa criada com sucesso", true)
                });

            // Act
            var resultado = await _controller.PostAsync(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);

            var retorno = Assert.IsAssignableFrom<IEnumerable<RetornoCopiarAtividadeAvaliativaDto>>(okResult.Value);
            Assert.Contains(retorno, r => r.Mensagem.Contains("Atividade Avaliativa criada com sucesso"));
        }
        #endregion

        #region ValidarAtividadeAvaliativa

        [Fact(DisplayName = "Deve chamar o caso de uso para validar atividade avaliativa e retornar Ok")]
        public async Task DeveChamarCasoDeUso_ValidarAtividadeAvaliativa()
        {
            var filtro = FiltroDto();

            _comandosAtividadeAvaliativaMock
                .Setup(x => x.Validar(filtro))
                .Returns(Task.CompletedTask);

            var result = await _controller.Validar(filtro);

            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            _comandosAtividadeAvaliativaMock.Verify(x => x.Validar(filtro), Times.Once);
        }
        #endregion

        #region ValidarAtividadeAvaliativaExistente

        [Fact(DisplayName = "Deve chamar o caso de uso para validar atividade avaliativa existente e retornar Ok")]
        public async Task DeveChamarCasoDeUso_ValidarAtividadeAvaliativaExistente()
        {
            // Arrange
            var filtro = new FiltroAtividadeAvaliativaExistenteDto
            {
                AtividadeAvaliativaTurmaDatas = new List<AtividadeAvaliativaTurmaDataDto>
                {
                    new AtividadeAvaliativaTurmaDataDto
                    {
                        TurmaId = 1,
                        DataAvaliacao = DateTime.Today,
                        DisciplinasId = new[] { "1" }
                    }
                }
            };

            var resultadoEsperado = new List<AtividadeAvaliativaExistenteRetornoDto>
            {
                new AtividadeAvaliativaExistenteRetornoDto
                {
                    TurmaId = 1,
                    Mensagem = "Teste",
                    Erro = false
                }
            };

            _consultaAtividadeAvaliativaMock
                .Setup(x => x.ValidarAtividadeAvaliativaExistente(filtro))
                .ReturnsAsync(resultadoEsperado);

            // Act
            var result = await _controller.ValidarAtividadeAvaliativaExistente(filtro);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var retorno = Assert.IsAssignableFrom<IEnumerable<AtividadeAvaliativaExistenteRetornoDto>>(okResult.Value);
            Assert.Equal(resultadoEsperado, retorno);

            _consultaAtividadeAvaliativaMock.Verify(x => x.ValidarAtividadeAvaliativaExistente(filtro), Times.Once);
        }
        #endregion

        #region ListaTurmasCopia

        [Fact(DisplayName = "Deve chamar o caso de uso e retornar turmas para cópia")]
        public async Task DeveRetornarListaDeTurmas_ObterTurmasCopia()
        {
            // Arrange
            var codigoTurma = "turma01";
            var disciplinaId = "disc01";

            var turmasEsperadas = new List<TurmaRetornoDto>
            {
                new TurmaRetornoDto { Codigo = "turma02", Nome = "Turma 02" },
                new TurmaRetornoDto { Codigo = "turma03", Nome = "Turma 03" }
            };

            _consultaAtividadeAvaliativaMock
                .Setup(x => x.ObterTurmasCopia(codigoTurma, disciplinaId))
                .ReturnsAsync(turmasEsperadas);

            // Act
            var resultado = await _controller.ObterTurmasCopia(codigoTurma, disciplinaId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);

            var turmasRetornadas = Assert.IsAssignableFrom<IEnumerable<TurmaRetornoDto>>(okResult.Value);
            Assert.Equal(turmasEsperadas.Count, turmasRetornadas.Count());
            Assert.Collection(turmasRetornadas,
                turma => Assert.Equal("turma02", turma.Codigo),
                turma => Assert.Equal("turma03", turma.Codigo));

            _consultaAtividadeAvaliativaMock.Verify(x => x.ObterTurmasCopia(codigoTurma, disciplinaId), Times.Once);
        }
        #endregion

        private AtividadeAvaliativaDto CriarDto()
        {
            return new AtividadeAvaliativaDto
            {
                CategoriaId = SME.SGP.Dominio.CategoriaAtividadeAvaliativa.Normal,
                DataAvaliacao = DateTime.Today,
                Nome = _faker.Lorem.Sentence(),
                Descricao = _faker.Lorem.Paragraph(),
                DreId = _faker.Random.String2(15),
                DisciplinasId = new[] { _faker.Random.Guid().ToString() },
                TurmaId = _faker.Random.Guid().ToString(),
                UeId = _faker.Random.String2(15),
                TipoAvaliacaoId = _faker.Random.Long(1),
                EhRegencia = false
            };
        }

        private FiltroAtividadeAvaliativaDto FiltroDto()
        {
            return new FiltroAtividadeAvaliativaDto
            {
                DataAvaliacao = DateTime.Today,
                DisciplinaContidaRegenciaId = new[] { "regencia1", "regencia2" },
                DisciplinasId = new[] { "disciplina1", "disciplina2" },
                DreId = "DRE123",
                Id = 1,
                Nome = "Atividade Teste",
                TipoAvaliacaoId = 2,
                TurmaId = "Turma01",
                UeID = "UE01"
            };
        }
    }
}
