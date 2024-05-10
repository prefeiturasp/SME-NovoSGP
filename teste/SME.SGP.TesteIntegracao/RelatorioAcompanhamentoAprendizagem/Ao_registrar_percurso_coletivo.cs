using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.RelatorioAcompanhamentoAprendizagem
{
    public class Ao_registrar_percurso_coletivo : RelatorioAcompanhamentoAprendizagemTesteBase
    {
        public Ao_registrar_percurso_coletivo(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Theory(DisplayName= "Relatorio Acompanhamento Aprendizagem - Ao registrar percurso coletivo dos semestres")]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Ao_registrar_percurso_coletivo_dos_semestres(int semestre)
        {
            await CriarDadosBasicos();
            await CriarPeriodoEscolar(DateTimeExtension.HorarioBrasilia().Date, DateTimeExtension.HorarioBrasilia().AddMonths(2), semestre * 2);
            var acompanhamento = $"Registro de percurso coletivo semestre {semestre}";
            var useCase = ObterSalvarAcompanhamentoUseCase();
            var dto = new AcompanhamentoTurmaDto()
            {
                TurmaId = TURMA_ID_1,
                Semestre = semestre,
                ApanhadoGeral = acompanhamento
            };
            var registro = await useCase.Executar(dto);

            registro.ShouldNotBeNull();
            registro.ApanhadoGeral.ShouldBe(acompanhamento);
            registro.TurmaId.ShouldBe(TURMA_ID_1);
        }

        [Fact(DisplayName = "Relatorio Acompanhamento Aprendizagem -  Ao registrar percurso coletivo com período fechado")]
        public async Task Ao_registrar_percurso_coletivo_com_periodo_fechado() 
        {
            await CriarDadosBasicos(false);
            await CriarPeriodoEscolarCustomizadoQuartoBimestre();

            var useCase = ObterSalvarAcompanhamentoUseCase();
            var dto = new AcompanhamentoTurmaDto()
            {
                TurmaId = TURMA_ID_1,
                Semestre = SEGUNDO_SEMESTRE,
                ApanhadoGeral = "Registro de percurso coletivo semestre"
            };

            await useCase.Executar(dto)
                    .ShouldThrowAsync<NegocioException>(MensagemAcompanhamentoTurma.PERIODO_NAO_ESTA_ABERTO);
        }

        [Fact(DisplayName = "Relatorio Acompanhamento Aprendizagem - Ao registrar percurso coletivo com período em reabertura")]
        public async Task Ao_registrar_percurso_coletivo_com_periodo_em_reabertura()
        {
            await CriarDadosBasicos(false);
            await CriarPeriodoEscolarCustomizadoQuartoBimestre();
            await CriarPeriodoAberturaCustomizadoQuartoBimestre();

            var acompanhamento = $"Registro de percurso coletivo semestre";
            var useCase = ObterSalvarAcompanhamentoUseCase();
            var dto = new AcompanhamentoTurmaDto()
            {
                TurmaId = TURMA_ID_1,
                Semestre = SEGUNDO_SEMESTRE,
                ApanhadoGeral = acompanhamento
            };

            var registro = await useCase.Executar(dto);

            registro.ShouldNotBeNull();
            registro.ApanhadoGeral.ShouldBe(acompanhamento);
            registro.TurmaId.ShouldBe(TURMA_ID_1);
        }
    }
}
