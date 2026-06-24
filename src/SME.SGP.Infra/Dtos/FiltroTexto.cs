namespace SME.SGP.Infra
{
    public class FiltroTexto
    {
        public FiltroTexto(string filtro, bool filtroEhCodigo = false)
        {
            Filtro = filtro;
            FiltroEhCodigo = filtroEhCodigo;
        }

        public string Filtro { get; set; }
        public bool FiltroEhCodigo { get; set; }
    }
}
