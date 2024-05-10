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
                Id = 1,
                TurmaId = 111,
                AlunoCodigo = "123",
                Excluido = false,
                CriadoEm = DateTime.Now,
                CriadoPor = "TESTE",
                AlteradoEm = DateTime.Now,
                AlteradoPor = "TESTE",
                CriadoRF = "999",
                AlteradoRF = "999",
                Situacao = SituacaoAEE.Rascunho,
                AlunoNome = "ALUNO TESTE"
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
