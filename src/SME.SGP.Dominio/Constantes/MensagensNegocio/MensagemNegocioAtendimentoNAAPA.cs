namespace SME.SGP.Dominio.Constantes.MensagensNegocio
{
    public class MensagemNegocioAtendimentoNAAPA
    {
        protected MensagemNegocioAtendimentoNAAPA() { }

        public const string ATENDIMENTO_NAO_ENCONTRADO = "Atendimento não encontrado";              
        public const string EXISTEM_QUESTOES_OBRIGATORIAS_NAO_PREENCHIDAS = "Existem questões obrigatórias não preenchidas no Atendimento NAAPA: {0}";
        public const string ATENDIMENTO_NAO_PODE_SER_EXCLUIDO_NESSA_SITUACAO = "Atendimentos só podem ser excluídos nas situações: 'Rascunho' ou 'Cadastrado'";
        public const string SITUACAO_ATENDIMENTO_DEVE_SER_DIFERENTE_RASCUNHO = "A situação do atendimento deve ser diferente de rascunho";
        public const string ATENDIMENTO_NAO_PODE_SER_REABERTO_NESTA_SITUACAO = "Atendimentos só podem ser reabertos na situação: 'Encerrado'";
        public const string ATENDIMENTO_ALUNO_INATIVO_NAO_PODE_SER_REABERTO = "Atendimentos com aluno inativo não podem ser reabertos";
        public const string SECAO_NAO_ENCONTRADA = "Seção não encontrada";
        public const string SECAO_NAO_VALIDA_ITINERANCIA = "Seção inválida para o lançamento de atendimentos!";
        public const string EXISTE_ATENDIMENTO_ATIVO_PARA_ALUNO = "Existe atendimento ativo para o aluno";
    }
}