using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao.Comandos
{
    public class ComandosGrade: IComandosGrade
    {
        private readonly IRepositorioGrade repositorioGrade;

        public ComandosGrade(IRepositorioGrade repositorioGrade)
        {
            this.repositorioGrade = repositorioGrade ?? throw new ArgumentNullException(nameof(repositorioGrade));
        }
    }
}
