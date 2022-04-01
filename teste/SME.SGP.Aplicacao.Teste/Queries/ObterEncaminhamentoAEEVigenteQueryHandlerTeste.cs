using Moq;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries
{
    public class ObterEncaminhamentoAEEVigenteQueryHandlerTeste
    {
        private readonly Mock<IRepositorioEncaminhamentoAEE> _repositorioEncaminhamentoAEE;
        private readonly ObterEncaminhamentoAEEVigenteQueryHandler _obterEncaminhamentoAEEVigenteQueryHandler;

        public ObterEncaminhamentoAEEVigenteQueryHandlerTeste()
        {
            _repositorioEncaminhamentoAEE = new Mock<IRepositorioEncaminhamentoAEE>();
            _obterEncaminhamentoAEEVigenteQueryHandler = new ObterEncaminhamentoAEEVigenteQueryHandler(_repositorioEncaminhamentoAEE.Object);
        }

        [Fact]
        public async Task Deve_Obter_Encaminhamentos_AEE_Vigentes()
        {
            //-> Arrange
            var encaminhamentoAEEVigente = new EncaminhamentoAEEVigenteDto()
            {
                EncaminhamentoId = 1983,
                AlunoCodigo = "4824410",
                TurmaId = 869773,
                TurmaCodigo = "2369048",
                AnoLetivo = 2022,
                UeId = 276,
                UeCodigo = "094668"
            };

            var listaRetorno = new List<EncaminhamentoAEEVigenteDto>
            {
                encaminhamentoAEEVigente
            };

            _repositorioEncaminhamentoAEE.Setup(c => c.ObterEncaminhamentosVigentes())
                .ReturnsAsync(listaRetorno);

            //-> Act
            var retorno = await _obterEncaminhamentoAEEVigenteQueryHandler.Handle(new ObterEncaminhamentoAEEVigenteQuery(), new CancellationToken());

            //-> Assert
            Assert.True(retorno.Any(), "Existe encaminhamento AEE vigente.");
        }
    }
}
