using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ReflexoFrequencia
{
    public class ConsolidarReflexoFrequenciaBuscaAtivaAlunoUseCaseTeste
    {
        [Fact]
        public async Task Executar_Deve_Chamar_Med_Queries_E_Commands_Quando_Dados_Forem_Validos()
        {
            var mediatorMock = new Mock<IMediator>();
            var repositorioBuscaAtivaMock = new Mock<IRepositorioRegistroAcaoBuscaAtiva>();

            var useCase = new ConsolidarReflexoFrequenciaBuscaAtivaAlunoUseCase(mediatorMock.Object, repositorioBuscaAtivaMock.Object);

            var filtro = new FiltroIdAnoLetivoDto(123, new DateTime(2025, 5, 10));

            var mensagemRabbit = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(filtro)
            };

            var buscaAtivaDto = new RegistroAcaoBuscaAtivaAlunoDto
            {
                AlunoCodigo = "1111",
                AlunoNome = "João",
                AnoLetivo = 2025,
                DataBuscaAtiva = new DateTime(2025, 5, 5),
                Modalidade = Modalidade.Fundamental,
                TurmaCodigo = "TURMA1",
                UeCodigo = "UE1"
            };

            var freqAntes = new RegistroFrequenciaAlunoPorTurmaEMesDto { QuantidadeAulas = 50, QuantidadeAusencias = 0, QuantidadeCompensacoes = 0 };
            var freqDepois = new RegistroFrequenciaAlunoPorTurmaEMesDto { QuantidadeAulas = 50, QuantidadeAusencias = 0, QuantidadeCompensacoes = 0 };

            repositorioBuscaAtivaMock.Setup(x => x.ObterRegistroBuscaAtivaAluno(filtro.Id))
                .ReturnsAsync(buscaAtivaDto);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciaMensalPorTurmaMesAlunoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(freqAntes); // para simplificação, retornamos a mesma frequência antes e depois

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarConsolidacaoReflexoFrequenciaBuscaAtivaCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var resultado = await useCase.Executar(mensagemRabbit);

            Assert.True(resultado);

            repositorioBuscaAtivaMock.Verify(x => x.ObterRegistroBuscaAtivaAluno(filtro.Id), Moq.Times.Once);

            mediatorMock.Verify(m => m.Send(It.IsAny<ObterFrequenciaMensalPorTurmaMesAlunoQuery>(), It.IsAny<CancellationToken>()), Moq.Times.Exactly(4)); // 2 mensal + 2 anual
            mediatorMock.Verify(m => m.Send(It.IsAny<SalvarConsolidacaoReflexoFrequenciaBuscaAtivaCommand>(), It.IsAny<CancellationToken>()), Moq.Times.Exactly(2));
        }
    }
}
