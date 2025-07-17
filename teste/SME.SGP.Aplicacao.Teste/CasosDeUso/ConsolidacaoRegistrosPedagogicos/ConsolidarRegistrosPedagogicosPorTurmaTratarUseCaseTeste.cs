using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConsolidacaoRegistrosPedagogicos
{
    public class ConsolidarRegistrosPedagogicosPorTurmaTratarUseCaseTeste
    {
        [Fact]
        public async Task Executar_Deve_Consolidar_Registros_Quando_Mensagem_Valida()
        {
            var mediatorMock = new Mock<IMediator>();

            var turmaCodigo = "12345";
            var anoLetivo = 2025;
            var professorRf = "123456";
            var professorNome = "Prof. Fulano";

            var professorEol = new ProfessorTitularDisciplinaEol
            {
                ProfessorRf = professorRf,
                ProfessorNome = professorNome,
                CodigosDisciplinas = "123"
            };

            var filtro = new FiltroConsolidacaoRegistrosPedagogicosPorTurmaDto(turmaCodigo, anoLetivo, new[] { professorEol });

            var mensagem = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(filtro)
            };

            var consolidacao = new ConsolidacaoRegistrosPedagogicosDto
            {
                TurmaId = 1,
                PeriodoEscolarId = 10,
                AnoLetivo = anoLetivo,
                ComponenteCurricularId = 123,
                ModalidadeCodigo = (int)Modalidade.EducacaoInfantil,
                QuantidadeAulas = 20,
                FrequenciasPendentes = 0,
                DiarioBordoPendentes = 0,
                PlanoAulaPendentes = 0
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterConsolidacaoRegistrosComSeparacaoDiarioBordoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new[] { consolidacao });

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ConsolidarRegistrosPedagogicosCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            var useCase = new ConsolidarRegistrosPedagogicosPorTurmaTratarUseCase(mediatorMock.Object);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);

            mediatorMock.Verify(m => m.Send(It.Is<ObterConsolidacaoRegistrosComSeparacaoDiarioBordoQuery>(
                q => q.TurmaCodigo == turmaCodigo &&
                     q.AnoLetivo == anoLetivo &&
                     q.ComponentesCurricularesIds.Any(c => c == 123)
            ), It.IsAny<CancellationToken>()), Times.Once);

            mediatorMock.Verify(m => m.Send(It.Is<ConsolidarRegistrosPedagogicosCommand>(
                c => c.ConsolidacaoRegistrosPedagogicos.ComponenteCurricularId == 123 &&
                     c.ConsolidacaoRegistrosPedagogicos.NomeProfessor == professorNome &&
                     c.ConsolidacaoRegistrosPedagogicos.RFProfessor == professorRf
            ), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
