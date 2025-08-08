using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Handlers
{
    public class GerarRelatorioCommandHandlerTeste
    {
        [Fact]
        public async Task Deve_Gerar_Relatorio_Com_Sucesso()
        {
            var mediatorMock = new Mock<IMediator>();
            var repositorioCorrelacaoMock = new Mock<IRepositorioCorrelacaoRelatorio>();

            var handler = new GerarRelatorioCommandHandler(mediatorMock.Object, repositorioCorrelacaoMock.Object);

            var usuario = new Usuario
            {
                Id = 123,
                CodigoRf = "123456",
                PerfilAtual = Guid.NewGuid()
            };

            var tipoRelatorio = TipoRelatorio.ConselhoClasseAtaFinal;
            var formato = TipoFormatoRelatorio.Pdf;
            var filtros = new { AnoLetivo = 2024 };
            var rotaRelatorio = "rota/relatorio";

            var command = new GerarRelatorioCommand(
                tipoRelatorio,
                filtros,
                usuario,
                rotaRelatorio,
                formato
            );

            mediatorMock
                .Setup(m => m.Send(It.IsAny<PublicaFilaWorkerServidorRelatoriosCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await handler.Handle(command, CancellationToken.None);

            repositorioCorrelacaoMock.Verify(r => r.Salvar(It.Is<RelatorioCorrelacao>(c =>
                c.TipoRelatorio == tipoRelatorio &&
                c.UsuarioSolicitanteId == usuario.Id &&
                c.Formato == formato
            )), Times.Once);

            mediatorMock.Verify(m => m.Send(It.Is<PublicaFilaWorkerServidorRelatoriosCommand>(c =>
                c.Fila == rotaRelatorio &&
                c.UsuarioLogadoRF == usuario.CodigoRf &&
                c.PerfilUsuario == usuario.PerfilAtual.ToString()      
            ), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(resultado);
        }
    }
}
