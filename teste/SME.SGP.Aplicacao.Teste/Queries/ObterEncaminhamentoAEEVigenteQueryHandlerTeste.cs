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
                EncaminhamentoId = 1,
                AlunoCodigo = "111",
                TurmaId = 2,
                TurmaCodigo = "222",
                AnoLetivo = 2022,
                UeId = 3,
                UeCodigo = "333"
            };

            var listaRetorno = new List<EncaminhamentoAEEVigenteDto>
            {
                encaminhamentoAEEVigente
            };

            _repositorioEncaminhamentoAEE.Setup(c => c.ObterEncaminhamentosVigentes(null))
                .ReturnsAsync(listaRetorno);

            //-> Act
            var retorno = await _obterEncaminhamentoAEEVigenteQueryHandler.Handle(new ObterEncaminhamentoAEEVigenteQuery(), new CancellationToken());

            //-> Assert
            Assert.True(retorno.Any(), "Existe encaminhamento AEE vigente.");
        }
    }
}
