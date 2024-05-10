﻿using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasPorBimestresUeAlunoTurmaQueryHandler : IRequestHandler<ObterNotasPorBimestresUeAlunoTurmaQuery, IEnumerable<NotaConceitoBimestreComponenteDto>>
    {
        private readonly IRepositorioConselhoClasseNotaConsulta repositorioConselhoClasseNota;

        public ObterNotasPorBimestresUeAlunoTurmaQueryHandler(IRepositorioConselhoClasseNotaConsulta repositorioConselhoClasseNota)
        {
            this.repositorioConselhoClasseNota = repositorioConselhoClasseNota ?? throw new System.ArgumentNullException(nameof(repositorioConselhoClasseNota));
        }

        public async Task<IEnumerable<NotaConceitoBimestreComponenteDto>> Handle(ObterNotasPorBimestresUeAlunoTurmaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioConselhoClasseNota.ObterNotasBimestresAluno(request.AlunoCodigo, request.UeCodigo, request.TurmaCodigo, request.Bimestres);
        }
    }
}
