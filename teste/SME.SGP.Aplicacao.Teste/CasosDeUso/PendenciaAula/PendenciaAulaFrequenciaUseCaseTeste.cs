using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class PendenciaAulaFrequenciaUseCaseTeste
    {
        private readonly PendenciaAulaFrequenciaUseCase pendenciaAulaFrequenciaUseCase;
        private readonly Mock<IMediator> mediator;

        public PendenciaAulaFrequenciaUseCaseTeste()
        {

            mediator = new Mock<IMediator>();
            pendenciaAulaFrequenciaUseCase = new PendenciaAulaFrequenciaUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Salvar_Pendencia_Aula_Frequencia()
        {
            // arrange
            var aula = new List<Aula>
            {
                new Aula()
                {
                    Id = 12456,
                    DisciplinaId = "512",
                    Turma = new Turma() { Id = 111, AnoLetivo = 2022, Ano = "1", ModalidadeCodigo = Modalidade.EducacaoInfantil },
                    TurmaId = "111"
                }
            };

            var hoje = DateTime.Today;
            mediator.Setup(a => a.Send(It.IsAny<ObterAulasPendenciaSemFechamentoTurmaDiscplinaProcessadoQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(aula);

            mediator.Setup(a => a.Send(It.IsAny<SalvarPendenciaAulasPorTipoCommand>(), It.IsAny<CancellationToken>()));

            // act
            var salvarPendenciaAulaFrequencia = await pendenciaAulaFrequenciaUseCase.Executar(new MensagemRabbit("{ 'mensagem': { 'dreId': 4, 'ueId': 101,'CodigoUe':'1'} }"));

            // assert
            mediator.Verify(x => x.Send(It.IsAny<SalvarPendenciaAulasPorTipoCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.True(true, "Pendencia do tipo frequência salva com sucesso!");
        }
    }
}
