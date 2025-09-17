using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarAgrupamentoEscola;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarAgrupamentoGlobal;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarAgrupamentoMensal;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading;
using Xunit;
using System.Threading.Tasks;
using SME.SGP.Infra;
using FluentAssertions;
using SME.SGP.Aplicacao.Commands.PainelEducacional.ExcluirAgrupamentoMensal;
using SME.SGP.Aplicacao.Commands.PainelEducacional.ExcluirAgrupamentoGlobal;
using SME.SGP.Aplicacao.Commands.PainelEducacional.ExcluirAgrupamentoEscola;
using System;
using System.Linq;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PainelEducacional
{
    public class ConsolidarInformacoesFrequenciaPainelEducacionalUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IRepositorioFrequenciaConsulta> _repositorioFrequenciaMock;
        private readonly ConsolidarInformacoesFrequenciaPainelEducacionalUseCase _useCase;


        private readonly Mock<IRepositorioDreConsulta> _repositorioDreConsultaMock;
        private readonly Mock<IRepositorioTurmaConsulta> _repositorioTurmaConsultaMock;
        private readonly Mock<IRepositorioUeConsulta> _repositorioUeConsultaaMock;
        private readonly Mock<IRepositorioTipoEscola> _repositorioTipoEscolaMock;

        public ConsolidarInformacoesFrequenciaPainelEducacionalUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _repositorioFrequenciaMock = new Mock<IRepositorioFrequenciaConsulta>();
            _useCase = new ConsolidarInformacoesFrequenciaPainelEducacionalUseCase(_mediatorMock.Object, _repositorioFrequenciaMock.Object, _repositorioDreConsultaMock.Object, _repositorioTurmaConsultaMock.Object,
                _repositorioUeConsultaaMock.Object, _repositorioTipoEscolaMock.Object);
        }

        [Fact]
        public async Task Executar_DeveConsolidarFrequenciaEChamarMediatorCorretamente()
        {
            // Arrange
            var mensagemRabbit = new MensagemRabbit();
            var listaRegistrosFrequencia = GerarRegistrosFrequenciaDto();

            _repositorioFrequenciaMock.Setup(r => r.ObterInformacoesFrequenciaPainelEducacional(It.IsAny<IEnumerable<long>>()))
                                      .ReturnsAsync(listaRegistrosFrequencia);

            PainelEducacionalSalvarAgrupamentoMensalCommand commandMensalCapturado = null;
            PainelEducacionalSalvarAgrupamentoGlobalCommand commandGlobalCapturado = null;
            PainelEducacionalSalvarAgrupamentoGlobalEscolaCommand commandEscolaCapturado = null;

            _mediatorMock.Setup(m => m.Send(It.IsAny<PainelEducacionalSalvarAgrupamentoMensalCommand>(), It.IsAny<CancellationToken>()))
                         .Callback<IRequest<bool>, CancellationToken>((cmd, ct) => commandMensalCapturado = cmd as PainelEducacionalSalvarAgrupamentoMensalCommand)
                         .ReturnsAsync(true);

            _mediatorMock.Setup(m => m.Send(It.IsAny<PainelEducacionalSalvarAgrupamentoGlobalCommand>(), It.IsAny<CancellationToken>()))
                         .Callback<IRequest<bool>, CancellationToken>((cmd, ct) => commandGlobalCapturado = cmd as PainelEducacionalSalvarAgrupamentoGlobalCommand)
                         .ReturnsAsync(true);

            _mediatorMock.Setup(m => m.Send(It.IsAny<PainelEducacionalSalvarAgrupamentoGlobalEscolaCommand>(), It.IsAny<CancellationToken>()))
                         .Callback<IRequest<bool>, CancellationToken>((cmd, ct) => commandEscolaCapturado = cmd as PainelEducacionalSalvarAgrupamentoGlobalEscolaCommand)
                         .ReturnsAsync(true);

            // Act
            var resultado = await _useCase.Executar(mensagemRabbit);

            // Assert
            resultado.Should().BeTrue();

            //_repositorioFrequenciaMock.Verify(r => r.ObterInformacoesFrequenciaPainelEducacional(DateTime.Now.Year), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.IsAny<PainelEducacionalExcluirAgrupamentoMensalCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PainelEducacionalExcluirAgrupamentoGlobalCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PainelEducacionalExcluirAgrupamentoGlobalEscolaCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            // Validações do Agrupamento Mensal
            commandMensalCapturado.Should().NotBeNull();
            commandMensalCapturado.RegistroFrequencia.Should().HaveCount(2);
            var agrupamentoMensal = commandMensalCapturado.RegistroFrequencia.First();
            agrupamentoMensal.Modalidade.Should().Be("Creche");
            agrupamentoMensal.TotalAulas.Should().Be(20);
            agrupamentoMensal.TotalFaltas.Should().Be(2);

            // Validações do Agrupamento Global
            commandGlobalCapturado.Should().NotBeNull();
            commandGlobalCapturado.RegistroFrequencia.Should().HaveCount(2);
            var agrupamentoGlobalCreche = commandGlobalCapturado.RegistroFrequencia.First(r => r.Modalidade == "Creche");
            agrupamentoGlobalCreche.TotalAlunos.Should().Be(1);
            agrupamentoGlobalCreche.TotalAulas.Should().Be(20);
            agrupamentoGlobalCreche.TotalAusencias.Should().Be(2);

            // Validações do Agrupamento por Escola
            commandEscolaCapturado.Should().NotBeNull();
            commandEscolaCapturado.RegistroFrequencia.Should().HaveCount(2);
            var agrupamentoEscola1 = commandEscolaCapturado.RegistroFrequencia.First(r => r.CodigoUe == "UE01");
            agrupamentoEscola1.TotalAlunos.Should().Be(1);
            agrupamentoEscola1.TotalAulas.Should().Be(20);
            agrupamentoEscola1.TotalAusencias.Should().Be(2);
        }

        [Fact]
        public async Task Executar_QuandoNaoHouverRegistros_DeveChamarExclusaoESalvarComListaVazia()
        {
            // Arrange
            var mensagemRabbit = new MensagemRabbit();
            var listaVazia = new List<RegistroFrequenciaPainelEducacionalDto>();

            _repositorioFrequenciaMock.Setup(r => r.ObterInformacoesFrequenciaPainelEducacional(It.IsAny<IEnumerable<long>>()))
                                      .ReturnsAsync(listaVazia);

            PainelEducacionalSalvarAgrupamentoMensalCommand commandMensalCapturado = null;

            _mediatorMock.Setup(m => m.Send(It.IsAny<PainelEducacionalSalvarAgrupamentoMensalCommand>(), It.IsAny<CancellationToken>()))
                         .Callback<IRequest<bool>, CancellationToken>((cmd, ct) => commandMensalCapturado = cmd as PainelEducacionalSalvarAgrupamentoMensalCommand)
                         .ReturnsAsync(true);

            // Act
            await _useCase.Executar(mensagemRabbit);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.IsAny<PainelEducacionalExcluirAgrupamentoMensalCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PainelEducacionalExcluirAgrupamentoGlobalCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PainelEducacionalExcluirAgrupamentoGlobalEscolaCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            commandMensalCapturado.Should().NotBeNull();
            commandMensalCapturado.RegistroFrequencia.Should().NotBeNull().And.BeEmpty();
        }

        private IEnumerable<RegistroFrequenciaPainelEducacionalDto> GerarRegistrosFrequenciaDto()
        {
            return new List<RegistroFrequenciaPainelEducacionalDto>()
            {
                new RegistroFrequenciaPainelEducacionalDto
                {
                    CodigoDre = "DRE01",
                    CodigoUe = "UE01",
                    Ue = "ESCOLA TESTE 1",
                    CodigoAluno = "ALUNO01",
                    Mes = 9,
                    Percentual = 90,
                    QuantidadeAulas = 20,
                    QuantidadeAusencias = 2,
                    ModalidadeCodigo = (int)Modalidade.EducacaoInfantil,
                    AnoTurma = "2", // Creche
                    AnoLetivo = DateTime.Now.Year
                },
                new RegistroFrequenciaPainelEducacionalDto
                {
                    CodigoDre = "DRE02",
                    CodigoUe = "UE02",
                    Ue = "ESCOLA TESTE 2",
                    CodigoAluno = "ALUNO02",
                    Mes = 9,
                    Percentual = 95,
                    QuantidadeAulas = 20,
                    QuantidadeAusencias = 1,
                    ModalidadeCodigo = (int)Modalidade.Fundamental,
                    AnoTurma = "6", // Fundamental
                    AnoLetivo = DateTime.Now.Year
                }
            };
        }
    }
}
