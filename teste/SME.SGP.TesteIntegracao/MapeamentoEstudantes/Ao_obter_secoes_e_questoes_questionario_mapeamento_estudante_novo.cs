using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
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
    public class Ao_obter_secoes_e_questoes_questionario_mapeamento_estudante_novo : MapeamentoBase
    {

        public Ao_obter_secoes_e_questoes_questionario_mapeamento_estudante_novo(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            if (!collectionFixture.DatabasePublicado)
            {
                Task.Run(() => CriarDadosBase()).Wait();
                collectionFixture.DatabasePublicado = true;
            }
        }

        [Fact(DisplayName = "Mapeamento Estudantes - Listar as seções")]
        public async Task Ao_listar_secoes_mapeamento_estudantes()
        {
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
            var useCase = ServiceProvider.GetService<IObterQuestionarioMapeamentoEstudanteUseCase>();
            var retorno = await useCase.Executar(1, null);
            retorno.ShouldNotBeNull();
            retorno.Count().ShouldBe(20);
            retorno.Any(q => q.NomeComponente.Equals(NOME_COMPONENTE_QUESTAO_1) 
                             && q.TipoQuestao.Equals(TipoQuestao.ComboDinamico)).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NOME_COMPONENTE_QUESTAO_2)
                             && q.TipoQuestao.Equals(TipoQuestao.Frase)).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NOME_COMPONENTE_QUESTAO_3)
                             && q.TipoQuestao.Equals(TipoQuestao.Texto)).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NOME_COMPONENTE_QUESTAO_4)
                             && q.TipoQuestao.Equals(TipoQuestao.Radio)).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NOME_COMPONENTE_QUESTAO_5)
                             && q.TipoQuestao.Equals(TipoQuestao.Radio)).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NOME_COMPONENTE_QUESTAO_6)
                             && q.TipoQuestao.Equals(TipoQuestao.Radio)).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NOME_COMPONENTE_QUESTAO_7)
                             && q.TipoQuestao.Equals(TipoQuestao.Radio)).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NOME_COMPONENTE_QUESTAO_8)
                             && q.TipoQuestao.Equals(TipoQuestao.Radio)).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NOME_COMPONENTE_QUESTAO_9)
                             && q.TipoQuestao.Equals(TipoQuestao.Radio)).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NOME_COMPONENTE_QUESTAO_10)
                             && q.TipoQuestao.Equals(TipoQuestao.EditorTexto)).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NOME_COMPONENTE_QUESTAO_11)
                             && q.TipoQuestao.Equals(TipoQuestao.EditorTexto)).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NOME_COMPONENTE_QUESTAO_12)
                             && q.TipoQuestao.Equals(TipoQuestao.ComboMultiplaEscolhaDinamico)).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NOME_COMPONENTE_QUESTAO_13)
                             && q.TipoQuestao.Equals(TipoQuestao.ComboMultiplaEscolhaDinamico)).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NOME_COMPONENTE_QUESTAO_14)
                             && q.TipoQuestao.Equals(TipoQuestao.ComboMultiplaEscolhaDinamico)).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NOME_COMPONENTE_QUESTAO_15)
                             && q.TipoQuestao.Equals(TipoQuestao.Radio)).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NOME_COMPONENTE_QUESTAO_16)
                             && q.TipoQuestao.Equals(TipoQuestao.Frase)).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NOME_COMPONENTE_QUESTAO_17)
                             && q.TipoQuestao.Equals(TipoQuestao.AvaliacoesExternasProvaSP)).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NOME_COMPONENTE_QUESTAO_18)
                             && q.TipoQuestao.Equals(TipoQuestao.EditorTexto)).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NOME_COMPONENTE_QUESTAO_19)
                             && q.TipoQuestao.Equals(TipoQuestao.Combo)).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NOME_COMPONENTE_QUESTAO_20)
                             && q.TipoQuestao.Equals(TipoQuestao.Numerico)).ShouldBeTrue();

        }
    
    }
}