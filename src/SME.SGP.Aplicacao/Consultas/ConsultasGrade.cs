using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasGrade : IConsultasGrade
    {
        private readonly IRepositorioGrade repositorioGrade;

        public ConsultasGrade(IRepositorioGrade repositorioGrade)
        {
            this.repositorioGrade = repositorioGrade ?? throw new System.ArgumentNullException(nameof(repositorioGrade));
        }
    }
}
