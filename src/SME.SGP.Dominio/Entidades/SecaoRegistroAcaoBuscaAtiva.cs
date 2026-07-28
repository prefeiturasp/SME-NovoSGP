using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio
{
    public class SecaoRegistroAcaoBuscaAtiva : EntidadeBase
    {
        [Computed]
        public Questionario Questionario { get; set; }
        public long QuestionarioId { get; set; }

        public string Nome { get; set; }
        public int Ordem { get; set; }
        public int Etapa { get; set; }
        public bool Excluido { get; set; }
        public string? NomeComponente { get; set; }
        [Computed]
        public RegistroAcaoBuscaAtivaSecao RegistroBuscaAtivaSecao { get; set; }
    }
}
