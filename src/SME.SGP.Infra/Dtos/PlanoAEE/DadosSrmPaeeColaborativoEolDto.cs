using System;

namespace SME.SGP.Infra
{
    public class DadosSrmPaeeColaborativoEolDto
    {
       public long CodigoTurma { get; set; }
       public string CodigoEscola { get; set; }
       public string Turno { get; set; }
       public string Componente { get; set; }
       public long CodigoComponente { get; set; }
       public long CodigoAluno { get; set; }
       public string SituacaoMatricula { get; set; }
       public DateTime DataMatricula { get; set; }
    }
}