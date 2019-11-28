using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ConsultasAtribuicaoCJ : IConsultasAtribuicaoCJ
    {
        private readonly IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ;

        public ConsultasAtribuicaoCJ(IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ)
        {
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new ArgumentNullException(nameof(repositorioAtribuicaoCJ));
        }

        public IEnumerable<AtribuicaoCJListaRetornoDto> Listar(AtribuicaoCJListaFiltroDto filtroDto)
        {
            return null;
        }
    }
}