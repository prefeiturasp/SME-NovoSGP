namespace SME.SGP.Dominio.Constantes.MensagensNegocio
{
    public static class MensagemNegocioConselhoClasse
    {
        public const string CONCEITO_POS_CONSELHO_DEVE_SER_INFORMADO = "O conceito pós-conselho deve ser informado no conselho de classe do aluno.";
        public const string NOTA_POS_CONSELHO_DEVE_SER_INFORMADA = "A nota pós-conselho deve ser informada no conselho de classe do aluno.";
        
        public const string NAO_PERMITE_ACESSO_ABA_FINAL_SEM_CONCLUIR_CONSELHO_BIMESTRE =
            "Para acessar esta aba você precisa concluir o conselho de classe do {0}º bimestre.";

        public const string NAO_PERMITE_ACESSO_ABA_SEM_REGISTRAR_CONSELHO_BIMESTRE =
            "Para acessar este aba você precisa registrar o conselho de classe do {0}º bimestre";

        public const string ALUNO_NAO_ENCONTRADO_PARA_SALVAR_CONSELHO_CLASSE =
            "Aluno não encontrado para salvar o conselho de classe.";

        public const string ALUNO_NAO_POSSUI_CONSELHO_CLASSE_ULTIMO_BIMESTRE =
            "Aluno não possui conselho de classe do último bimestre";

        public const string JA_EXISTE_CONSELHO_CLASSE_GERADO_PARA_TURMA = "Já existe um conselho de classe gerado para a turma {0}!";

        public const string ERRO_ATUALIZAR_SITUACAO_CONSELHO_CLASSE = "Erro ao atualizar situação do conselho de classe";
    }
}