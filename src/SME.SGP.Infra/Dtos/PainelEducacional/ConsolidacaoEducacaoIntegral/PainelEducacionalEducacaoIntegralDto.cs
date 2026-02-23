using System.Collections.Generic;

public class PainelEducacionalEducacaoIntegralDto
{
    public string Modalidade { get; set; }
    public IEnumerable<IndicadorEducacaoIntegralDto> Indicadores { get; set; }
}

public class IndicadorEducacaoIntegralDto
{
    public string AnoSerieEtapa { get; set; }
    public int QuantidadeAlunosIntegral { get; set; }
    public int QuantidadeAlunosParcial { get; set; }
}