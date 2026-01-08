namespace SME.SGP.Dominio.Constantes.MensagensNegocio
{
    public class MensagemNegocioPlanoAee
    {
        protected MensagemNegocioPlanoAee() { }

        public const string Plano_aee_nao_encontrado = "Plano AEE não encontrado!";
        public const string PLANO_AEE_ENCERRAMENTO_MANUAL_NAO_PERMITIDO = "O encerramento manual só é permitido quando a matrícula do aluno está concluída ou inativa.";
        public const string CRIANCA_JA_POSSUI_PLANO_AEE_ABERTO_INTEGRACAO = "Estudante/Criança já possui plano AEE em aberto";
    }
}