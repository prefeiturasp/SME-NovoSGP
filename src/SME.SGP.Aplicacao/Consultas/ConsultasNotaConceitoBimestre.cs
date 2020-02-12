using SME.SGP.Dominio.Interfaces;
using System;

namespace SME.SGP.Aplicacao
{
    public class ConsultasNotaConceitoBimestre : IConsultasNotaConceitoBimestre
    {
        private readonly IRepositorioNotaConceitoBimestre repositorioNotaConceitoBimestre;
        public ConsultasNotaConceitoBimestre(IRepositorioNotaConceitoBimestre repositorioNotaConceitoBimestre)
        {
            this.repositorioNotaConceitoBimestre = repositorioNotaConceitoBimestre ?? throw new ArgumentNullException(nameof(repositorioNotaConceitoBimestre));
        }
    }
}