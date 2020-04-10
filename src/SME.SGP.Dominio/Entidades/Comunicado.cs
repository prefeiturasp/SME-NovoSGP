namespace SME.SGP.Dominio
{
    public class Comunicado : EntidadeBase
    {
        public string ComunicadoGrupoId { get; set; }
        public string Descricao { get; set; }
        public bool Excluido { get; set; }
        public string Titulo { get; set; }
    }
}