namespace SME.SGP.Dominio.Constantes.MensagensNegocio
{
    public class MensagemNegocioEncaminhamentoAee
    {
        protected MensagemNegocioEncaminhamentoAee() { }

        public const string ESTUDANTE_JA_POSSUI_ENCAMINHAMENTO_AEE_EM_ABERTO =
            "Estudante/Criança já possui encaminhamento AEE em aberto";

        public const string ENCAMINHAMENTO_SO_PODEM_SER_DEVOLVIDOS_NA_SITUACAO_ENCAMINHADO = "Encaminhamento só podem ser devolvidos na situação 'Encaminhado'";

        public const string NAO_FORAM_ENCONTRADOS_DADOS_DE_NECESSIDADE_ESPECIAL = "Não foram encontrados dados de necessidades especiais para o aluno no EOL";
        public const string ENCAMINHAMENTO_NAO_ENCONTRADO = "Encaminhamento não encontrado";
        public const string ENCAMINHAMENTO_NAO_PODE_SER_EXCLUIDO_NESSA_SITUACAO = "Encaminhamento só podem ser excluídos nas situações: 'Rascunho' ou 'Encaminhado'";
        public const string ENCAMINHAMENTO_NAO_PODE_SER_EXCLUIDO_PELO_USUARIO_LOGADO = "Encaminhamento só podem ser excluídos pelos gestores da ue ou pelo professor criador do encaminhamento";
        public const string ENCAMINHAMENTO_SO_PODEM_SER_DEVOLVIDO_PELA_GESTAO = "Encaminhamento só podem ser devolvidos por gestores da escola";
        public const string EXISTEM_QUESTOES_OBRIGATORIAS_NAO_PREENCHIDAS = "Existem questões obrigatórias não preenchidas no Encaminhamento AEE: {0}";
        public const string SOMENTE_GESTOR_ESCOLAR_PODE_REALIZAR_INDEFERIMENTO = "Somente gestor escolar pode realizar indeferimento";
        public const string SOMENTE_USUARIO_PAAE_OU_PAEE_PODE_CONCLUIR_O_ENCAMINHAMENTO = "Somente usuário paai ou paee pode concluir o encaminhamento";
        public const string NAO_LOCALIZADO_A_SECAO_ETAPA_3_NO_ENCAMINHAMENTO_AEE = "Não localizado a seção da Etapa 3 no encaminhamento AEE";
        public const string QUESTAO_NAO_LOCALIZADO_PARA_IDENTIFICAR_SE_ESTUDANTE_CRIANCA_NECESSITA_ATENDIMENTO = "Questão não localizada para identificar se o estudante/criaNça necessita do Atendimento Educacional Especializado";
        public const string NAO_LOCALIZADO_RESPOSTA_PARA_IDENTIFICAR_SE_ESTUDANTE_CRIANCA_NECESSITA_ATENDIMENTO = "Não localizada resposta para identificar se o estudante/criaça necessita do Atendimento Educacional Especializado";
    }
}