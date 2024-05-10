namespace SME.SGP.Dominio.Constantes.MensagensNegocio
{
    public class MensagemNegocioPlanoAula
    {
        protected MensagemNegocioPlanoAula() { }

        public const string NAO_EXISTE_PLANO_ANUAL_CADASTRADO = "Não foi possível concluir o cadastro, pois não existe plano anual cadastrado";

        public const string NAO_FOI_LOCALIZADO_BIMESTRE_DA_AULA =
            "Não foi possível concluir o cadastro, pois não foi localizado o bimestre da aula.";

        public const string OBRIGATORIO_SELECIONAR_OBJETIVOS_APRENDIZAGEM =
            "A seleção de objetivos de aprendizagem é obrigatória para criação do plano de aula";
    }
}