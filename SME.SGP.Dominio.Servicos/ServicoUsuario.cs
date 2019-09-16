using SME.SGP.Dominio.Interfaces;
using System;

namespace SME.SGP.Dominio
{
    public class ServicoUsuario : IServicoUsuario
    {
        private readonly IRepositorioUsuario repositorioUsuario;

        public ServicoUsuario(IRepositorioUsuario repositorioUsuario)
        {
            this.repositorioUsuario = repositorioUsuario ?? throw new ArgumentNullException(nameof(repositorioUsuario));
        }

        public Usuario ObterUsuarioPorCodigoRfOuAdiciona(string codigoRf)
        {
            var usuario = repositorioUsuario.ObterPorCodigoRf(codigoRf);
            if (usuario != null)
                return usuario;

            usuario = new Usuario() { CodigoRf = codigoRf };
            repositorioUsuario.Salvar(usuario);

            return usuario;
        }
    }
}