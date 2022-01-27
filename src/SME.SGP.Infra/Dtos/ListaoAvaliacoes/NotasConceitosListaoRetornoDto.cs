
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class NotasConceitosListaoRetornoDto
    {
        public NotasConceitosListaoRetornoDto()
        {
            Bimestres = new List<NotasConceitosBimestreListaoRetornoDto>();
        }

        public string AuditoriaAlterado { get; set; }
        public string AuditoriaInserido { get; set; }
        public int BimestreAtual { get; set; }
        public double PercentualAlunosInsuficientes { get; set; }
        public List<NotasConceitosBimestreListaoRetornoDto> Bimestres { get; set; }
        public TipoNota NotaTipo { get; set; }
    }
}
