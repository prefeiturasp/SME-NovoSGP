using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Infra
{
    public class FiltroPlanosAEEDto
    {
        public long DreId { get; set; }
        public long UeId { get; set; }
        public long TurmaId { get; set; }
        public string AlunoCodigo { get; set; }
        public SituacaoPlanoAEE? Situacao { get; set; }
        public string ResponsavelRf { get; set; }
        public string PaaiReponsavelRf { get; set; }
        public bool ExibirEncerrados { get; set; }
    }
}
