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
        private readonly Mock<IRepositorioDreConsulta> _repositorioDreConsultaMock;
        private readonly Mock<IRepositorioTurmaConsulta> _repositorioTurmaConsultaMock;
        private readonly Mock<IRepositorioUeConsulta> _repositorioUeConsultaaMock;
        private readonly Mock<IRepositorioTipoEscola> _repositorioTipoEscolaMock;

        private readonly ConsolidarInformacoesFrequenciaPainelEducacionalUseCase _useCase;

        public ConsolidarInformacoesFrequenciaPainelEducacionalUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _repositorioFrequenciaMock = new Mock<IRepositorioFrequenciaConsulta>();
            _repositorioDreConsultaMock = new Mock<IRepositorioDreConsulta>();
            _repositorioTurmaConsultaMock = new Mock<IRepositorioTurmaConsulta>();
            _repositorioUeConsultaaMock = new Mock<IRepositorioUeConsulta>();
            _repositorioTipoEscolaMock = new Mock<IRepositorioTipoEscola>();

            _repositorioDreConsultaMock.Setup(r => r.ObterTodas())
                .ReturnsAsync(new List<SME.SGP.Dominio.Dre>
                {
                    new SME.SGP.Dominio.Dre
                    {
                        Id = 1,
                        CodigoDre = "123456",
                        Nome = "DIRETORIA REGIONAL DE EDUCACAO IPIRANGA",
                        Abreviacao = "IP",
                        DataAtualizacao = DateTime.Now
                    },
                    new SME.SGP.Dominio.Dre
                    {
                        Id = 2,
                        CodigoDre = "654321",
                        Nome = "DIRETORIA REGIONAL DE EDUCACAO SANTO AMARO",
                        Abreviacao = "SA",
                        DataAtualizacao = DateTime.Now
                    }
                });

            _repositorioUeConsultaaMock.Setup(r => r.ObterTodas())
                .Returns(new List<Ue>
                {
                    new Ue { Id = 1, CodigoUe = "987654", Nome = "ESCOLA TESTE 1", DreId = 1, TipoEscola = Dominio.TipoEscola.EMEF },
                    new Ue { Id = 2, CodigoUe = "456789", Nome = "ESCOLA TESTE 2", DreId = 2, TipoEscola = Dominio.TipoEscola.CIEJA }
                });

            _repositorioTurmaConsultaMock.Setup(r => r.ObterTodasTurmasPainelEducacionalFrequenciaAsync())
                .ReturnsAsync(new List<TurmaPainelEducacionalFrequenciaDto>
                {
                    new TurmaPainelEducacionalFrequenciaDto
                    {
                        TurmaId = 1,
                        UeId = 1,
                        ModalidadeCodigo = (int)Modalidade.EducacaoInfantil,
                        Ano = "2", 
                        AnoLetivo = DateTime.Now.Year.ToString()
                    },
                    new TurmaPainelEducacionalFrequenciaDto
                    {
                        TurmaId = 2,
                        UeId = 2,
                        ModalidadeCodigo = (int)Modalidade.Fundamental,
                        Ano = "6", 
                        AnoLetivo = DateTime.Now.Year.ToString()
                    }
                });

            _repositorioTipoEscolaMock.Setup(r => r.ObterTodasAsync())
                .ReturnsAsync(new List<TipoEscolaEol>
                {
                    new TipoEscolaEol { CodEol = (int)Dominio.TipoEscola.EMEF, Descricao = "EMEF" },
                    new TipoEscolaEol { CodEol = (int)Dominio.TipoEscola.CIEJA, Descricao = "CIEJA" }
                });

            _useCase = new ConsolidarInformacoesFrequenciaPainelEducacionalUseCase(
                _mediatorMock.Object,
                _repositorioFrequenciaMock.Object,
                _repositorioDreConsultaMock.Object,
                _repositorioTurmaConsultaMock.Object,
                _repositorioUeConsultaaMock.Object,
                _repositorioTipoEscolaMock.Object);
        }

        [Fact]
        public async Task Executar_DeveConsolidarFrequenciaEChamarMediatorCorretamente()
        {
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

            var resultado = await _useCase.Executar(mensagemRabbit);

            resultado.Should().BeTrue();

            _mediatorMock.Verify(m => m.Send(It.IsAny<PainelEducacionalExcluirAgrupamentoMensalCommand>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PainelEducacionalExcluirAgrupamentoGlobalCommand>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PainelEducacionalExcluirAgrupamentoGlobalEscolaCommand>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);

            commandMensalCapturado.Should().NotBeNull();
            commandMensalCapturado.RegistroFrequencia.Should().HaveCount(2);
            
            var agrupamentoMensalCreche = commandMensalCapturado.RegistroFrequencia.FirstOrDefault(r => r.Modalidade == "Creche");
            agrupamentoMensalCreche.Should().NotBeNull("Deve existir um agrupamento para Creche");
            agrupamentoMensalCreche.Modalidade.Should().Be("Creche");
            agrupamentoMensalCreche.TotalAulas.Should().Be(20);
            agrupamentoMensalCreche.TotalFaltas.Should().Be(2);

            var agrupamentoMensalFundamental = commandMensalCapturado.RegistroFrequencia.FirstOrDefault(r => r.Modalidade == "Ensino Fundamental");
            agrupamentoMensalFundamental.Should().NotBeNull("Deve existir um agrupamento para Ensino Fundamental");
            agrupamentoMensalFundamental.Modalidade.Should().Be("Ensino Fundamental");
            agrupamentoMensalFundamental.TotalAulas.Should().Be(20);
            agrupamentoMensalFundamental.TotalFaltas.Should().Be(1);

            commandGlobalCapturado.Should().NotBeNull();
            commandGlobalCapturado.RegistroFrequencia.Should().HaveCount(2);
            
            var agrupamentoGlobalCreche = commandGlobalCapturado.RegistroFrequencia.FirstOrDefault(r => r.Modalidade == "Creche");
            agrupamentoGlobalCreche.Should().NotBeNull("Deve existir um agrupamento global para Creche");
            agrupamentoGlobalCreche.TotalAlunos.Should().Be(1);
            agrupamentoGlobalCreche.TotalAulas.Should().Be(20);
            agrupamentoGlobalCreche.TotalAusencias.Should().Be(2);

            var agrupamentoGlobalFundamental = commandGlobalCapturado.RegistroFrequencia.FirstOrDefault(r => r.Modalidade == "Ensino Fundamental");
            agrupamentoGlobalFundamental.Should().NotBeNull("Deve existir um agrupamento global para Ensino Fundamental");
            agrupamentoGlobalFundamental.TotalAlunos.Should().Be(1);
            agrupamentoGlobalFundamental.TotalAulas.Should().Be(20);
            agrupamentoGlobalFundamental.TotalAusencias.Should().Be(1);

            commandEscolaCapturado.Should().NotBeNull();
            commandEscolaCapturado.RegistroFrequencia.Should().HaveCount(2);
            
            var agrupamentoEscola1 = commandEscolaCapturado.RegistroFrequencia.FirstOrDefault(r => r.CodigoUe == "987654");
            agrupamentoEscola1.Should().NotBeNull("Deve existir um agrupamento para a escola 987654");
            agrupamentoEscola1.TotalAlunos.Should().Be(1);
            agrupamentoEscola1.TotalAulas.Should().Be(20);
            agrupamentoEscola1.TotalAusencias.Should().Be(2);

            var agrupamentoEscola2 = commandEscolaCapturado.RegistroFrequencia.FirstOrDefault(r => r.CodigoUe == "456789");
            agrupamentoEscola2.Should().NotBeNull("Deve existir um agrupamento para a escola 456789");
            agrupamentoEscola2.TotalAlunos.Should().Be(1);
            agrupamentoEscola2.TotalAulas.Should().Be(20);
            agrupamentoEscola2.TotalAusencias.Should().Be(1);
        }

        [Fact]
        public async Task Executar_QuandoNaoHouverRegistros_DeveChamarExclusaoESalvarComListaVazia()
        {
            var mensagemRabbit = new MensagemRabbit();
            var listaVazia = new List<RegistroFrequenciaPainelEducacionalDto>();

            _repositorioFrequenciaMock.Setup(r => r.ObterInformacoesFrequenciaPainelEducacional(It.IsAny<IEnumerable<long>>()))
                                      .ReturnsAsync(listaVazia);

            PainelEducacionalSalvarAgrupamentoMensalCommand commandMensalCapturado = null;

            _mediatorMock.Setup(m => m.Send(It.IsAny<PainelEducacionalSalvarAgrupamentoMensalCommand>(), It.IsAny<CancellationToken>()))
                         .Callback<IRequest<bool>, CancellationToken>((cmd, ct) => commandMensalCapturado = cmd as PainelEducacionalSalvarAgrupamentoMensalCommand)
                         .ReturnsAsync(true);

            await _useCase.Executar(mensagemRabbit);

            _mediatorMock.Verify(m => m.Send(It.IsAny<PainelEducacionalExcluirAgrupamentoMensalCommand>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PainelEducacionalExcluirAgrupamentoGlobalCommand>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PainelEducacionalExcluirAgrupamentoGlobalEscolaCommand>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);

            commandMensalCapturado.Should().NotBeNull();
            commandMensalCapturado.RegistroFrequencia.Should().NotBeNull().And.BeEmpty();
        }

        private IEnumerable<RegistroFrequenciaPainelEducacionalDto> GerarRegistrosFrequenciaDto()
        {
            return new List<RegistroFrequenciaPainelEducacionalDto>()
            {
                new RegistroFrequenciaPainelEducacionalDto
                {
                    TurmaId = 1,
                    CodigoDre = "123456",
                    CodigoUe = "987654",
                    Ue = "ESCOLA TESTE 1",
                    CodigoAluno = "111",
                    Mes = 9,
                    Percentual = 90,
                    QuantidadeAulas = 20,
                    QuantidadeAusencias = 2,
                    ModalidadeCodigo = (int)Modalidade.EducacaoInfantil,
                    AnoTurma = "2", 
                    AnoLetivo = DateTime.Now.Year
                },
                new RegistroFrequenciaPainelEducacionalDto
                {
                    TurmaId = 2,
                    CodigoDre = "654321",
                    CodigoUe = "456789",
                    Ue = "ESCOLA TESTE 2",
                    CodigoAluno = "222222",
                    Mes = 9,
                    Percentual = 95,
                    QuantidadeAulas = 20,
                    QuantidadeAusencias = 1,
                    ModalidadeCodigo = (int)Modalidade.Fundamental,
                    AnoTurma = "6", 
                    AnoLetivo = DateTime.Now.Year
                }
            };
        }
    }
}
