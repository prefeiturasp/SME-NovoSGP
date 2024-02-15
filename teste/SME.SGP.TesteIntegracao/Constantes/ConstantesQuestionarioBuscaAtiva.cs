using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Constantes
{
    public static class ConstantesQuestionarioBuscaAtiva
    {
        public const long QUESTIONARIO_REGISTRO_ACAO_ID_1 = 1;
        public const long SECAO_REGISTRO_ACAO_ID_1 = 1;
        public const string SECAO_REGISTRO_ACAO_NOME_COMPONENTE = "SECAO_1_REGISTRO_ACAO";

        public const string QUESTAO_1_NOME_COMPONENTE_DATA_REGISTRO_ACAO = "DATA_REGISTRO_ACAO";
        public const string QUESTAO_2_NOME_COMPONENTE_CONSEGUIU_CONTATO_RESP = "CONSEGUIU_CONTATO_RESP";
        public const string QUESTAO_2_1_NOME_COMPONENTE_CONTATO_COM_RESPONSAVEL = "CONTATO_COM_RESPONSAVEL";
        public const string QUESTAO_2_2_NOME_COMPONENTE_APOS_CONTATO_CRIANCA_RETORNOU_ESCOLA = "APOS_CONTATO_CRIANCA_RETORNOU_ESCOLA";
        public const string QUESTAO_2_3_NOME_COMPONENTE_JUSTIFICATIVA_MOTIVO_FALTA = "JUSTIFICATIVA_MOTIVO_FALTA";
        public const string QUESTAO_2_3_1_NOME_COMPONENTE_JUSTIFICATIVA_MOTIVO_FALTA_OUTROS = "JUSTIFICATIVA_MOTIVO_FALTA_OUTROS";
        public const string QUESTAO_2_4_NOME_COMPONENTE_PROCEDIMENTO_REALIZADO = "PROCEDIMENTO_REALIZADO";
        public const string QUESTAO_2_1_NOME_COMPONENTE_PROCEDIMENTO_REALIZADO_NAO_CONTATOU_RESP = "PROCEDIMENTO_REALIZADO_NAO_CONTATOU_RESP";
        public const string QUESTAO_2_4_1_NOME_COMPONENTE_QUESTOES_OBS_DURANTE_VISITA = "QUESTOES_OBS_DURANTE_VISITA";
        public const string QUESTAO_3_NOME_COMPONENTE_OBS_GERAL = "OBS_GERAL";

        public const long QUESTAO_1_ID_DATA_REGISTRO_ACAO = 1;
        public const long QUESTAO_2_ID_CONSEGUIU_CONTATO_RESP = 2;
        public const long QUESTAO_2_1_ID_CONTATO_COM_RESPONSAVEL = 4;
        public const long QUESTAO_2_2_ID_APOS_CONTATO_CRIANCA_RETORNOU_ESCOLA = 5;
        public const long QUESTAO_2_3_ID_JUSTIFICATIVA_MOTIVO_FALTA = 6;
        public const long QUESTAO_2_3_1_ID_JUSTIFICATIVA_MOTIVO_FALTA_OUTROS = 7;
        public const long QUESTAO_2_4_ID_PROCEDIMENTO_REALIZADO = 3;
        public const long QUESTAO_2_4_1_ID_QUESTOES_OBS_DURANTE_VISITA = 8;
        public const long QUESTAO_3_ID_OBS_GERAL = 9;
        public const long QUESTAO_2_1_ID_PROCEDIMENTO_REALIZADO_NAO_CONTATOU_RESP = 10;

        public const string QUESTAO_OPCAO_RESPOSTA_SIM = "Sim";
        public const string QUESTAO_JUSTIFICATIVA_MOTIVO_FALTA_RESPOSTA_OUTROS = "Outros";
        public const string QUESTAO_PROCEDIMENTO_REALIZADO_RESPOSTA_VISITA_DOMICILIAR = "Visita Domiciliar";
        public const string QUESTAO_OPCAO_RESPOSTA_NAO = "Não";
        public const string QUESTAO_PROCEDIMENTO_REALIZADO_RESPOSTA_LIG_TELEFONICA = "Ligação telefonica";
        public const int QUESTAO_PROCEDIMENTO_REALIZADO_ORDEM_RESPOSTA_LIG_TELEFONICA = 1;

        public static IEnumerable<(long id, string descricao)> ObterOpcoesRespostas_JUSTIFICATIVA_MOTIVO_FALTA()
        {
            var opcoesRespostas = new List<(long id, string descricao)>();
            opcoesRespostas.Add((1, "Ausência por gripe ou resfriado (tosse, febre, dor de garganta)"));
            opcoesRespostas.Add((2, "Ausência por enjôo, diarreia, vômito"));
            opcoesRespostas.Add((3, "Ausência por doenças crônicas como anemia, diabetes, câncer, problemas cardíacos ou neurológicos, convulsões ou transplantados"));
            opcoesRespostas.Add((4, "Ausência por questões de diagnóstico de transtorno mental ou em sofrimento psíquico (depressão, ansiedade)"));
            opcoesRespostas.Add((5, "Ausência por deficiência que impeça ou dificulte o acesso e permanência à Unidade Educacional"));
            opcoesRespostas.Add((6, "Ausência do adolescente por motivo de cumprimento de medidas socioeducativas em regime fechado"));
            opcoesRespostas.Add((7, "Ausência do adolescente por motivo de cumprimento de medidas socioeducativas em casa"));
            opcoesRespostas.Add((8, "Ausência por estarem viajando no período"));
            opcoesRespostas.Add((9, "Ausência porque mora distante da escola e apresente dificuldades no deslocamento"));
            opcoesRespostas.Add((10, "Ausência por estarem cuidando de irmãos, pais ou avós"));
            opcoesRespostas.Add((11, "Ausência por motivo de falecimento"));
            opcoesRespostas.Add((12, "Há suspeita de ausência por estar realizando trabalho infantil"));
            opcoesRespostas.Add((13, "Ausência por motivo de gravidez da estudante"));
            opcoesRespostas.Add((14, "Ausência por relato do estudante que não deseja voltar para a escola"));
            opcoesRespostas.Add((15, "Ausência por não ter material escolar/uniforme"));
            opcoesRespostas.Add((16, "Ausência por falta de transporte escolar"));
            opcoesRespostas.Add((17, "Ausência por negligência da família sobre a frequência escolar (não sabe/não se preocupa/não se importa)"));
            opcoesRespostas.Add((18, "Ausência por estar em situação de rua ou na rua"));
            opcoesRespostas.Add((19, "Ausência por enfrentar dificuldades financeiras"));
            opcoesRespostas.Add((20, "Ausência por não ter moradia fixa"));
            opcoesRespostas.Add((21, "Ausência por ter sido vitima de preconceito, discriminação ou bullyng na unidade educacional"));
            opcoesRespostas.Add((22, "Ausência pelo estudante estar em luto"));
            opcoesRespostas.Add((23, "Ausência por não haver um responsável para levar para a escola"));
            opcoesRespostas.Add((24, "Ausência por ter perdido a vaga"));
            opcoesRespostas.Add((25, "Ausência devido aos seus responsáveis serem pessoas com deficiência e/ou apresentarem problemas de saúde mental ou dependência química (alcoolismo, drogas, medicamentos)"));
            opcoesRespostas.Add((26, "Ausência porque os responsáveis não querem levar o bebê/criança/adolescente para a unidade educacional"));
            opcoesRespostas.Add((27, "Ausência por envolvimento do estudante com ácool, drogas ou medicamentos"));
            opcoesRespostas.Add((28, "Ausência devido a violência do território (comunidade, bairro)"));
            opcoesRespostas.Add((29, "Outros"));
            return opcoesRespostas;
        }

        public static IEnumerable<(long id, string descricao)> ObterOpcoesRespostas_QUESTOES_OBS_DURANTE_VISITA()
        {
            var opcoesRespostas = new List<(long id, string descricao)>();
            opcoesRespostas.Add((1, "Há suspeita de negligência"));
            opcoesRespostas.Add((2, "Há suspeita de violência física"));
            opcoesRespostas.Add((3, "Há suspeita/relato de violência sexual"));
            return opcoesRespostas;
        }
    }
}
