using SME.SGP.Dominio;
using System;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasConselhoClasseNota
    {
        ConselhoClasseNota ObterPorId(long id);
        Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasFinaisAlunoAsync(string alunoCodigo, string turmaCodigo);
    }
}
