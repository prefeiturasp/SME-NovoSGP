using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos
{
    public class NotificacaoAndamentoFechamentoPorUeDto
    {
        public long[] TurmasIds { get; set; }
        public long PeriodoEscolarId { get; set; }
        public long UeId { get; set; }
        public ComponenteCurricularDto[] Componentes { get; set; }
    }
}
