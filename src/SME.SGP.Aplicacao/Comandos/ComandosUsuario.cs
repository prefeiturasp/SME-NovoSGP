using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ComandosUsuario
    {
        private readonly IRepositorioUsuario repositorioUsuario;

        public ComandosUsuario(IRepositorioUsuario repositorioUsuario)
        {
            this.repositorioUsuario = repositorioUsuario ?? throw new System.ArgumentNullException(nameof(repositorioUsuario));
        }
    }
}