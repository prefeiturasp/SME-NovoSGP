using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoAula
    {
        private readonly IRepositorioAula repositorioAula;

        public ServicoAula(IRepositorioAula repositorioAula)
        {
            this.repositorioAula = repositorioAula ?? throw new System.ArgumentNullException(nameof(repositorioAula));
        }

        public void Salvar(Aula aula, Usuario usuario)
        {
            if (repositorioAula.UsuarioPodeCriarAula(aula, usuario))
            {
                repositorioAula.Salvar(aula);
            }
        }
    }
}