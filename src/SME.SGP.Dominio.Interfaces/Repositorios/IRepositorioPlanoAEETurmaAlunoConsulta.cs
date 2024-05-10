﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPlanoAEETurmaAlunoConsulta
    {
        Task<IEnumerable<PlanoAEETurmaAluno>> ObterPlanoAEETurmaAlunoPorIdAsync(long planoAEEId);
    }
}
