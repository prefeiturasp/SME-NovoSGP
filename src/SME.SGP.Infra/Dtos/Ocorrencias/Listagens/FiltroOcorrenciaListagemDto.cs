using System;

namespace SME.SGP.Infra.Dtos.Ocorrencias.Listagens
{
    public class FiltroOcorrenciaListagemDto
    {
        public DateTime? DataOcorrenciaInicio { get; set; }
        public DateTime? DataOcorrenciaFim { get; set; }
        public string AlunoNome { get; set; }
        public string Titulo { get; set; }
    }
}