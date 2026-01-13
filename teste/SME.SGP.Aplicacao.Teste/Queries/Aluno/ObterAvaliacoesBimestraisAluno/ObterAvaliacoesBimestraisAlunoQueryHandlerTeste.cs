using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.Aluno.ObterAvaliacoesBimestraisAluno
{
    public class ObterAvaliacoesBimestraisAlunoQueryHandlerTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IRepositorioAvaliacoesBimestraisAluno> _repositorioMock;
        private readonly ObterAvaliacoesBimestraisAlunoQueryHandler _handler;

        public ObterAvaliacoesBimestraisAlunoQueryHandlerTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _repositorioMock = new Mock<IRepositorioAvaliacoesBimestraisAluno>();
            _handler = new ObterAvaliacoesBimestraisAlunoQueryHandler(_mediatorMock.Object, _repositorioMock.Object);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Null_Quando_Aluno_Nao_Encontrado()
        {
            var query = new ObterAvaliacoesBimestraisAlunoQuery("123456", 2024);
            
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunoPorCodigoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((AlunoReduzidoDto)null);

            var resultado = await _handler.Handle(query, CancellationToken.None);

            Assert.Null(resultado);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Null_Quando_Aluno_Nao_Tem_Turmas()
        {
            var query = new ObterAvaliacoesBimestraisAlunoQuery("123456", 2024);
            var aluno = new AlunoReduzidoDto { CodigoAluno = "123456", Nome = "Aluno Teste" };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunoPorCodigoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aluno);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasAlunoPorFiltroQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoPorTurmaResposta>());

            var resultado = await _handler.Handle(query, CancellationToken.None);

            Assert.Null(resultado);
        }

        [Fact]
        public async Task Handle_Deve_Processar_Turma_Ensino_Fundamental_Com_4_Bimestres()
        {
            var query = new ObterAvaliacoesBimestraisAlunoQuery("123456", 2024);
            var aluno = new AlunoReduzidoDto { CodigoAluno = "123456", Nome = "Aluno Teste" };
            var turmasAluno = new List<AlunoPorTurmaResposta>
            {
                new AlunoPorTurmaResposta { CodigoTurma = 1234567 }
            };

            var turma = new Turma 
            { 
                Id = 1, 
                CodigoTurma = "1234567", 
                ModalidadeCodigo = Modalidade.Fundamental 
            };

            var bimestresComConselhoClasse = new List<BimestreComConselhoClasseTurmaDto>
            {
                new BimestreComConselhoClasseTurmaDto { ConselhoClasseId = 1, FechamentoTurmaId = 1, Bimestre = 1 },
                new BimestreComConselhoClasseTurmaDto { ConselhoClasseId = 2, FechamentoTurmaId = 2, Bimestre = 2 },
                new BimestreComConselhoClasseTurmaDto { ConselhoClasseId = 3, FechamentoTurmaId = 3, Bimestre = 3 },
                new BimestreComConselhoClasseTurmaDto { ConselhoClasseId = 4, FechamentoTurmaId = 4, Bimestre = 4 }
            };
            var indicadoresBimestre = new List<IndicadorAvaliacaoDto>
            {
                new IndicadorAvaliacaoDto { Componente = "Indicador 1", Nota = "Satisfatório" }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunoPorCodigoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aluno);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasAlunoPorFiltroQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmasAluno);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterBimestresComConselhoClasseTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(bimestresComConselhoClasse);

            _repositorioMock.Setup(r => r.ObterIndicadoresPorBimestre(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(indicadoresBimestre);

            var resultado = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal("123456", resultado.CodigoAluno);
            Assert.Equal("Aluno Teste", resultado.NomeAluno);
            Assert.True(resultado.PossuiConselhoClasse);
            Assert.Equal(4, resultado.AvaliacoesBimestrais.Count());
            
            _repositorioMock.Verify(r => r.ObterIndicadoresPorBimestre(
                It.IsAny<string>(), It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), 
                Times.Exactly(4));
        }

        [Fact]
        public async Task Handle_Deve_Processar_Turma_EJA_Com_3_Trimestres()
        {

            var query = new ObterAvaliacoesBimestraisAlunoQuery("123456", 2024);
            var aluno = new AlunoReduzidoDto { CodigoAluno = "123456", Nome = "Aluno Teste" };
            var turmasAluno = new List<AlunoPorTurmaResposta>
            {
                new AlunoPorTurmaResposta { CodigoTurma = 1234567 }
            };

            var turma = new Turma 
            { 
                Id = 1, 
                CodigoTurma = "1234567", 
                ModalidadeCodigo = Modalidade.EJA 
            };

            var bimestresComConselhoClasse = new List<BimestreComConselhoClasseTurmaDto>
            {
                new BimestreComConselhoClasseTurmaDto { ConselhoClasseId = 1, FechamentoTurmaId = 1, Bimestre = 1 },
                new BimestreComConselhoClasseTurmaDto { ConselhoClasseId = 2, FechamentoTurmaId = 2, Bimestre = 2 },
                new BimestreComConselhoClasseTurmaDto { ConselhoClasseId = 3, FechamentoTurmaId = 3, Bimestre = 3 }
            };
            var indicadoresBimestre = new List<IndicadorAvaliacaoDto>
            {
                new IndicadorAvaliacaoDto { Componente = "Indicador EJA", Nota = "Satisfatório" }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunoPorCodigoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aluno);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasAlunoPorFiltroQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmasAluno);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterBimestresComConselhoClasseTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(bimestresComConselhoClasse);

            _repositorioMock.Setup(r => r.ObterIndicadoresPorBimestre(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(indicadoresBimestre);

            var resultado = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(3, resultado.AvaliacoesBimestrais.Count());
            
            _repositorioMock.Verify(r => r.ObterIndicadoresPorBimestre(
                It.IsAny<string>(), It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), 
                Times.Exactly(3));
        }

        [Fact]
        public async Task Handle_Deve_Ignorar_Modalidade_Nao_Permitida()
        {
            var query = new ObterAvaliacoesBimestraisAlunoQuery("123456", 2024);
            var aluno = new AlunoReduzidoDto { CodigoAluno = "123456", Nome = "Aluno Teste" };
            var turmasAluno = new List<AlunoPorTurmaResposta>
            {
                new AlunoPorTurmaResposta { CodigoTurma = 1234567 }
            };

            var turma = new Turma 
            { 
                Id = 1, 
                CodigoTurma = "1234567", 
                ModalidadeCodigo = Modalidade.EducacaoInfantil 
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunoPorCodigoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aluno);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasAlunoPorFiltroQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmasAluno);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            var resultado = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.False(resultado.PossuiConselhoClasse);
            Assert.Empty(resultado.AvaliacoesBimestrais);
        }

        [Fact]
        public async Task Handle_Deve_Incluir_Avaliacao_Final_Quando_Disponivel()
        {
            var query = new ObterAvaliacoesBimestraisAlunoQuery("123456", 2024);
            var aluno = new AlunoReduzidoDto { CodigoAluno = "123456", Nome = "Aluno Teste" };
            var turmasAluno = new List<AlunoPorTurmaResposta>
            {
                new AlunoPorTurmaResposta { CodigoTurma = 1234567 }
            };

            var turma = new Turma 
            { 
                Id = 1, 
                CodigoTurma = "1234567", 
                ModalidadeCodigo = Modalidade.Fundamental 
            };

            var bimestresComConselhoClasse = new List<BimestreComConselhoClasseTurmaDto>
            {
                new BimestreComConselhoClasseTurmaDto { ConselhoClasseId = 1, FechamentoTurmaId = 1, Bimestre = 1 },
                new BimestreComConselhoClasseTurmaDto { ConselhoClasseId = 2, FechamentoTurmaId = 2, Bimestre = 2 },
                new BimestreComConselhoClasseTurmaDto { ConselhoClasseId = 3, FechamentoTurmaId = 3, Bimestre = 3 },
                new BimestreComConselhoClasseTurmaDto { ConselhoClasseId = 4, FechamentoTurmaId = 4, Bimestre = 4 }
            };
            var indicadoresBimestre = new List<IndicadorAvaliacaoDto>
            {
                new IndicadorAvaliacaoDto {Componente = "Indicador 1", Nota = "Satisfatório"}
            };

            var indicadoresFinais = new List<IndicadorAvaliacaoDto>
            {
                new IndicadorAvaliacaoDto { Componente = "Indicador Final", Nota = "Aprovado" }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunoPorCodigoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aluno);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasAlunoPorFiltroQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmasAluno);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterBimestresComConselhoClasseTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(bimestresComConselhoClasse);

            _repositorioMock.Setup(r => r.ObterIndicadoresPorBimestre(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(indicadoresBimestre);

            _repositorioMock.Setup(r => r.ObterIndicadoresAvaliacaoFinal(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(indicadoresFinais);

            var resultado = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.NotNull(resultado.AvaliacaoFinal);
            Assert.Single(resultado.AvaliacaoFinal.Indicadores);
            Assert.Equal("Indicador Final", resultado.AvaliacaoFinal.Indicadores.First().Componente);

            _repositorioMock.Verify(r => r.ObterIndicadoresAvaliacaoFinal(
                It.IsAny<string>(), It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int>()), 
                Times.Once);
        }

        [Fact]
        public async Task Handle_Deve_Continuar_Quando_Turma_Nao_Encontrada()
        {
            var query = new ObterAvaliacoesBimestraisAlunoQuery("123456", 2024);
            var aluno = new AlunoReduzidoDto { CodigoAluno = "123456", Nome = "Aluno Teste" };
            var turmasAluno = new List<AlunoPorTurmaResposta>
            {
                new AlunoPorTurmaResposta { CodigoTurma = 1234567 },
                new AlunoPorTurmaResposta { CodigoTurma = 9999999 }
            };

            var turma = new Turma 
            { 
                Id = 1, 
                CodigoTurma = "9999999", 
                ModalidadeCodigo = Modalidade.Fundamental 
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunoPorCodigoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aluno);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasAlunoPorFiltroQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmasAluno);

            _mediatorMock.SetupSequence(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Turma)null)
                .ReturnsAsync(turma);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterBimestresComConselhoClasseTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<BimestreComConselhoClasseTurmaDto> { new BimestreComConselhoClasseTurmaDto { ConselhoClasseId = 1, FechamentoTurmaId = 1, Bimestre = 1 } });

            _repositorioMock.Setup(r => r.ObterIndicadoresPorBimestre(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<IndicadorAvaliacaoDto>());

            var resultado = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()), 
                Times.Exactly(2));
        }

        [Fact]
        public void Construtor_Deve_Lancar_Excecao_Quando_Mediator_Null()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new ObterAvaliacoesBimestraisAlunoQueryHandler(null, _repositorioMock.Object));
            
            Assert.Equal("mediator", exception.ParamName);
        }

        [Fact]
        public void Construtor_Deve_Lancar_Excecao_Quando_Repositorio_Null()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new ObterAvaliacoesBimestraisAlunoQueryHandler(_mediatorMock.Object, null));
            
            Assert.Equal("repositorio", exception.ParamName);
        }

        [Theory]
        [InlineData(Modalidade.Fundamental, true)]
        [InlineData(Modalidade.Medio, true)]
        [InlineData(Modalidade.EJA, true)]
        [InlineData(Modalidade.EducacaoInfantil, false)]
        [InlineData(Modalidade.CIEJA, false)]
        public async Task Handle_Deve_Validar_Modalidades_Permitidas_Corretamente(Modalidade modalidade, bool esperaProcessar)
        {
            var query = new ObterAvaliacoesBimestraisAlunoQuery("123456", 2024);
            var aluno = new AlunoReduzidoDto { CodigoAluno = "123456", Nome = "Aluno Teste" };
            var turmasAluno = new List<AlunoPorTurmaResposta>
            {
                new AlunoPorTurmaResposta { CodigoTurma = 1234567 }
            };

            var turma = new Turma 
            { 
                Id = 1, 
                CodigoTurma = "1234567", 
                ModalidadeCodigo = modalidade 
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunoPorCodigoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aluno);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasAlunoPorFiltroQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmasAluno);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            if (esperaProcessar)
            {
                _mediatorMock.Setup(m => m.Send(It.IsAny<ObterBimestresComConselhoClasseTurmaQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new List<BimestreComConselhoClasseTurmaDto> { new BimestreComConselhoClasseTurmaDto { ConselhoClasseId = 1, FechamentoTurmaId = 1, Bimestre = 1 } });

                _repositorioMock.Setup(r => r.ObterIndicadoresPorBimestre(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                    .ReturnsAsync(new List<IndicadorAvaliacaoDto>());
            }

            var resultado = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            
            if (esperaProcessar)
            {
                _mediatorMock.Verify(m => m.Send(It.IsAny<ObterBimestresComConselhoClasseTurmaQuery>(), It.IsAny<CancellationToken>()), 
                    Times.Once);
            }
            else
            {
                _mediatorMock.Verify(m => m.Send(It.IsAny<ObterBimestresComConselhoClasseTurmaQuery>(), It.IsAny<CancellationToken>()), 
                    Times.Never);
            }
        }
    }
}