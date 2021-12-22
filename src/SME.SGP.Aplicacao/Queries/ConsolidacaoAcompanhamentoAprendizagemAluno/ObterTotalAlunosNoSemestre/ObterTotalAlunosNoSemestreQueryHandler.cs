﻿using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalAlunosNoSemestreQueryHandler : IRequestHandler<ObterTotalAlunosNoSemestreQuery, int>
    {
        private readonly IRepositorioAcompanhamentoAlunoConsulta repositorio;

        public ObterTotalAlunosNoSemestreQueryHandler(IRepositorioAcompanhamentoAlunoConsulta repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<int> Handle(ObterTotalAlunosNoSemestreQuery request, CancellationToken cancellationToken)
        {
            return await repositorio.ObterTotalAlunosTurmaSemestre(request.TurmaId, request.Semestre);
        }
    }
}
