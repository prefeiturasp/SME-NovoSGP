using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobalEscola : RepositorioBase<PainelEducacionalRegistroFrequenciaAgrupamentoEscola>, IRepositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobalEscola
    {
        public RepositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobalEscola(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task ExcluirFrequenciaGlobal(PainelEducacionalRegistroFrequenciaAgrupamentoEscola entidade)
        {
           await RemoverAsync(entidade);
        }

        public async Task<bool> SalvarFrequenciaGlobal(PainelEducacionalRegistroFrequenciaAgrupamentoEscola entidade)
        {
            var retorno = await SalvarAsync(entidade);
            return retorno != 0;
        }
    }
}
