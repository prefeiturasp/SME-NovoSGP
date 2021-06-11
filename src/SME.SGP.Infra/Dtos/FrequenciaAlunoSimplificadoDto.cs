using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class FrequenciaAlunoSimplificadoDto
    {
        public TipoFrequencia TipoFrequencia { get; set; }
        public int NumeroAula { get; set; }
        public string CodigoAluno { get; set; }
    }
}