using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoCompensacaoAusencia
    {
        Task Salvar(long id, CompensacaoAusenciaDto compensacaoDto);
        Task Excluir(long[] compensacoesIds);
        Task<string> Copiar(CompensacaoAusenciaCopiaDto compensacaoCopia);
    }
}
