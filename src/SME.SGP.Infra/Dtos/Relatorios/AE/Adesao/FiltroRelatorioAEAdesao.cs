namespace SME.SGP.Infra
{
    public class FiltroRelatorioAEAdesao
    {
        public long DreCodigo { get; set; }
        public string UeCodigo { get; set; }        
        public bool ListarUsuariosNao { get; set; }
        public bool ListarUsuariosValidos { get; set; }
        public bool ListarUsuariosCPFIrregular { get; set; }
        public bool ListarUsuariosCPFTodos { get; set; }
        public string NomeUsuario { get; set; }

    }
}
