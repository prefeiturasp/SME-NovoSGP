using System.Collections.Generic;

namespace SME.SGP.TesteIntegracao.Constantes
{
    public static class ConstantesQuestionarioBuscaAtiva
    {
        public const long QUESTIONARIO_REGISTRO_ACAO_ID_1 = 1;
        public const long SECAO_REGISTRO_ACAO_ID_1 = 1;
        public const string SECAO_REGISTRO_ACAO_NOME_COMPONENTE = "SECAO_1_REGISTRO_ACAO";

        public const string QUESTAO_1_NOME_COMPONENTE_DATA_REGISTRO_ACAO = "DATA_REGISTRO_ACAO";
        public const string QUESTAO_2_NOME_COMPONENTE_CONSEGUIU_CONTATO_RESP = "CONSEGUIU_CONTATO_RESP";
        public const string QUESTAO_2_3_NOME_COMPONENTE_JUSTIFICATIVA_MOTIVO_FALTA = "JUSTIFICATIVA_MOTIVO_FALTA";
        public const string QUESTAO_2_3_1_NOME_COMPONENTE_JUSTIFICATIVA_MOTIVO_FALTA_OUTROS = "JUSTIFICATIVA_MOTIVO_FALTA_OUTROS";
        public const string QUESTAO_2_4_NOME_COMPONENTE_PROCEDIMENTO_REALIZADO = "PROCEDIMENTO_REALIZADO";
        public const string QUESTAO_2_4_1_NOME_COMPONENTE_QUESTOES_OBS_DURANTE_VISITA = "QUESTOES_OBS_DURANTE_VISITA";
        public const string QUESTAO_2_5_NOME_COMPONENTE_OBS_GERAL = "OBS_GERAL";
        public const string QUESTAO_2_2_NOME_COMPONENTE_OBS_GERAL_NAO_CONTATOU_RESP = "OBS_GERAL_NAO_CONTATOU_RESP";

        public const long QUESTAO_1_ID_DATA_REGISTRO_ACAO = 1;
        public const long QUESTAO_2_ID_CONSEGUIU_CONTATO_RESP = 2;
        public const long QUESTAO_2_3_ID_JUSTIFICATIVA_MOTIVO_FALTA = 4;
        public const long QUESTAO_2_3_1_ID_JUSTIFICATIVA_MOTIVO_FALTA_OUTROS = 5;
        public const long QUESTAO_2_4_ID_PROCEDIMENTO_REALIZADO = 3;
        public const long QUESTAO_2_4_1_ID_QUESTOES_OBS_DURANTE_VISITA = 6;
        public const long QUESTAO_2_5_ID_OBS_GERAL = 7;
        public const long QUESTAO_2_2_ID_OBS_GERAL_NAO_CONTATOU_RESP = 8;

        public const string QUESTAO_OPCAO_RESPOSTA_SIM = "Sim";
        public const string QUESTAO_JUSTIFICATIVA_MOTIVO_FALTA_RESPOSTA_OUTROS = "Outros";
        public const string QUESTAO_PROCEDIMENTO_REALIZADO_RESPOSTA_VISITA_DOMICILIAR = "Visita Domiciliar";
        public const string QUESTAO_OPCAO_RESPOSTA_NAO = "Não";
        public const string QUESTAO_PROCEDIMENTO_REALIZADO_RESPOSTA_LIG_TELEFONICA = "Ligação telefonica";
        public const int QUESTAO_PROCEDIMENTO_REALIZADO_ORDEM_RESPOSTA_LIG_TELEFONICA = 1;

        public static IEnumerable<(long id, string descricao)> ObterOpcoesRespostas_JUSTIFICATIVA_MOTIVO_FALTA()
        {
            var opcoesRespostas = new List<(long id, string descricao)>();
            opcoesRespostas.Add((1, "Estudante com questões de saúde mental (depressão, ansiedade, etc.)"));
            opcoesRespostas.Add((2, "Estudante em luto por familiar/ responsável falecido"));
            opcoesRespostas.Add((3, "Estudante com doenças crônicas (diabete, câncer, doença do coração, epilepsia, etc.)"));
            opcoesRespostas.Add((4, "Estudante está doente (enjoo, diarreia, vômito, gripe, resfriado, etc.)"));
            opcoesRespostas.Add((5, "Estudante grávida"));
            opcoesRespostas.Add((6, "Estudante é pessoa com deficiência"));
            opcoesRespostas.Add((7, "Estudante está cuidando de familiares"));
            opcoesRespostas.Add((8, "Trabalho infantil (vende bala no farol, pede coisas, cuida de outras crianças, responsável por trabalho doméstico)"));
            opcoesRespostas.Add((9, "Mora na rua"));
            opcoesRespostas.Add((10, "Não tem moradia fixa (ficando temporariamente em casas diferentes)"));
            opcoesRespostas.Add((11, "Responsável justifica que não tem material escolar/uniforme"));
            opcoesRespostas.Add((12, "Falta de transporte escolar"));
            opcoesRespostas.Add((13, "Família em situação de extrema pobreza"));
            opcoesRespostas.Add((14, "Responsável justifica a ausência da estudante por questões relacionadas à menstruação"));
            opcoesRespostas.Add((15, "Estudante não deseja voltar para a escola"));
            opcoesRespostas.Add((16, "Não há um responsável para levar para a escola"));
            opcoesRespostas.Add((17, "Responsável não compreende a obrigatoriedade da frequência escolar"));
            opcoesRespostas.Add((18, "Responsável não quer levar para a escola"));
            opcoesRespostas.Add((19, "Responsável é pessoa com deficiência"));
            opcoesRespostas.Add((20, "Mora distante da escola e apresenta dificuldades para o deslocamento"));
            opcoesRespostas.Add((21, "Violência no território (comunidade/ bairro)"));
            opcoesRespostas.Add((22, "Estudante é vítima de preconceito/ discriminação/ bullying na escola"));
            opcoesRespostas.Add((23, "Estudante faleceu"));
            opcoesRespostas.Add((24, "Adolescente na Fundação Casa (medidas socioeducativas, aplicáveis a adolescentes envolvidos na prática de um ato infracional)"));
            opcoesRespostas.Add((25, "Responsável informa que o bebê/criança/adolescente perdeu a vaga"));
            opcoesRespostas.Add((26, "Viagem"));
            opcoesRespostas.Add((27, "Outros"));

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
