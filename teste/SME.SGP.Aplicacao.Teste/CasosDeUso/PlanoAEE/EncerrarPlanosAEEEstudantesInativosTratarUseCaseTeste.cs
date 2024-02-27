using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PlanoAEE
{
    public class EncerrarPlanosAEEEstudantesInativosTratarUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IUnitOfWork> unitOfWork;
        private readonly EncerrarPlanosAEEEstudantesInativosTratarUseCase useCase;

        public EncerrarPlanosAEEEstudantesInativosTratarUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            unitOfWork = new Mock<IUnitOfWork>();
            useCase = new EncerrarPlanosAEEEstudantesInativosTratarUseCase(mediator.Object, unitOfWork.Object);
        }

        [Fact(DisplayName = "EncerrarPlanosAEEEstudantesInativosTratarUseCase - Deve reaver um plano AEE encerrado indevidamente")]
        public async Task DeveReaverPlanoAeeEncerradoIndevidamente()
        {
            var anoLetivo = DateTimeExtension.HorarioBrasilia().Year;

            mediator.Setup(x => x.Send(It.Is<ObterMatriculasAlunoPorCodigoEAnoQuery>(y => y.CodigoAluno == "1" && y.AnoLetivo == anoLetivo && y.FiltrarSituacao == false), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoPorTurmaResposta>()
                {
                    new() 
                    { 
                        DataSituacao = DateTimeExtension.HorarioBrasilia(), 
                        NumeroAlunoChamada = 1, 
                        CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo, 
                        CodigoTurma = 1 
                    }
                });

            mediator.Setup(x => x.Send(It.Is<ObterTurmaComUeEDrePorIdQuery>(y => y.TurmaId == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma() { CodigoTurma = "1", Ue = new Ue() { CodigoUe = "1" } });

            mediator.Setup(x => x.Send(It.Is<ObterTurmaComUeEDrePorCodigoQuery>(y => y.TurmaCodigo == "1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma() { Ue = new Ue() { CodigoUe = "1" } });

            var planoAee = new Dominio.PlanoAEE() 
            { 
                Id = 1,
                Situacao = Dominio.Enumerados.SituacaoPlanoAEE.EncerradoAutomaticamente, 
                TurmaId = 1, 
                AlunoCodigo = "1"                
            };

            var mensagemRabbit = new MensagemRabbit(JsonConvert.SerializeObject(planoAee));
            var resultado = await useCase.Executar(mensagemRabbit);

            mediator.Verify(x => 
                x.Send(It.Is<PersistirPlanoAEECommand>(y => y.PlanoAEE.Id == planoAee.Id && y.PlanoAEE.Situacao == Dominio.Enumerados.SituacaoPlanoAEE.ParecerCP), It.IsAny<CancellationToken>()), Times.Once);

            mediator.Verify(x =>
                x.Send(It.Is<ReaverPendenciaPlanoAeeQuery>(y => y.PlanoAeeId == planoAee.Id), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(resultado);
        }
    }
}
