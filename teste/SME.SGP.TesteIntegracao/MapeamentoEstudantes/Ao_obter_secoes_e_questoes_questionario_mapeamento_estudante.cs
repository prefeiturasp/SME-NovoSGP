using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Constantes;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.MapeamentoEstudante;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Constantes;
using SME.SGP.TesteIntegracao.MapeamentoEstudantes.Base;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.MapeamentoEstudantes
{
    public class Ao_obter_secoes_e_questoes_questionario_mapeamento_estudante : MapeamentoBase
    {

        public Ao_obter_secoes_e_questoes_questionario_mapeamento_estudante(CollectionFixture collectionFixture) : base(collectionFixture)
        {}

        [Fact(DisplayName = "Mapeamento Estudantes - Listar as seções")]
        public async Task Ao_listar_secoes_mapeamento_estudantes()
        {
            await CriarDadosBase();
            var useCase = ServiceProvider.GetService<IObterSecoesMapeamentoSecaoUseCase>();
            var retorno = await useCase.Executar(null);
            retorno.ShouldNotBeNull();
            retorno.Count().ShouldBe(1);
            var secao = retorno.FirstOrDefault();
            secao.NomeComponente.ShouldBe(NOME_COMPONENTE_SECAO_1_MAPEAMENTO_ESTUDANTE);
            secao.Concluido.ShouldBeFalse();
            secao.Auditoria.ShouldBe(null);
        }

        [Fact(DisplayName = "Mapeamento Estudantes - Listar questões por questionário")]
        public async Task Ao_listar_questoes_questionario()
        {
            await CriarDadosBase();
            var useCase = ServiceProvider.GetService<IObterQuestionarioMapeamentoEstudanteUseCase>();
            var retorno = await useCase.Executar(new FiltroQuestoesQuestionarioMapeamentoEstudanteDto() { QuestionarioId = 1, MapeamentoEstudanteId = 1});
            retorno.ShouldNotBeNull();
            retorno.Count().ShouldBe(19);
            retorno.Any(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PARECER_CONCLUSIVO_ANO_ANTERIOR) 
                             && q.TipoQuestao.Equals(TipoQuestao.ComboDinamico)
                             && q.Obrigatorio).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.TURMA_ANO_ANTERIOR)
                             && q.TipoQuestao.Equals(TipoQuestao.Frase)
                             && q.Obrigatorio).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.ANOTACOES_PEDAG_BIMESTRE_ANTERIOR)
                             && q.TipoQuestao.Equals(TipoQuestao.Texto)
                             && !q.Obrigatorio).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.DISTORCAO_IDADE_ANO_SERIE)
                             && q.TipoQuestao.Equals(TipoQuestao.Radio)
                             && q.Obrigatorio).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.MIGRANTE)
                             && q.TipoQuestao.Equals(TipoQuestao.Radio)
                             && q.Obrigatorio).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.ACOMPANHADO_SRM_CEFAI)
                             && q.TipoQuestao.Equals(TipoQuestao.Radio)
                             && q.Obrigatorio).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.POSSUI_PLANO_AEE)
                             && q.TipoQuestao.Equals(TipoQuestao.Radio)
                             && q.Obrigatorio).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.ACOMPANHADO_NAAPA)
                             && q.TipoQuestao.Equals(TipoQuestao.Radio)
                             && q.Obrigatorio).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.ACOES_REDE_APOIO)
                             && q.TipoQuestao.Equals(TipoQuestao.EditorTexto)
                             && !q.Obrigatorio).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.ACOES_RECUPERACAO_CONTINUA)
                             && q.TipoQuestao.Equals(TipoQuestao.EditorTexto)
                             && !q.Obrigatorio).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PARTICIPA_PAP)
                             && q.TipoQuestao.Equals(TipoQuestao.ComboMultiplaEscolhaDinamico)
                             && q.Obrigatorio).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PARTICIPA_MAIS_EDUCACAO)
                             && q.TipoQuestao.Equals(TipoQuestao.ComboMultiplaEscolhaDinamico)
                             && q.Obrigatorio).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PROJETO_FORTALECIMENTO_APRENDIZAGENS)
                             && q.TipoQuestao.Equals(TipoQuestao.ComboMultiplaEscolhaDinamico)
                             && q.Obrigatorio).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PROGRAMA_SAO_PAULO_INTEGRAL)
                             && q.TipoQuestao.Equals(TipoQuestao.Radio)
                             && q.Obrigatorio).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.HIPOTESE_ESCRITA)
                             && q.TipoQuestao.Equals(TipoQuestao.Frase)
                             && !q.Obrigatorio).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.AVALIACOES_EXTERNAS_PROVA_SP)
                             && q.TipoQuestao.Equals(TipoQuestao.AvaliacoesExternasProvaSP)
                             && !q.Obrigatorio).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.OBS_AVALIACAO_PROCESSUAL)
                             && q.TipoQuestao.Equals(TipoQuestao.EditorTexto)
                             && q.Obrigatorio).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.FREQUENCIA)
                             && q.TipoQuestao.Equals(TipoQuestao.Combo)
                             && q.Obrigatorio).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.QDADE_REGISTROS_BUSCA_ATIVA)
                             && q.TipoQuestao.Equals(TipoQuestao.Numerico)
                             && q.Obrigatorio).ShouldBeTrue();

        }
    
    }
}