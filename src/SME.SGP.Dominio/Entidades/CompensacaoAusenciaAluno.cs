using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio
{
    public class CompensacaoAusenciaAluno : EntidadeBase
    {
        public bool Excluido { get; set; }
        public long CompensacaoAusenciaId { get; set; }
        [Computed]
        public CompensacaoAusencia CompensacaoAusencia { get; set; }
        public string CodigoAluno { get; set; }
        public int QuantidadeFaltasCompensadas { get; set; }
        public bool Notificado { get; set; }

        public void Excluir()
            => Excluido = true;
    }
}
