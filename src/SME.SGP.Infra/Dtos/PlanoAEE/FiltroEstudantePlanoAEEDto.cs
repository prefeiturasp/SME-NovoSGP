namespace SME.SGP.Infra
{
    public class FiltroEstudantePlanoAEEDto
    {
            public FiltroEstudantePlanoAEEDto(string codigoEstudante, string codigoUe)
            {
                CodigoEstudante = codigoEstudante;
                CodigoUe = codigoUe;
            }
            public string CodigoEstudante { get; }
            public string CodigoUe { get; }
    }
}
