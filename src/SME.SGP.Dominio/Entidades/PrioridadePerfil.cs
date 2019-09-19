namespace SME.SGP.Dominio.Entidades
{
    public class PrioridadePerfil : EntidadeBase
    {
        public string CodigoPerfil { get; set; }
        public string NomePerfil { get; set; }
        public int Ordem { get; set; }
    }
}