namespace SME.SGP.Dominio
{
    public class OcorrenciaAluno
    {
        public long Id { get; set; }
        public long CodigoAluno { get; set; }
        public Ocorrencia Ocorrencia { get; set; }
        public long OcorrenciaId { get; set; }

        public OcorrenciaAluno(long codigoAluno, Ocorrencia ocorrencia)
        {
            CodigoAluno = codigoAluno;
            SetOcorrencia(ocorrencia);
        }

        protected OcorrenciaAluno()
        {
        }

        private void SetOcorrencia(Ocorrencia ocorrencia)
        {
            if (ocorrencia is null)
                throw new NegocioException("É necessário informar a ocorrência para adicionar o aluno.");

            Ocorrencia = ocorrencia;
            OcorrenciaId = ocorrencia.Id;
        }
    }
}