using System;

namespace SME.SGP.Infra.Dtos.Ocorrencias.Listagens
{
    public class OcorrenciaListagemDto
    {
        public long Id { get; set; }
        public string DataOcorrencia { get; set; }
        public string Titulo { get; set; }
        public string AlunoOcorrencia { get; set; }
    }
}