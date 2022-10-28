using System.Threading.Tasks;
using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.RelatorioAcompanhamentoAprendizagem
{
    public class Ao_salvar_percurso_coletivo_imagens_reabertura : RelatorioAcompanhamentoAprendizagemTesteBase
    {
        public Ao_salvar_percurso_coletivo_imagens_reabertura(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        [Fact(DisplayName = "Relatório do Acompanhamento da Aprendizagem - Não Deve Registrar o percurso coletivo para semestre e ano anterior sem reabertura")]
        public async Task Registrar_percurso_coletivo_para_semestre_e_ano_anterior_sem_reabertura()
        {
            await CriarDadosBasicos(abrirPeriodos:false);
            var useCase = ObterSalvarAcompanhamentoUseCase();
            
            var dto = new AcompanhamentoTurmaDto
            {
                TurmaId = TURMA_ID_1,
                Semestre = PRIMEIRO_SEMESTRE,
                ApanhadoGeral = TEXTO_PADRAO_APANHADO_GERAL
            };
            var ex = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));
            ex.ShouldNotBeNull();
            
            var obterTodos = ObterTodos<AcompanhamentoTurma>();
            obterTodos.ShouldNotBeNull();
            obterTodos.Count.ShouldBeEquivalentTo(0);
        }
    }
}