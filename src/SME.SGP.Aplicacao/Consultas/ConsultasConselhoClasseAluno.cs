using SME.SGP.Dominio.Interfaces;
using System;

namespace SME.SGP.Aplicacao
{
    public class ConsultasConselhoClasseAluno : IConsultasConselhoClasseAluno
    {
        private readonly IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno;
        public ConsultasConselhoClasseAluno(IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno)
        {
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
        }
    }
}