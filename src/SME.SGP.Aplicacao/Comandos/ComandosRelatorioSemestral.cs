using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ComandosRelatorioSemestral : IComandosRelatorioSemestral
    {
        private readonly IRepositorioRelatorioSemestral repositorioRelatorioSemestral;
        public ComandosRelatorioSemestral(IRepositorioRelatorioSemestral repositorioRelatorioSemestral)
        {
            this.repositorioRelatorioSemestral = repositorioRelatorioSemestral ?? throw new ArgumentNullException(nameof(repositorioRelatorioSemestral));
        }

        public async Task SalvarAsync(RelatorioSemestral relatorioSemestral)
            => await repositorioRelatorioSemestral.SalvarAsync(relatorioSemestral);
    }
}
