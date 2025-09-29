using MediatR;
using Moq;
using SME.SGP.Aplicacao.Commands;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Commands
{
    public class ExcluirRegistroAcaoCommandHandlerTeste
    {
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<IRepositorioRegistroAcaoBuscaAtiva> _repositorio;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly ExcluirRegistroAcaoCommandHandler _handler;

        public ExcluirRegistroAcaoCommandHandlerTeste()
        {
            _mediator = new Mock<IMediator>();
            _repositorio = new Mock<IRepositorioRegistroAcaoBuscaAtiva>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _handler = new ExcluirRegistroAcaoCommandHandler(_mediator.Object, _repositorio.Object, _unitOfWork.Object);
        }

        [Fact]
        public async Task Deve_Retornar_False_Quando_Excluir_Falhar()
        {
            _repositorio.Setup(r => r.ObterCodigoArquivoPorRegistroAcaoId(1)).ReturnsAsync(new List<string>());

            var resultado = await _handler.Handle(new ExcluirRegistroAcaoCommand(1), CancellationToken.None);

            Assert.False(resultado);
        }

        [Fact]
        public async Task Deve_Retornar_False_Quando_Nao_Excluir()
        {
            _repositorio.Setup(r => r.ObterCodigoArquivoPorRegistroAcaoId(1)).ReturnsAsync(new List<string>());

            var resultado = await _handler.Handle(new ExcluirRegistroAcaoCommand(1), CancellationToken.None);

            Assert.False(resultado);
        }
    }
}