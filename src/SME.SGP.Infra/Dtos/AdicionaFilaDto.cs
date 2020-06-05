namespace SME.SGP.Infra.Dtos
{
    public class AdicionaFilaDto
    {
        public AdicionaFilaDto(string fila, object dados, string endpoint)
        {
            Fila = fila;
            Dados = dados;
            Endpoint = endpoint;
        }

        public string Fila { get; set; }
        public object Dados { get; set; }
        //TODO: PENSAR EM NOME MELHOR
        public string Endpoint { get; set; }
    }
}
