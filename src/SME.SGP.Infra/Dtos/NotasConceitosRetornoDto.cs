using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class NotasConceitosRetornoDto
    {
        public NotasConceitosRetornoDto()
        {
            Bimestres = new List<NotasConceitosBimestreRetornoDto>();
        }

        public string AuditoriaAlterado { get; set; }
        public string AuditoriaInserido { get; set; }
        public List<NotasConceitosBimestreRetornoDto> Bimestres { get; set; }
        public TipoNota NotaTipo { get; set; }
    }
}