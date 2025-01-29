using FluentValidation.TestHelper;
using Moq;
using SME.SGP.Infra;
using SME.SGP.Dominio;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Validators
{
    public class FiltroRelatorioFaltasFrequenciaDtoValidatorTeste
    {
        private readonly FiltroRelatorioFaltasFrequenciaDtoValidator _validator;
        private readonly Mock<IMediator> _mediator;
        private readonly GerarRelatorioFrequenciaUseCase _useCase;

        public FiltroRelatorioFaltasFrequenciaDtoValidatorTeste()
        {
            _validator = new FiltroRelatorioFaltasFrequenciaDtoValidator();
            _mediator = new Mock<IMediator>();
            _useCase = new GerarRelatorioFrequenciaUseCase(_mediator.Object);
        }

        private FiltroRelatorioFrequenciaDto CriarFiltroPadrao()
        {
            return new FiltroRelatorioFrequenciaDto
            {
                CodigoRf = "123123",
                AnoLetivo = 2024,
                Modalidade = Modalidade.Fundamental,
                TipoRelatorio = TipoRelatorioFaltasFrequencia.Turma,
                TipoFormatoRelatorio = TipoFormatoRelatorio.Pdf,
                Bimestres = new List<int> { 1, 2 }
            };
        }

        [Fact]
        public void Deve_Falhar_Quando_TipoRelatorio_Nao_For_Informado()
        {
            var filtro = CriarFiltroPadrao();
            filtro.TipoRelatorio = 0;

            var resultado = _validator.TestValidate(filtro);

            resultado.ShouldHaveValidationErrorFor(f => f.TipoRelatorio);
        }

        [Fact]
        public void Deve_Falhar_Quando_Modalidade_Nao_For_Informada()
        {
            var filtro = CriarFiltroPadrao();
            filtro.Modalidade = 0;

            var resultado = _validator.TestValidate(filtro);

            resultado.ShouldHaveValidationErrorFor(f => f.Modalidade);
        }

        [Fact]
        public void Deve_Falhar_Quando_EJA_Nao_Tiver_Semestre()
        {
            var filtro = CriarFiltroPadrao();
            filtro.Modalidade = Modalidade.EJA;
            filtro.Semestre = 0;

            var resultado = _validator.TestValidate(filtro);

            resultado.ShouldHaveValidationErrorFor(f => f.Semestre);
        }

        [Fact]
        public void Deve_Falhar_Quando_Bimestres_Nao_For_Informado_Para_Modalidade_Normal()
        {
            var filtro = CriarFiltroPadrao();
            filtro.Bimestres = null;

            var resultado = _validator.TestValidate(filtro);

            resultado.ShouldHaveValidationErrorFor(f => f.Bimestres);
        }

        [Fact]
        public void Deve_Passar_Quando_Todas_As_Informacoes_Sao_Corretas()
        {
            var filtro = CriarFiltroPadrao();

            var resultado = _validator.TestValidate(filtro);

            resultado.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Deve_Publicar_Mensagem_Para_Fila()
        {

            var filtro = CriarFiltroPadrao();

            _mediator.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario { CodigoRf = "123123", Nome = "Usuário Teste" });

            _mediator.Setup(m => m.Send(It.IsAny<GerarRelatorioCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await _useCase.Executar(filtro);
            
            _mediator.Verify(m => m.Send(It.IsAny<GerarRelatorioCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.True(resultado);
        }
    }
}
