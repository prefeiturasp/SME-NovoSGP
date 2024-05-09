using MediatR;
using Microsoft.Win32;
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
                Situacao = SituacaoPlanoAEE.EncerradoAutomaticamente, 
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

        [Fact(DisplayName = "EncerrarPlanosAEEEstudantesInativosTratarUseCase - Deve considerar corretamente a turma do aluno com registros de matriculas com data de situação identicas")]
        public async Task DeveConsiderarCorretamenteTurmaAlunoComRegistrosMatriculasComDataSituacaoIdenticas()
        {
            var dataAtual = DateTimeExtension.HorarioBrasilia();

            var matriculas = new List<AlunoPorTurmaResposta>()
            {
                new() { CodigoTurma = 1, CodigoTipoTurma = (int)TipoTurma.Regular, DataSituacao = dataAtual, DataAtualizacaoTabela = dataAtual, CodigoSituacaoMatricula = SituacaoMatriculaAluno.RemanejadoSaida },
                new() { CodigoTurma = 2, CodigoTipoTurma = (int)TipoTurma.Regular, DataSituacao = dataAtual, DataAtualizacaoTabela = dataAtual.AddMilliseconds(5), CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo }
            };

            mediator.Setup(x => x.Send(It.Is<ObterMatriculasAlunoPorCodigoEAnoQuery>(y => y.CodigoAluno == "1" && y.AnoLetivo == dataAtual.Year && !y.FiltrarSituacao), It.IsAny<CancellationToken>()))
                .ReturnsAsync(matriculas);

            mediator.Setup(x => x.Send(It.Is<ObterTurmaComUeEDrePorIdQuery>(y => y.TurmaId == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma() { Ue = new Ue() { CodigoUe = "1" } });

            mediator.Setup(x => x.Send(It.Is<ObterTurmaComUeEDrePorCodigoQuery>(y => y.TurmaCodigo == "2"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma() { Ue = new Ue() { CodigoUe = "2" } });

            var planoAee = new Dominio.PlanoAEE()
            {
                Id = 1,
                Situacao = SituacaoPlanoAEE.Expirado,
                TurmaId = 1,
                AlunoCodigo = "1"
            };

            var mensagemRabbit = new MensagemRabbit(JsonConvert.SerializeObject(planoAee));
            var resultado = await useCase.Executar(mensagemRabbit);

            Assert.True(resultado);

            mediator.Verify(x => 
                x.Send(It.Is<PersistirPlanoAEECommand>(y => y.PlanoAEE.Id == 1 && y.PlanoAEE.AlunoCodigo == "1" && y.PlanoAEE.TurmaId == 1), It.IsAny<CancellationToken>()), Times.Once);

            mediator.Verify(x => 
                x.Send(It.Is<ResolverPendenciaPlanoAEECommand>(y => y.PlanoAEEId == 1), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
