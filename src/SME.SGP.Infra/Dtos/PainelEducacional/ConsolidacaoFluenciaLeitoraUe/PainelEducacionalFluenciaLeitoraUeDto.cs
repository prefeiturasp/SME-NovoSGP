using System.Collections.Generic;

public class PainelEducacionalFluenciaLeitoraUeDto
{
    public string Turma { get; set; }
    public int AlunosPrevistos { get; set; }
    public int AlunosAvaliados { get; set; }
    public int TotalPreLeitor { get; set; }
    public IEnumerable<IndicadorPreLeitorDto> Indicadores { get; set; }
}

public class IndicadorPreLeitorDto
{
    public int Fluencia { get; set; }
    public int QuantidadeAlunos { get; set; }
    public decimal PercentualFluencia { get; set; }
}