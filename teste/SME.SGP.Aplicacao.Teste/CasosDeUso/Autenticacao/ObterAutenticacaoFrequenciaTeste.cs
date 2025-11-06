using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Autenticacao
{
    public class ObterAutenticacaoFrequenciaTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IComandosUsuario> _comandoUsuarioMock;
        private readonly Mock<IRepositorioCache> _repositorioCacheMock;
        private readonly ObterAutenticacaoFrequencia _useCase;

        public ObterAutenticacaoFrequenciaTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _comandoUsuarioMock = new Mock<IComandosUsuario>();
            _repositorioCacheMock = new Mock<IRepositorioCache>();
            _useCase = new ObterAutenticacaoFrequencia(_mediatorMock.Object, _comandoUsuarioMock.Object, _repositorioCacheMock.Object);
        }

        private AutenticacaoFrequenciaDto CriarAutenticacaoFrequenciaDto(string rf, string componenteCodigo, TurmaUeDreDto turmaDto)
        {
            var usuarioAutenticacao = new UsuarioAutenticacaoRetornoDto
            {
                Autenticado = true,
                UsuarioRf = rf,
                PerfisUsuario = new PerfisPorPrioridadeDto()
            };

            return new AutenticacaoFrequenciaDto
            {
                Rf = rf,
                ComponenteCurricularCodigo = componenteCodigo,
                Turma = turmaDto,
                UsuarioAutenticacao = (usuarioAutenticacao, rf, new List<Guid>(), false, false)
            };
        }

        private TurmaUeDreDto CriarTurmaUeDreDto(string codigoTurma)
        {
            return new TurmaUeDreDto { Codigo = codigoTurma, Nome = "Turma Teste" };
        }

        private UsuarioAutenticacaoRetornoDto CriarUsuarioAutenticacaoRetornoDto(string rf)
        {
            return new UsuarioAutenticacaoRetornoDto
            {
                Autenticado = true,
                UsuarioRf = rf,
                Token = "token_teste",
                PerfisUsuario = new PerfisPorPrioridadeDto()
            };
        }     

        [Fact]
        public async Task Executar_Quando_Cache_Nulo_Ou_Vazio_Deve_Lancar_NegocioException()
        {
            var guid = Guid.NewGuid();
            var chaveCacheEsperada = string.Format(NomeChaveCache.AUTENTICACAO_FREQUENCIA, guid);

            _repositorioCacheMock.Setup(r => r.ObterAsync(chaveCacheEsperada, false)).ReturnsAsync((string)null);

            await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(guid));
            _repositorioCacheMock.Verify(r => r.ObterAsync(chaveCacheEsperada, false), Times.Once);
            _comandoUsuarioMock.Verify(c => c.ObterAutenticacao(It.IsAny<ValueTuple<UsuarioAutenticacaoRetornoDto, string, System.Collections.Generic.IEnumerable<Guid>, bool, bool>>(), It.IsAny<string>(), null), Times.Never);

            _repositorioCacheMock.Setup(r => r.ObterAsync(chaveCacheEsperada, false)).ReturnsAsync(string.Empty);

            await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(guid));
            _repositorioCacheMock.Verify(r => r.ObterAsync(chaveCacheEsperada, false), Times.Exactly(2));
            _comandoUsuarioMock.Verify(c => c.ObterAutenticacao(It.IsAny<ValueTuple<UsuarioAutenticacaoRetornoDto, string, System.Collections.Generic.IEnumerable<Guid>, bool, bool>>(), It.IsAny<string>(), null), Times.Never);

            _repositorioCacheMock.Setup(r => r.ObterAsync(chaveCacheEsperada, false)).ReturnsAsync(" ");

            await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(guid));
            _repositorioCacheMock.Verify(r => r.ObterAsync(chaveCacheEsperada, false), Times.Exactly(3));
            _comandoUsuarioMock.Verify(c => c.ObterAutenticacao(It.IsAny<(UsuarioAutenticacaoRetornoDto, string, System.Collections.Generic.IEnumerable<Guid>, bool, bool)>(), It.IsAny<string>(), null), Times.Never);

            _comandoUsuarioMock.Setup(c => c.ObterAutenticacao(
                It.IsAny<(UsuarioAutenticacaoRetornoDto, string, System.Collections.Generic.IEnumerable<Guid>, bool, bool)>(),
                It.IsAny<string>(), null))
                .ReturnsAsync((UsuarioAutenticacaoRetornoDto)null);
        }

        [Fact]
        public async Task Executar_Quando_Construtor_Argumentos_Nulos_Deve_Lancar_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterAutenticacaoFrequencia(null, _comandoUsuarioMock.Object, _repositorioCacheMock.Object));
            Assert.Throws<ArgumentNullException>(() => new ObterAutenticacaoFrequencia(_mediatorMock.Object, null, _repositorioCacheMock.Object));
            Assert.Throws<ArgumentNullException>(() => new ObterAutenticacaoFrequencia(_mediatorMock.Object, _comandoUsuarioMock.Object, null));
        }
    }
}