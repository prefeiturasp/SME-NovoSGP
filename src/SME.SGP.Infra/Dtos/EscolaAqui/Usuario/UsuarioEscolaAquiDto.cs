namespace SME.SGP.Infra.Dtos.EscolaAqui
{
    public class UsuarioEscolaAquiDto
    {
        public string Cpf { get; set; }

        public string Nome { get; set; }

        public UsuarioEscolaAquiDto(string cpf, string nome)
        {
            Cpf = cpf;
            Nome = nome;
        }
    }
}
