namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAula : IRepositorioBase<Aula>
    {
        bool UsuarioPodeCriarAulaNaUeETurma(Aula aula);
    }
}