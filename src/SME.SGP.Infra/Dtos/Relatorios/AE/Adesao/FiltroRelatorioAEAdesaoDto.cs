namespace SME.SGP.Infra
{
    public class FiltroRelatorioAEAdesaoDto
    {
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        [EnumeradoRequirido(ErrorMessage = "É necessário informar a opção de listagem de usuários.")]
        public FiltroRelatorioAEAdesao OpcaoListaUsuarios { get; set; }
        public string UsuarioNome { get; set; }
        public string UsuarioRF { get; set; }

    }
    public enum FiltroRelatorioAEAdesao
    {
        ListarUsuariosNao = 1,
        ListarUsuariosValidos = 2,
        ListarUsuariosCPFIrregular = 3,
        ListarUsuariosCPFTodos = 4

    }
}
