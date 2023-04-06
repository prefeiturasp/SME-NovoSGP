namespace SME.SGP.Infra
{
    public class FiltroFaltasNaoCompensadasDto
    {
       public string TurmaId { get; set; }
       public string DisciplinaId { get; set; }
       public string CodigoAluno { get; set; }
       public int QuantidadeCompensar { get; set; }
       public int CompensacaoId { get; set; }
       public int Bimestre { get; set; }
    }
}