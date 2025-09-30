using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Fechamento
{
    public class AlterarPeriodosComHierarquiaInferiorFechamentoUseCaseTeste
    {
        private readonly Mock<IServicoPeriodoFechamento> _servicoPeriodoFechamentoMock;
        private readonly AlterarPeriodosComHierarquiaInferiorFechamentoUseCase _useCase;

        public AlterarPeriodosComHierarquiaInferiorFechamentoUseCaseTeste()
        {
            _servicoPeriodoFechamentoMock = new Mock<IServicoPeriodoFechamento>();
            var mediatorMock = new Mock<IMediator>();

            _useCase = new AlterarPeriodosComHierarquiaInferiorFechamentoUseCase(mediatorMock.Object, _servicoPeriodoFechamentoMock.Object);
        }

        [Fact]
        public async Task Deve_Executar_Alterar_Periodos_Com_Sucesso()
        {
            var dre = new SME.SGP.Dominio.Dre
            {
                Id = 1,
                Nome = "DRE TESTE"
            };

            var ue = new Ue
            {
                Id = 100,
                Nome = "UE TESTE"
            };

            var periodoEscolar = new PeriodoEscolar
            {
                Id = 999,
                Bimestre = 1
            };

            var fechamentoBimestre = new PeriodoFechamentoBimestre
            {
                Id = 555,
                PeriodoEscolar = periodoEscolar,
                PeriodoEscolarId = periodoEscolar.Id,
                InicioDoFechamento = new DateTime(2025, 3, 1),
                FinalDoFechamento = new DateTime(2025, 3, 31)
            };

            var fechamento = new SME.SGP.Dominio.PeriodoFechamento(dre, ue);
            fechamento.AdicionarFechamentoBimestre(fechamentoBimestre);

            var mensagem = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(fechamento)
            };

            var servicoPeriodoFechamentoMock = new Mock<IServicoPeriodoFechamento>();
            var mediatorMock = new Mock<IMediator>();

            var useCase = new AlterarPeriodosComHierarquiaInferiorFechamentoUseCase(mediatorMock.Object, servicoPeriodoFechamentoMock.Object);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            servicoPeriodoFechamentoMock.Verify(s => s.AlterarPeriodosComHierarquiaInferior(It.Is<SME.SGP.Dominio.PeriodoFechamento>(f => f.DreId == dre.Id && f.UeId == ue.Id)), Times.Once);
        }
    }
}
