﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao.Consultas
{
    public class ConsultasFechamentoTurma : IConsultasFechamentoTurma
    {
        private readonly IRepositorioFechamentoTurmaConsulta repositorioFechamentoTurma;

        public ConsultasFechamentoTurma(IRepositorioFechamentoTurmaConsulta repositorioFechamentoTurma)
        {
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurma));
        }

        public async Task<FechamentoTurma> ObterCompletoPorIdAsync(long fechamentoTurmaId)
            => await repositorioFechamentoTurma.ObterCompletoPorIdAsync(fechamentoTurmaId);

        public async Task<FechamentoTurma> ObterPorTurmaCodigoBimestreAsync(string turmaCodigo, int bimestre = 0)
            => await repositorioFechamentoTurma.ObterPorTurmaCodigoBimestreAsync(turmaCodigo, bimestre);
    }
}
