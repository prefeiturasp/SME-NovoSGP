using FluentValidation.TestHelper;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Armazenamento
{
    public class ExcluirArquivosUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ExcluirArquivosUseCase _useCase;
        private readonly ObterArquivosPorCodigosQueryValidator _validator;

        public ExcluirArquivosUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExcluirArquivosUseCase(_mediatorMock.Object);
            _validator = new ObterArquivosPorCodigosQueryValidator();
        }

        [Fact]
        public async Task Executar_Nenhum_Arquivo_Encontrado_Deve_Lancar_Negocio_Exception()
        {
            var codigos = new[] { Guid.NewGuid() };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterArquivosPorCodigosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<Arquivo>());

            var ex = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(codigos));
            Assert.Equal(MensagemNegocioComuns.NENHUM_ARQUIVO_ENCONTRADO, ex.Message);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterArquivosPorCodigosQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Arquivos_Encontrados_Deve_Excluir_E_Publicar_Na_Fila()
        {
            var codigos = new[] { Guid.NewGuid(), Guid.NewGuid() };
            var arquivos = new List<Arquivo>
            {
                new Arquivo { Id = 1, Codigo = codigos[0], Nome = "arquivo1.txt" },
                new Arquivo { Id = 2, Codigo = codigos[1], Nome = "arquivo2.pdf" }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterArquivosPorCodigosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(arquivos);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ExcluirArquivosRepositorioPorIdsCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await _useCase.Executar(codigos);

            Assert.True(resultado);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterArquivosPorCodigosQuery>(q => q.Codigos.SequenceEqual(codigos)), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.Is<ExcluirArquivosRepositorioPorIdsCommand>(c =>
                c.Ids.Count == arquivos.Count &&
                c.Ids.Contains(1) &&
                c.Ids.Contains(2)), It.IsAny<CancellationToken>()), Times.Once);

            foreach (var arquivo in arquivos)
            {
                var ext = Path.GetExtension(arquivo.Nome);
                var esperado = arquivo.Codigo + ext;

                _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(cmd =>
                    cmd.Rota == RotasRabbitSgp.RemoverArquivoArmazenamento &&
                    ((FiltroExcluirArquivoArmazenamentoDto)cmd.Filtros).ArquivoNome == esperado
                ), It.IsAny<CancellationToken>()), Times.Once);
            }
        }

        [Fact]
        public void Deve_Retornar_Erro_Quando_Codigos_For_Null()
        {
            var query = new ObterArquivosPorCodigosQuery(null);

            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(q => q.Codigos)
                .WithErrorMessage("Os códigos dos arquivos devem ser informados para obter seus dados.");
        }

        [Fact]
        public void Nao_Deve_Retornar_Erro_Quando_Codigos_For_Valido()
        {
            var query = new ObterArquivosPorCodigosQuery(new[] { Guid.NewGuid() });

            var result = _validator.TestValidate(query);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
