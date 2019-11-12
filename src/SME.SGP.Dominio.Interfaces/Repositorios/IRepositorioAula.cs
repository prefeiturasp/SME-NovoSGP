namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAula : IRepositorioBase<Aula>
    {
        bool UsuarioPodeCriarAula(Aula aula, Usuario usuario);
    }
}
