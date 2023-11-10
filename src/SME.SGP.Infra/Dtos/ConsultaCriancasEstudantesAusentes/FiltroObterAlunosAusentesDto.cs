using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Infra.Dtos.ConsultaCriancasEstudantesAusentes
{
    public class FiltroObterAlunosAusentesDto
    {
        public string CodigoUe { get; set; }
        public int AnoLetivo {  get; set; }
        public string CodigoTurma { get; set; }
        public EnumAusencias Ausencias { get; set; }
    }
}
