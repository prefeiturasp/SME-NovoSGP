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

        public Task ExcluirFrequenciaGlobal(long frequenciaId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SalvarFrequenciaGlobal(PainelEducacionalRegistroFrequenciaAgrupamentoEscola entidade)
        {
            var retorno = await SalvarAsync(entidade);
            return retorno != 0;
        }
    }
}
