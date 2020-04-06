using SME.SGP.Dominio.Interfaces;
using System;

namespace SME.SGP.Aplicacao
{
    public class ComandosConselhoClasseAluno : IComandosConselhoClasseAluno
    {
        private readonly IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno;

        public ComandosConselhoClasseAluno(IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno)
        {
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
        }


    }
}