namespace SME.SGP.Infra
{
    public class FiltroObterSecoesDto
    {
        public FiltroObterSecoesDto(string codigoTurma, string codigoAluno, long pAPPeriodoId)
        {
            CodigoTurma = codigoTurma;
            CodigoAluno = codigoAluno;
            PAPPeriodoId = pAPPeriodoId;
        }

        public string CodigoTurma { get; set; }
        public string CodigoAluno { get; set; }
        public long PAPPeriodoId { get; set; }
    }
}
