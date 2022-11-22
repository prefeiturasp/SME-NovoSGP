namespace SME.SGP.Dominio.Constantes.MensagensNegocio
{
    public class MensagemNegocioEncaminhamentoNAAPA
    {
        public const string ESTUDANTE_JA_POSSUI_ENCAMINHAMENTO_NAAPA_EM_ABERTO = "Estudante/Criança já possui encaminhamento NAAPA em aberto";
        
        public const string ENCAMINHAMENTO_NAO_ENCONTRADO = "Encaminhamento não encontrado";
        
        public const string ENCAMINHAMENTO_NAO_PODE_SER_EXCLUIDO_PELO_USUARIO_LOGADO = "Encaminhamentos só podem ser excluídos pelos gestores da UE ou pelo professor criador do encaminhamento";
        
        public const string EXISTEM_QUESTOES_OBRIGATORIAS_NAO_PREENCHIDAS = "Existem questões obrigatórias não preenchidas no Encaminhamento NAAPA: {0}";

        public const string ENCAMINHAMENTO_NAO_PODE_SER_EXCLUIDO_NESSA_SITUACAO = "Encaminhamentos só podem ser excluídos nas situações: 'Rascunho' ou 'Encaminhado'";
    }
}