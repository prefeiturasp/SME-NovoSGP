using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class UeRegistroColetivoDto
    {
        public long RegistroColetivoId { get; set; }
        public long Id { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public TipoEscola TipoEscola { get; set; }
        public string NomeFormatado => TipoEscola != TipoEscola.Nenhum ? $"{TipoEscola.ObterNomeCurto()} {Nome}" : $"{Nome}";
    }
}
