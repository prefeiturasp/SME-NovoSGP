using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasUnidadesEscolares : IConsultasUnidadesEscolares
    {
        private readonly IServicoEol servicoEOL;

        public ConsultasUnidadesEscolares(IServicoEol servicoEOL)
        {
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
        }

        public async Task<IEnumerable<UsuarioEolRetornoDto>> ObtemFuncionariosPorUe(BuscaFuncionariosFiltroDto buscaFuncionariosFiltroDto)
        {
            return await servicoEOL.ObterFuncionariosPorUe(buscaFuncionariosFiltroDto);
        }
    }
}