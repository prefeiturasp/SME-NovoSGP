namespace SME.SGP.Infra.Dtos.Relatorios.HistoricoEscolar
{

    public class AlunoDto
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }

        public AlunoDto(string codigo, string nome)
        {
            Codigo = codigo;
            Nome = nome;
        }
    }

}
