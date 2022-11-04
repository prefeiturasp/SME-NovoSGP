namespace SME.SGP.Dominio
{ 
    public class OcorrenciaServidor : EntidadeBase
    {
        public string CodigoServidor { get; set; }
        public Ocorrencia Ocorrencia { get; set; }
        public long OcorrenciaId { get; set; }
        
        public OcorrenciaServidor(string codigoServidor, Ocorrencia ocorrencia)
        {
            CodigoServidor = codigoServidor;
            SetOcorrencia(ocorrencia);
        }

        public OcorrenciaServidor()
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
