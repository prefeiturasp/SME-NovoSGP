namespace SME.SGP.Dominio.Constantes.MensagensNegocio
{
    public class MensagemNegocioEncaminhamentoNAAPA
    {
        public const string ENCAMINHAMENTO_NAO_ENCONTRADO = "Encaminhamento não encontrado";              
        public const string EXISTEM_QUESTOES_OBRIGATORIAS_NAO_PREENCHIDAS = "Existem questões obrigatórias não preenchidas no Encaminhamento NAAPA: {0}";
        public const string ENCAMINHAMENTO_NAO_PODE_SER_EXCLUIDO_NESSA_SITUACAO = "Encaminhamentos só podem ser excluídos nas situações: 'Rascunho' ou 'Cadastrado'";
        public const string SITUACAO_ENCAMINHAMENTO_DEVE_SER_DIFERENTE_RASCUNHO = "A situação do encaminhamento deve ser diferente de rascunho";
        public const string ENCAMINHAMENTO_NAO_PODE_SER_REABERTO_NESTA_SITUACAO = "Encaminhamentos só podem ser reabertos na situação: 'Encerrado'";
        public const string ENCAMINHAMENTO_ALUNO_INATIVO_NAO_PODE_SER_REABERTO = "Encaminhamentos com aluno inativo não podem ser reabertos";
        public const string SECAO_NAO_ENCONTRADA = "Seção não encontrada";
        public const string SECAO_NAO_VALIDA_ITINERANCIA = "Seção inválida para o lançamento de atendimentos!";
    }
}