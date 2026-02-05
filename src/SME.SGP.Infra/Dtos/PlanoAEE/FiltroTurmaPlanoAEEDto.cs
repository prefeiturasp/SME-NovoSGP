namespace SME.SGP.Infra.Dtos.PlanoAEE
{
    public class FiltroTurmaPlanoAEEDto
    {
        public FiltroTurmaPlanoAEEDto(long codigoTurma, string codigoUe = null)
        {
            CodigoTurma = codigoTurma;
            CodigoUe = codigoUe;
        }

        public long CodigoTurma { get; }
        public string? CodigoUe { get; }
    }
}