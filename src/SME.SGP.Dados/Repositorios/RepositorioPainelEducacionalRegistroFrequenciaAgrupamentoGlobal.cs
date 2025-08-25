using Dommel;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobal : RepositorioBase<PainelEducacionalRegistroFrequenciaAgrupamentoGlobal>, IRepositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobal
    {
        public RepositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobal(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task ExcluirFrequenciaGlobal(PainelEducacionalRegistroFrequenciaAgrupamentoGlobal entidade)
        {
             await RemoverAsync(entidade);
        }

        public async Task<bool> SalvarFrequenciaGlobal(PainelEducacionalRegistroFrequenciaAgrupamentoGlobal entidade)
        {
            var retorno = await SalvarAsync(entidade);
            return retorno != 0;
        }
    }
}
