﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUltimoBimestreAlunoTurmaQueryHandler : IRequestHandler<ObterUltimoBimestreAlunoTurmaQuery, (int bimestre, bool possuiConselho)>
    {
        private readonly IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar;
        private readonly IRepositorioConselhoClasseConsulta repositorioConselhoClasseConsulta;

        public ObterUltimoBimestreAlunoTurmaQueryHandler(IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolarConsulta,
                                                    IRepositorioConselhoClasseConsulta repositorioConselhoClasseConsulta)
        {
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolarConsulta ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolarConsulta));
            this.repositorioConselhoClasseConsulta = repositorioConselhoClasseConsulta ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseConsulta));
        }

        public async Task<(int bimestre, bool possuiConselho)> Handle(ObterUltimoBimestreAlunoTurmaQuery request, CancellationToken cancellationToken)
        {
            var periodoEscolar = await repositorioPeriodoEscolar.ObterUltimoBimestreAsync(request.Turma.AnoLetivo, request.Turma.ObterModalidadeTipoCalendario(), request.Turma.Semestre);

            if (periodoEscolar == null)
                throw new NegocioException($"Não foi encontrado o ultimo periodo escolar para a turma {request.Turma.Nome}");

            var conselhoClasseUltimoBimestre = await repositorioConselhoClasseConsulta.ObterPorTurmaAlunoEPeriodoAsync(request.Turma.Id, request.AlunoCodigo, periodoEscolar.Id);

            return (periodoEscolar.Bimestre, conselhoClasseUltimoBimestre != null);
        }
    }
}
