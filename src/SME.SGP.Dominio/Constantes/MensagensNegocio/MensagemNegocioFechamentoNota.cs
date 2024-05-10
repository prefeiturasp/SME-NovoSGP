namespace SME.SGP.Dominio.Constantes.MensagensNegocio
{
    public class MensagemNegocioFechamentoNota
    {
        protected MensagemNegocioFechamentoNota() { }

        public const string REGISTRADO_COM_SUCESSO_EM_24_HORAS_SERA_ENVIADO_PARA_APROVACAO =
            "{0} registrados com sucesso.Em até 24 horas será enviado para aprovação e será considerado válido após a aprovação do último nível.";

        public const string EXISTEM_ALUNOS_INATIVOS_FECHAMENTO_NOTA_BIMESTRE =
            "Existe(m) aluno(s) inativo(s) no fechamento das notas do bimestre.";

        public const string ALUNO_INATIVO_ANTES_PERIODO_REABERTURA = "Existe(m) aluno(s) inativados fora do periodo de reabertura.";
        public const string ALUNO_INATIVO_ANTES_PERIODO_ESCOLAR = "Existe(m) aluno(s) inativados fora do periodo escolar.";

        public const string EXISTE_COMPONENTES_SEM_NOTA_INFORMADA =
            "Existe(m) componente(s) curriculares sem a nota informada.";

        public const string FECHAMENTO_TURMA_NAO_LOCALIZADO = "Fechamento da turma não localizado";

        public const string NOTA_ALUNO_NAO_PODE_SER_INSERIDA_OU_ALTERADA_NO_PERIODO = "A nota do aluno não pode ser alterada ou incluída no periodo.";
    }
}
