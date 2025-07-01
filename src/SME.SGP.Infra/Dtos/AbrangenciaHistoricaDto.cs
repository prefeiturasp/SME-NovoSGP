using System;

namespace SME.SGP.Dto
{
    public class AbrangenciaHistoricaDto
    {
        public long Id { get; set; }
        public long UsuarioId { get; set; }
        public string Login { get; set; }
        public long DreId { get; set; }
        public string CodigoDre { get; set; }
        public long UeId { get; set; }
        public string CodigoUe { get; set; }
        public long TurmaId { get; set; }
        public string CodigoTurma { get; set; }
        public Guid perfil { get; set; }
    }
}
