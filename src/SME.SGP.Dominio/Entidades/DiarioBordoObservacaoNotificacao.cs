namespace SME.SGP.Dominio
{
    public class DiarioBordoObservacaoNotificacao
    {
        protected DiarioBordoObservacaoNotificacao() { }

        public DiarioBordoObservacaoNotificacao(long idObservacao, long idNotificacao, long idUsuario)
        {
            this.IdObservacao = idObservacao;
            this.IdNotificacao = idNotificacao;
            this.IdUsuario = idUsuario;
        }

        public long Id { get; set; }
        public long IdObservacao { get; set; }
        public long IdNotificacao { get; set; }
        public long IdUsuario { get; set; }
    }
}
