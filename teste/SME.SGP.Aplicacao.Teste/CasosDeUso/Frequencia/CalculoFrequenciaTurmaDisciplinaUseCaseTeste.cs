using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Frequencia
{
    public class CalculoFrequenciaTurmaDisciplinaUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly CalculoFrequenciaTurmaDisciplinaUseCase useCase;

        public CalculoFrequenciaTurmaDisciplinaUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new CalculoFrequenciaTurmaDisciplinaUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DeveExecutarComando_SeComandoForValido()
        {
            // Arrange
            var comando = new CalcularFrequenciaPorTurmaCommand(
                alunos: new[] { "1", "2" },
                dataAula: DateTime.Today,
                turmaId: "T1",
                disciplinaId: "D1"
            );

            var mensagem = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(comando),
                UsuarioLogadoRF = "123456",
                PerfilUsuario = "Administrador"
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<CalcularFrequenciaPorTurmaCommand>(), default))
                .ReturnsAsync(true);

            // Act
            var resultado = await useCase.Executar(mensagem);

            // Assert
            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.Is<CalcularFrequenciaPorTurmaCommand>(
                c =>
                    c.TurmaId == "T1" &&
                    c.UsuarioConsiderado.rf == "123456" &&
                    c.UsuarioConsiderado.perfil == "Administrador"
            ), default), Times.Once);
        }


        [Fact]
        public async Task IncluirCalculoFila_DeveEnviarComandoComDadosValidados()
        {
            // Arrange
            var dto = new CalcularFrequenciaDto
            {
                CodigoTurma = "T1",
                DataReferencia = DateTime.Today,
                CodigoComponenteCurricular = "CC1",
                CodigosAlunos = new[] { "1", "2" },
                Bimestre = 1
            };

            var alunos = new List<AlunoPorTurmaResposta>
    {
        new AlunoPorTurmaResposta { CodigoAluno = "1" },
        new AlunoPorTurmaResposta { CodigoAluno = "2" }
    };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), default))
                .ReturnsAsync(new Turma
                {
                    CodigoTurma = "T1",
                    ModalidadeCodigo = Modalidade.Fundamental,
                    Ano = "3",
                    AnoLetivo = DateTime.Now.Year,
                    TipoTurma = TipoTurma.Regular,
                    Ue = new Ue
                    {
                        Id = 1,
                        Nome = "Escola Teste",
                        Dre = new Dre { Abreviacao = "DRE1" }
                    }
                });

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosPorTurmaEAnoLetivoQuery>(), default))
                .ReturnsAsync(alunos);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesQuery>(), default))
                .ReturnsAsync(new List<ComponenteCurricularDto>
                {
            new ComponenteCurricularDto { Codigo = "CC1" }
                });

            // Act
            await useCase.IncluirCalculoFila(dto);

            // Assert
            mediatorMock.Verify(m => m.Send(It.Is<IncluirFilaCalcularFrequenciaPorTurmaCommand>(
                c =>
                    c.Alunos.SequenceEqual(alunos.Select(a => a.CodigoAluno.ToString())) &&
                    c.DisciplinaId == "CC1" &&
                    c.TurmaId == "T1" &&
                    c.DataAula == dto.DataReferencia
            ), default), Times.Once);
        }



        [Theory]
        [InlineData(null, "Informe o código da turma.")]
        [InlineData("", "Informe o código da turma.")]
        public async Task ValidarDadosCalculo_DeveLancarExcecao_SeCodigoTurmaForInvalido(string codigoTurma, string mensagemErro)
        {
            // Arrange
            var dto = new CalcularFrequenciaDto { CodigoTurma = codigoTurma };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<NegocioException>(() => useCase.ValidarDadosCalculo(dto));
            Assert.Equal(mensagemErro, ex.Message);
        }

        [Fact]
        public async Task ValidarDadosCalculo_DeveLancarExcecao_SeAlunosNaoForemEncontrados()
        {
            // Arrange
            var dto = new CalcularFrequenciaDto
            {
                CodigoTurma = "T1",
                CodigoComponenteCurricular = "CC1",
                DataReferencia = DateTime.Today,
                Bimestre = 1
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), default))
                .ReturnsAsync(new Turma
                {
                    CodigoTurma = "T1",
                    ModalidadeCodigo = Modalidade.Fundamental,
                    Ano = "3",
                    AnoLetivo = DateTime.Now.Year
                });

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosPorTurmaEAnoLetivoQuery>(), default))
                .ReturnsAsync((IEnumerable<AlunoPorTurmaResposta>)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<NegocioException>(() => useCase.ValidarDadosCalculo(dto));
            Assert.Equal("Não foram localizados alunos para a turma informada.", ex.Message);
        }


        [Fact]
        public async Task ValidarDadosCalculo_DeveLancarExcecao_SeComponenteCurricularForInvalido()
        {
            // Arrange
            var dto = new CalcularFrequenciaDto
            {
                CodigoTurma = "T1",
                CodigoComponenteCurricular = "INVALIDO",
                CodigosAlunos = new[] { "1" },
                DataReferencia = DateTime.Today,
                Bimestre = 1
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), default))
                .ReturnsAsync(new Turma
                {
                    CodigoTurma = "T1",
                    ModalidadeCodigo = Modalidade.Fundamental,
                    Ano = "3",
                    AnoLetivo = DateTime.Now.Year,
                    TipoTurma = TipoTurma.Regular,
                    Ue = new Ue
                    {
                        Id = 1,
                        Nome = "Escola Teste",
                        Dre = new Dre { Abreviacao = "DRE1" }
                    }
                });

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosPorTurmaEAnoLetivoQuery>(), default))
                .ReturnsAsync(new List<AlunoPorTurmaResposta>
                {
            new AlunoPorTurmaResposta { CodigoAluno = "1" }
                });

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesQuery>(), default))
                .ReturnsAsync(new List<ComponenteCurricularDto>
                {
            new ComponenteCurricularDto { Codigo = "MAT" },
            new ComponenteCurricularDto { Codigo = "PORT" }
                });

            // Act & Assert
            var ex = await Assert.ThrowsAsync<NegocioException>(() => useCase.ValidarDadosCalculo(dto));
            Assert.Equal("O código de componente curricular informado é inválido.", ex.Message);
        }

    }
}



