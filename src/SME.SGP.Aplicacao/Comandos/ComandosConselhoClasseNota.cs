using SME.SGP.Dominio.Interfaces;
using System;

namespace SME.SGP.Aplicacao
{
    public class ComandosConselhoClasseNota : IComandosConselhoClasseNota
    {
        private readonly IRepositorioConselhoClasseNota repositorioConselhoClasseNota;

        public ComandosConselhoClasseNota(IRepositorioConselhoClasseNota repositorioConselhoClasseNota)
        {
            this.repositorioConselhoClasseNota = repositorioConselhoClasseNota ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseNota));
        }


    }
}