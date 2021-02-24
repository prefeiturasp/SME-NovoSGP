using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoEncaminhamentoAEE
    { 
        Task<IEnumerable<UsuarioEolRetornoDto>> ObterPAEETurma(Turma turma);
        Task<long> ObtemUsuarioCEFAIDaDre(string codigoDre);
    }
}