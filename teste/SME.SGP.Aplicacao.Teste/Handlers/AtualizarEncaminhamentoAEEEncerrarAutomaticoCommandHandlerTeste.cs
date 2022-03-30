using Moq;
using SME.SGP.Aplicacao.Commands;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Handlers
{
    public class AtualizarEncaminhamentoAEEEncerrarAutomaticoCommandHandlerTeste
    {
        private readonly Mock<IRepositorioEncaminhamentoAEE> _repositorioEncaminhamentoAEE;
        private readonly AtualizarEncaminhamentoAEEEncerrarAutomaticoCommandHandler _atualizarEncaminhamentoAEEEncerrarAutomaticoCommandHandler;

        public AtualizarEncaminhamentoAEEEncerrarAutomaticoCommandHandlerTeste()
        {
            _repositorioEncaminhamentoAEE = new Mock<IRepositorioEncaminhamentoAEE>();
            _atualizarEncaminhamentoAEEEncerrarAutomaticoCommandHandler = new AtualizarEncaminhamentoAEEEncerrarAutomaticoCommandHandler(_repositorioEncaminhamentoAEE.Object);
        }

        [Fact]
        public async Task Deve_Atualizar_Encaminhamento_AEE_Encerrar_Automaticamente()
        {
            //-> Arrange
            var encaminhamentoAEE = new EncaminhamentoAEE
            { 
                Id = 1983,
                TurmaId = 869773,
                AlunoCodigo = "4824410",
                Excluido = false,
                CriadoEm = DateTime.Now,
                CriadoPor = "PAULO ROBERTO ANTUNES OLIVEIRA MANOEL",
                AlteradoEm = DateTime.Now,
                AlteradoPor = "PAULO ROBERTO ANTUNES OLIVEIRA MANOEL",
                CriadoRF = "8425825",
                AlteradoRF = "8425825",
                Situacao = SituacaoAEE.Rascunho,
                AlunoNome = "NICOLAS DOS SANTOS ALMEIDA SILVA"
            };

            _repositorioEncaminhamentoAEE.Setup(c => c.ObterEncaminhamentoPorId(1983))
                .ReturnsAsync(encaminhamentoAEE);

            _repositorioEncaminhamentoAEE.Setup(c => c.SalvarAsync(It.IsAny<EncaminhamentoAEE>()))
                .ReturnsAsync(1);

            //-> Act
            var retorno = await _atualizarEncaminhamentoAEEEncerrarAutomaticoCommandHandler.Handle(new AtualizarEncaminhamentoAEEEncerrarAutomaticoCommand(1983),
                new CancellationToken());

            //-> Assert
            _repositorioEncaminhamentoAEE.Verify(c => c.SalvarAsync(It.IsAny<EncaminhamentoAEE>()), Times.Once);
            Assert.True(retorno.Situacao.Equals(SituacaoAEE.EncerradoAutomaticamente));
        }
    }
}
