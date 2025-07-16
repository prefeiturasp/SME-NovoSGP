using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class DiarioBordoObservacaoNotificacao
    {
        public DiarioBordoObservacaoNotificacao() { }

        public DiarioBordoObservacaoNotificacao(long idObservacao, long idNotificacao)
        {
            this.IdObservacao = idObservacao;
            this.IdNotificacao = idNotificacao;
        }

        public long Id { get; set; }
        public long IdObservacao { get; set; }
        public long IdNotificacao { get; set; }
    }
}
