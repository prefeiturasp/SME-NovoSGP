using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Infra.Dtos
{
    public class TurmaUeDreDto
    {
        public TurmaUeDreDto()
        { }
        public long Id { get; set; }
        public string Codigo { get; set; }
        public int AnoLetivo { get; set; }
        public TipoTurma Tipo { get; set; }
        public Modalidade ModalidadeCodigo { get; set; }
        public int TipoTurno { get; set; }
        public string Nome { get; set; }
        public string UeNome { get; set; }
        public string UeCodigo { get; set; }
        public long UeId { get; set; }
        public TipoEscola TipoEscola { get; set; }
        public long DreId { get; set; }
        public string DreCodigo { get; set; }
        public string DreNome { get; set; }
        public string DreAbreviacao { get; set; }
        public string NomeTipoUeDre { get; set; }
    }
}
