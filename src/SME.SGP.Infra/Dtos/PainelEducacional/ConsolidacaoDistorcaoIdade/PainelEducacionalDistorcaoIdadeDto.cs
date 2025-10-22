using System.Collections.Generic;

public class PainelEducacionalDistorcaoIdadeDto
{
    public string Modalidade { get; set; }
    public IEnumerable<SerieAnoDistorcaoIdadeDto> SerieAno { get; set; }
}

public class SerieAnoDistorcaoIdadeDto
{
    public string Ano { get; set; }
    public int QuantidadeAlunos { get; set; }
}