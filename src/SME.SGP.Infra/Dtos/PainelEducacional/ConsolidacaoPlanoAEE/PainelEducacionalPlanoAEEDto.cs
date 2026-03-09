using System.Collections.Generic;

public class PainelEducacionalPlanoAEEDto
{
    public int QuantidadePlanos { get; set; }
    public IEnumerable<IndicadorPlanoAEEDto> Planos { get; set; }
}

public class IndicadorPlanoAEEDto
{
    public string SituacaoPlano { get; set; }
    public int QuantidadeAlunos { get; set; }
}