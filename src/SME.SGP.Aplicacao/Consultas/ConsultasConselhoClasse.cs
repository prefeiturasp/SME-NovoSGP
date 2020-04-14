using SME.SGP.Dominio.Interfaces;
using System;

namespace SME.SGP.Aplicacao
{
    public class ConsultasConselhoClasse : IConsultasConselhoClasse
    {
        private readonly IRepositorioConselhoClasse repositorioConselhoClasse;
        public ConsultasConselhoClasse(IRepositorioConselhoClasse repositorioConselhoClasse)
        {
            this.repositorioConselhoClasse = repositorioConselhoClasse ?? throw new ArgumentNullException(nameof(repositorioConselhoClasse));
        }
    }
}