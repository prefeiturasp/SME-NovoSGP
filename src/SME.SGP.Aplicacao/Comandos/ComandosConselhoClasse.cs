using SME.SGP.Dominio.Interfaces;
using System;

namespace SME.SGP.Aplicacao
{
    public class ComandosConselhoClasse : IComandosConselhoClasse
    {
        private readonly IRepositorioConselhoClasse repositorioConselhoClasse;

        public ComandosConselhoClasse(IRepositorioConselhoClasse repositorioConselhoClasse)
        {
            this.repositorioConselhoClasse = repositorioConselhoClasse ?? throw new ArgumentNullException(nameof(repositorioConselhoClasse));
        }


    }
}