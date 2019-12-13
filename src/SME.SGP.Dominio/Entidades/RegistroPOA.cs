namespace SME.SGP.Dominio
{
    public class RegistroPoa : EntidadeBase
    {
        public string CodigoRf { get; set; }
        public string Descricao { get; set; }
        public string DreId { get; set; }
        public bool Excluido { get; set; }
        public int Mes { get; set; }
        public string Titulo { get; set; }
        public string UeId { get; set; }
    }
}