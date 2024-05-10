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
    public class AtualizarTurmaDoPlanoAEEUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly AtualizarTurmaDoPlanoAEEUseCase useCase;

        public AtualizarTurmaDoPlanoAEEUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new AtualizarTurmaDoPlanoAEEUseCase(mediator.Object);
        }

        [Fact(DisplayName = "Deve considerar última matrícula valida com datas de situações identicas")]
        public async Task DeveConsiderarUltimaMatriculaValidaComDatasDeSituacoesIdenticas()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            var mesAtual = DateTimeExtension.HorarioBrasilia().Month;
            var dataSituacao = new DateTime(anoAtual, mesAtual, DateTimeExtension.HorarioBrasilia().Day).AddDays(-2);

            var turmasAluno = new List<TurmasDoAlunoDto>()
            {
               new () { CodigoTurma = 1, CodigoTipoTurma = 1, AnoLetivo = anoAtual, DataSituacao = dataSituacao, DataAtualizacaoTabela = dataSituacao  },
               new () { CodigoTurma = 2, CodigoTipoTurma = 1, AnoLetivo = anoAtual, DataSituacao = dataSituacao, DataAtualizacaoTabela = dataSituacao.AddSeconds(1)  }
            };

            mediator.Setup(x => x.Send(It.Is<ObterAlunosEolPorCodigosQuery>(y => y.CodigosAluno[0] == 1 && y.TodasMatriculas), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmasAluno);

            mediator.Setup(x => x.Send(It.Is<ObterTurmaComUeEDrePorCodigoQuery>(y => y.TurmaCodigo == "2"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma() { Id = 2, Ue = new Ue() { CodigoUe = "1" } });

            mediator.Setup(x => x.Send(It.Is<ObterTurmaComUeEDrePorIdQuery>(y => y.TurmaId == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma() { Id = 1, Ue = new Ue() { CodigoUe = "1" } });

            mediator.Setup(x => x.Send(It.Is<ObterPlanoAEEComTurmaPorIdQuery>(y => y.PlanoAEEId == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Dominio.PlanoAEE() { Id = 1, Situacao = Dominio.Enumerados.SituacaoPlanoAEE.Validado });

            mediator.Setup(x => x.Send(It.Is<SalvarPlanoAeeSimplificadoCommand>(y => y.PlanoAEE.Id == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var dto = new PlanoAEETurmaDto() { Id = 1, AlunoCodigo = "1", TurmaId = 1 };
            var retorno = await useCase.Executar(new MensagemRabbit(JsonConvert.SerializeObject(dto)));

            Assert.True(retorno);

            mediator.Verify(x => x.Send(It.Is<SalvarPlanoAeeSimplificadoCommand>(y => y.PlanoAEE.TurmaId == 2), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
