using FluentValidation.TestHelper;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Validators
{
    public class FiltroRelatorioFaltasFrequenciaDtoValidatorTeste
    {
        private readonly FiltroRelatorioFaltasFrequenciaDtoValidator _validator;

        public FiltroRelatorioFaltasFrequenciaDtoValidatorTeste()
        {
            _validator = new FiltroRelatorioFaltasFrequenciaDtoValidator();
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
    }
}
