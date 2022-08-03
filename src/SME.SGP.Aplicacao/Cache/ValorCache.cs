namespace SME.SGP.Aplicacao
{
    public class ValorCache<T> where T : class
    {
        public string Chave { get; set; }
        public T Valor {  get; set; }
    }
}
