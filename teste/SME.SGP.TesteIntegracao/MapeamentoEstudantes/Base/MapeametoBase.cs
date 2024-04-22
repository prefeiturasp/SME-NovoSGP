using Nest;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.MapeamentoEstudantes.Base
{
    public class MapeamentoBase : TesteBaseComuns
    {
        protected const string NOME_COMPONENTE_SECAO_1_MAPEAMENTO_ESTUDANTE = "SECAO_1_MAPEAMENTO_ESTUDANTE";

        protected const string NOME_COMPONENTE_QUESTAO_1 = "PARECER_CONCLUSIVO_ANO_ANTERIOR";
        protected const string NOME_COMPONENTE_QUESTAO_2 = "TURMA_ANO_ANTERIOR";
        protected const string NOME_COMPONENTE_QUESTAO_3 = "ANOTACOES_PEDAG_BIMESTRE_ANTERIOR";
        protected const string NOME_COMPONENTE_QUESTAO_4 = "CLASSIFICADO";
        protected const string NOME_COMPONENTE_QUESTAO_5 = "RECLASSIFICADO";
        protected const string NOME_COMPONENTE_QUESTAO_6 = "MIGRANTE";
        protected const string NOME_COMPONENTE_QUESTAO_7 = "ACOMPANHADO_SRM_CEFAI";
        protected const string NOME_COMPONENTE_QUESTAO_8 = "POSSUI_PLANO_AEE";
        protected const string NOME_COMPONENTE_QUESTAO_9 = "ACOMPANHADO_NAAPA";
        protected const string NOME_COMPONENTE_QUESTAO_10 = "ACOES_REDE_APOIO";
        protected const string NOME_COMPONENTE_QUESTAO_11 = "ACOES_RECUPERACAO_CONTINUA";
        protected const string NOME_COMPONENTE_QUESTAO_12 = "PARTICIPA_PAP";
        protected const string NOME_COMPONENTE_QUESTAO_13 = "PARTICIPA_MAIS_EDUCACAO";
        protected const string NOME_COMPONENTE_QUESTAO_14 = "PROJETO_FORTALECIMENTO_APRENDIZAGENS";
        protected const string NOME_COMPONENTE_QUESTAO_15 = "PROGRAMA_SAO_PAULO_INTEGRAL";
        protected const string NOME_COMPONENTE_QUESTAO_16 = "HIPOTESE_ESCRITA";
        protected const string NOME_COMPONENTE_QUESTAO_17 = "AVALIACOES_EXTERNAS_PROVA_SP";
        protected const string NOME_COMPONENTE_QUESTAO_18 = "OBS_AVALIACAO_PROCESSUAL";
        protected const string NOME_COMPONENTE_QUESTAO_19 = "FREQUENCIA";
        protected const string NOME_COMPONENTE_QUESTAO_20 = "QDADE_REGISTROS_BUSCA_ATIVA";

        public MapeamentoBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {}

        protected async Task CriarDadosBase()
        {
            ExecutarScripts(new List<ScriptCarga> { ScriptCarga.CARGA_QUESTIONARIO_MAPEAMENTO_ESTUDANTE });

            await CriarDreUePerfilComponenteCurricular();

            CriarClaimUsuario(ObterPerfilProfessor());

            await CriarUsuarios();

            await CriarTipoCalendario(Dominio.ModalidadeTipoCalendario.FundamentalMedio, false);

            await CriarTurma(Dominio.Modalidade.Fundamental, ANO_4, false, tipoTurno: 2);

            await CriarPeriodoEscolarCustomizadoQuartoBimestre(true);
        }
    }
}
