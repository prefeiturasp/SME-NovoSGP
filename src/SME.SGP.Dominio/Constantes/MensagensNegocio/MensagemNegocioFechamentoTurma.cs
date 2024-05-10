namespace SME.SGP.Dominio.Constantes.MensagensNegocio
{
    public class MensagemNegocioFechamentoTurma
    {
        protected MensagemNegocioFechamentoTurma() { }

        public const string NAO_EXISTE_FECHAMENTO_TURMA =
            "Não existe fechamento para a turma";

        public const string FECHAMENTO_TURMA_NAO_LOCALIZADO_BIMESTRE =
          "Fechamento da turma não localizado para o bimestre {0}";

        public const string FECHAMENTO_TURMA_NAO_LOCALIZADO =
          "Fechamento da turma não localizado";

        public const string TURMA_NAO_ESTA_EM_PERIODO_FECHAMENTO_PARA_BIMESTRE =
            "Turma {0} não esta em período de fechamento para o {1}º Bimestre!";

    }
}
