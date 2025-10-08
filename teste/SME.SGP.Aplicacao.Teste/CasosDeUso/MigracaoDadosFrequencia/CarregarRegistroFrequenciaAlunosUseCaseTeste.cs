using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.MigracaoDadosFrequencia
{
    public class CarregarRegistroFrequenciaAlunosUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly CarregarRegistroFrequenciaAlunosUseCase _useCase;

        public CarregarRegistroFrequenciaAlunosUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new CarregarRegistroFrequenciaAlunosUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Todos_Registros_Existem_Nao_Deve_Publicar_Na_Fila()
        {
            var dadosAula = new MigracaoFrequenciaTurmaAulaDto("T1", 1L, 2, 10L, new[] { "A1", "A2" });
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(dadosAula) };

            var frequenciasExistentes = new List<FrequenciaAlunoSimplificadoDto>
            {
                new FrequenciaAlunoSimplificadoDto { CodigoAluno = "A1", NumeroAula = 1 },
                new FrequenciaAlunoSimplificadoDto { CodigoAluno = "A1", NumeroAula = 2 },
                new FrequenciaAlunoSimplificadoDto { CodigoAluno = "A2", NumeroAula = 1 },
                new FrequenciaAlunoSimplificadoDto { CodigoAluno = "A2", NumeroAula = 2 }
            };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterRegistrosFrequenciasAlunosSimplificadoPorAulaIdQuery>(q => q.AulaId == dadosAula.AulaId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(frequenciasExistentes);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Existem_Registros_Faltantes_Deve_Criar_E_Publicar_Na_Fila()
        {
            var dadosAula = new MigracaoFrequenciaTurmaAulaDto("T1", 1L, 2, 10L, new[] { "A1", "A2" });
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(dadosAula) };

            var frequenciasExistentes = new List<FrequenciaAlunoSimplificadoDto>
            {
                new FrequenciaAlunoSimplificadoDto { CodigoAluno = "A1", NumeroAula = 1 }
            };

            ParametroFrequenciasPersistirDto payloadPublicado = null;

            _mediatorMock.Setup(m => m.Send(It.Is<ObterRegistrosFrequenciasAlunosSimplificadoPorAulaIdQuery>(q => q.AulaId == dadosAula.AulaId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(frequenciasExistentes);

            _mediatorMock.Setup(m => m.Send(It.Is<PublicarFilaSgpCommand>(c => c.Rota == RotasRabbitSgpFrequencia.SincronizarDadosAlunosFrequenciaMigracao), It.IsAny<CancellationToken>()))
                         .Callback<IRequest<bool>, CancellationToken>((cmd, token) =>
                         {
                             var command = cmd as PublicarFilaSgpCommand;
                             payloadPublicado = command.Filtros as ParametroFrequenciasPersistirDto;
                         })
                         .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.NotNull(payloadPublicado);
            Assert.Equal(3, payloadPublicado.FrequenciasPersistir.Count);
            Assert.Contains(payloadPublicado.FrequenciasPersistir, f => f.CodigoAluno == "A1" && f.NumeroAula == 2);
            Assert.Contains(payloadPublicado.FrequenciasPersistir, f => f.CodigoAluno == "A2" && f.NumeroAula == 1);
            Assert.Contains(payloadPublicado.FrequenciasPersistir, f => f.CodigoAluno == "A2" && f.NumeroAula == 2);
        }
    }
}
