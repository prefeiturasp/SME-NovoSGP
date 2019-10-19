namespace SME.SGP.Infra
{
    public class Paginacao
    {
        public Paginacao(int pagina, int registros)
        {
            pagina = pagina < 1 ? 1 : pagina;
            registros = registros < 1 ? 0 : registros;

            QuantidadeRegistros = registros;
            QuantidadeRegistrosIgnorados = (pagina - 1) * registros;
        }

        public int QuantidadeRegistros { get; private set; }
        public int QuantidadeRegistrosIgnorados { get; private set; }
    }
}