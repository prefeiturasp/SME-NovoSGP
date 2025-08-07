using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DiarioBordoPendenciaDevolutiva
{
    public class ReprocessarDiarioBordoPendenciaDevolutivaPorTurmaUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ReprocessarDiarioBordoPendenciaDevolutivaPorTurmaUseCase useCase;

        public ReprocessarDiarioBordoPendenciaDevolutivaPorTurmaUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ReprocessarDiarioBordoPendenciaDevolutivaPorTurmaUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_True_E_Publicar_Para_Cada_Turma()
        {
            var filtro = new FiltroDiarioBordoPendenciaDevolutivaDto(anoLetivo: 2025, dreId: 1, ueCodigo: "UE123", ueId: 10);
            var mensagemJson = JsonConvert.SerializeObject(filtro);
            var mensagemRabbit = new MensagemRabbit(mensagemJson);

            var turmas = new List<long> { 100, 200 };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterCodigosTurmasPorAnoModalidadeUeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmas);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagemRabbit);

            Assert.True(resultado);

            mediatorMock.Verify(m => m.Send(It.Is<ObterCodigosTurmasPorAnoModalidadeUeQuery>(q =>
                q.AnoLetivo == filtro.AnoLetivo &&
                q.Modalidade == Modalidade.EducacaoInfantil &&
                q.UeId == filtro.UeId), It.IsAny<CancellationToken>()), Times.Once);

            mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(cmd =>
                cmd.Rota == RotasRabbitSgp.RotaReprocessarDiarioBordoPendenciaDevolutivaPorComponente &&
                VerificarFiltroDiarioBordoPendencia(cmd.Filtros, filtro, turmas)), It.IsAny<CancellationToken>()), Times.Exactly(turmas.Count));
        }     

        [Fact]
        public async Task Executar_Deve_Retornar_False_E_Salvar_Log_Quando_Exception()
        {
            var mensagemRabbit = new MensagemRabbit("{}");

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterCodigosTurmasPorAnoModalidadeUeQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro de teste"));

            mediatorMock
                .Setup(m => m.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagemRabbit);

            Assert.False(resultado);

            mediatorMock.Verify(m => m.Send(It.Is<SalvarLogViaRabbitCommand>(cmd =>
                cmd.Mensagem.Contains("Não foi possível executar a verificação") &&
                cmd.Nivel == LogNivel.Critico &&
                cmd.Contexto == LogContexto.Devolutivas &&
                cmd.Observacao == "Erro de teste"), It.IsAny<CancellationToken>()), Times.Once);
        }
        private bool VerificarFiltroDiarioBordoPendencia(object filtros, FiltroDiarioBordoPendenciaDevolutivaDto filtroEsperado, List<long> turmasEsperadas)
        {
            if (filtros is not FiltroDiarioBordoPendenciaDevolutivaDto filtroCmd)
                return false;

            bool turmaValida = turmasEsperadas.Contains(filtroCmd.TurmaId);

            return filtroCmd.AnoLetivo == filtroEsperado.AnoLetivo
                && filtroCmd.DreId == filtroEsperado.DreId
                && filtroCmd.UeCodigo == filtroEsperado.UeCodigo
                && turmaValida
                && filtroCmd.UeId == filtroEsperado.UeId;
        }
    }
}
