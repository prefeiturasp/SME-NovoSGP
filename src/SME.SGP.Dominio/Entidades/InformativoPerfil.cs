namespace SME.SGP.Dominio
{
    public class InformativoPerfil : EntidadeBase
    {
        public long InformativoId { get; set; }
        public long CodigoPerfil { get; set; }
        public bool Excluido { get; set; }
    }
}
