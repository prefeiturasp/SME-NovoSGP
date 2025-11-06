using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Autenticacao
{
    public class ObterGuidAutenticacaoFrequenciaTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IServicoAutenticacao> _servicoAutenticacaoMock;
        private readonly ObterGuidAutenticacaoFrequencia _useCase;

        public ObterGuidAutenticacaoFrequenciaTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _servicoAutenticacaoMock = new Mock<IServicoAutenticacao>();
            _useCase = new ObterGuidAutenticacaoFrequencia(_mediatorMock.Object, _servicoAutenticacaoMock.Object);
        }

        private Turma CriarTurmaMock()
        {
            return new Turma
            {
                Ano = "1",
                AnoLetivo = 2024,
                CodigoTurma = "F123",
                Id = 10,
                ModalidadeCodigo = Modalidade.Fundamental,
                Nome = "1A",
                Semestre = 1,
                TipoTurma = TipoTurma.Regular,
                TipoTurno = 1,
                Historica = false,
                UeId = 20,
                Ue = new Ue
                {
                    Id = 20,
                    CodigoUe = "U100",
                    Nome = "EMEF Teste",
                    TipoEscola = Dominio.TipoEscola.EMEF,
                    DreId = 30,
                    Dre = new Dominio.Dre
                    {
                        Id = 30,
                        CodigoDre = "D90",
                        Nome = "DRE Teste",
                        Abreviacao = "DR-E"
                    }
                }
            };
        }

        private (UsuarioAutenticacaoRetornoDto UsuarioAutenticacaoRetornoDto, string CodigoRf, IEnumerable<Guid> Perfis, bool PossuiCargoCJ, bool PossuiPerfilCJ) CriarRetornoAutenticacao(bool autenticado)
        {
            return (
                new UsuarioAutenticacaoRetornoDto { Autenticado = autenticado },
                "123456",
                new List<Guid>(),
                false,
                false
            );
        }

        private SolicitacaoGuidAutenticacaoFrequenciaDto CriarFiltro(string rf, string turmaCodigo, string componente)
        {
            return new SolicitacaoGuidAutenticacaoFrequenciaDto
            {
                Rf = rf,
                TurmaCodigo = turmaCodigo,
                ComponenteCurricularCodigo = componente
            };
        }

        [Fact]
        public async Task Executar_Quando_Sucesso_Deve_Autenticar_ObterTurma_E_Salvar_Cache()
        {
            var filtro = CriarFiltro("123456", "F123", "01");
            var retornoAutenticacao = CriarRetornoAutenticacao(true);
            var turma = CriarTurmaMock();

            _servicoAutenticacaoMock.Setup(s => s.AutenticarNoEolSemSenha(filtro.Rf)).ReturnsAsync(retornoAutenticacao);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaComUeEDrePorCodigoQuery>(c => c.TurmaCodigo == filtro.TurmaCodigo), It.IsAny<CancellationToken>())).ReturnsAsync(turma);
            _mediatorMock.Setup(m => m.Send(It.IsAny<SalvarCachePorValorObjetoCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync("");

            var resultado = await _useCase.Executar(filtro);

            Assert.NotEqual(Guid.Empty, resultado);
            _servicoAutenticacaoMock.Verify(s => s.AutenticarNoEolSemSenha(filtro.Rf), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterTurmaComUeEDrePorCodigoQuery>(c => c.TurmaCodigo == filtro.TurmaCodigo), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<SalvarCachePorValorObjetoCommand>(c => c.NomeChave.StartsWith("autenticacao-frequencia:")), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Autenticacao_Eol_Falha_Deve_Lancar_NegocioException_Unauthorized()
        {
            var filtro = CriarFiltro("123456", "F123", "01");
            var retornoAutenticacao = CriarRetornoAutenticacao(false);

            _servicoAutenticacaoMock.Setup(s => s.AutenticarNoEolSemSenha(filtro.Rf)).ReturnsAsync(retornoAutenticacao);

            var ex = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(filtro));

            Assert.Equal((int)HttpStatusCode.Unauthorized, ex.StatusCode);
            _servicoAutenticacaoMock.Verify(s => s.AutenticarNoEolSemSenha(filtro.Rf), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()), Times.Never);
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarCachePorValorObjetoCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Turma_Nao_Encontrada_Deve_Lancar_NegocioException()
        {
            var filtro = CriarFiltro("123456", "F123", "01");
            var retornoAutenticacao = CriarRetornoAutenticacao(true);
            Turma turmaNula = null;

            _servicoAutenticacaoMock.Setup(s => s.AutenticarNoEolSemSenha(filtro.Rf)).ReturnsAsync(retornoAutenticacao);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(turmaNula);

            await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(filtro));

            _servicoAutenticacaoMock.Verify(s => s.AutenticarNoEolSemSenha(filtro.Rf), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterTurmaComUeEDrePorCodigoQuery>(c => c.TurmaCodigo == filtro.TurmaCodigo), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarCachePorValorObjetoCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public void Executar_Quando_Construtor_ServicoAutenticacao_Nulo_Deve_Lancar_ArgumentNullException()
        {
           Assert.Throws<ArgumentNullException>(() => new ObterGuidAutenticacaoFrequencia(_mediatorMock.Object, null));
        }

        [Fact]
        public void Executar_Quando_Construtor_Mediator_Nulo_Deve_Lancar_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterGuidAutenticacaoFrequencia(null, _servicoAutenticacaoMock.Object));
        }
    }
}