using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ComandosRelatorioSemestralTurmaPAP : IComandosRelatorioSemestralTurmaPAP
    {
        private readonly IRepositorioRelatorioSemestralTurmaPAP repositorioRelatorioSemestral;
        public ComandosRelatorioSemestralTurmaPAP(IRepositorioRelatorioSemestralTurmaPAP repositorioRelatorioSemestral)
        {
            this.repositorioRelatorioSemestral = repositorioRelatorioSemestral ?? throw new ArgumentNullException(nameof(repositorioRelatorioSemestral));
        }

        public async Task SalvarAsync(RelatorioSemestralTurmaPAP relatorioSemestral)
            => await repositorioRelatorioSemestral.SalvarAsync(relatorioSemestral);
    }
}
