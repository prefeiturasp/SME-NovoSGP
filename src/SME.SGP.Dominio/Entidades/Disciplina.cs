namespace SME.SGP.Dominio
{
    public class ComponenteCurricularEol
    {
        public long? CdComponenteCurricularPai { get; set; }
        public long Codigo { get; set; }
        public bool Compartilhada { get; set; }
        public bool LancaNota { get; set; }
        public string Nome { get; set; }
        public bool PossuiObjetivos { get; set; }
        public bool Regencia { get; set; }
        public bool RegistraFrequencia { get; set; }
        public bool TerritorioSaber { get; set; }
    }
}