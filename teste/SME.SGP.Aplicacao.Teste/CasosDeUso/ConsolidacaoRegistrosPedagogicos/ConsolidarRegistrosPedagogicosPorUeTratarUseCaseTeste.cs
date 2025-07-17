using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConsolidacaoRegistrosPedagogicos
{
    public class ConsolidarRegistrosPedagogicosPorUeTratarUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IRepositorioUeConsulta> repositorioUeMock;
        private readonly ConsolidarRegistrosPedagogicosPorUeTratarUseCase useCase;

        public ConsolidarRegistrosPedagogicosPorUeTratarUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            repositorioUeMock = new Mock<IRepositorioUeConsulta>();

            useCase = new ConsolidarRegistrosPedagogicosPorUeTratarUseCase(mediatorMock.Object, repositorioUeMock.Object);
        }

        [Fact]
        public async Task Executar_Parametro_Sistema_Existe_Deve_Publicar_Para_Cada_Turma()
        {
            var filtro = new FiltroConsolidacaoRegistrosPedagogicosPorUeDto(1, 2025);
            var mensagem = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(filtro)
            };

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new ParametrosSistema { Valor = "true" });

            repositorioUeMock.Setup(x => x.ObterUePorId(It.IsAny<long>()))
                             .ReturnsAsync(new Ue { CodigoUe = "UE123" });

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterProfessoresTitularesPorUeQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<ProfessorTitularDisciplinaEol>
                        {
                            new ProfessorTitularDisciplinaEol
                            {
                                TurmaId = 100,
                                CodigosDisciplinas = "1",
                                ProfessorNome = "João",
                                ProfessorRf = "123456"
                            }
                        });

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);

            mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(
                cmd => cmd.Rota == RotasRabbitSgp.ConsolidarRegistrosPedagogicosPorTurmaTratar),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Parametro_Sistema_Nao_Existe_Deve_Executar_Consolidacao_Direta()
        {
            var filtro = new FiltroConsolidacaoRegistrosPedagogicosPorUeDto(1, 2025);
            var mensagem = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(filtro)
            };

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((ParametrosSistema)null);

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterConsolidacaoRegistrosPedagogicosQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<ConsolidacaoRegistrosPedagogicosDto>
                        {
                            new ConsolidacaoRegistrosPedagogicosDto
                            {
                                TurmaId = 1,
                                TurmaCodigo = "TURMA123",
                                ComponenteCurricularId = 1,
                                PeriodoEscolarId = 1,
                                AnoLetivo = 2025,
                                RFProfessor = "123456",
                                ModalidadeCodigo = 1
                            }
                        });

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterProfessoresTitularesDaTurmaCompletosQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<ProfessorTitularDisciplinaEol>
                        {
                            new ProfessorTitularDisciplinaEol
                            {
                                CodigosDisciplinas = "1",
                                ProfessorNome = "Maria",
                                ProfessorRf = "123456"
                            }
                        });

            mediatorMock.Setup(x => x.Send(It.IsAny<PossuiAtribuicaoCJPorTurmaRFQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(false);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<ConsolidarRegistrosPedagogicosCommand>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }
    }
}
