namespace SME.SGP.Dominio.Entidades
{
    public class PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacao : EntidadeBase
    {
        public string NivelAlfabetizacao { get; set; }
        public string NivelAlfabetizacaoDescricao { get; set; }
        public int Ano { get; set; }
        public int TotalAlunos { get; set; }
        public int Periodo { get; set; }
    }
}
