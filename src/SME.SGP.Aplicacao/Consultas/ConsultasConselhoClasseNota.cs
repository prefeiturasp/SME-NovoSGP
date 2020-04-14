using SME.SGP.Dominio.Interfaces;
using System;

namespace SME.SGP.Aplicacao
{
    public class ConsultasConselhoClasseNota : IConsultasConselhoClasseNota
    {
        private readonly IRepositorioConselhoClasseNota repositorioConselhoClasseNota;
        public ConsultasConselhoClasseNota(IRepositorioConselhoClasseNota repositorioConselhoClasseNota)
        {
            this.repositorioConselhoClasseNota = repositorioConselhoClasseNota ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseNota));
        }
    }
}