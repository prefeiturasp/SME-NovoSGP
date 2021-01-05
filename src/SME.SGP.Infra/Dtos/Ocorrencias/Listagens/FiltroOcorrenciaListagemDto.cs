using System;

namespace SME.SGP.Infra
{
    public class FiltroOcorrenciaListagemDto
    {
        public DateTime? DataOcorrenciaInicio { get; set; }
        public DateTime? DataOcorrenciaFim { get; set; }
        public string AlunoNome { get; set; }
        public string Titulo { get; set; }
        public long TurmaId { get; set; }
    }
}