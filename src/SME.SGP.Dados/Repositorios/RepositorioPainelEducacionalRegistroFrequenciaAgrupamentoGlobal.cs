using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobal : RepositorioBase<PainelEducacionalRegistroFrequenciaAgrupamentoGlobal>, IRepositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobal
    {
        public RepositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobal(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public Task ExcluirFrequenciaGlobal(long frequenciaId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SalvarFrequenciaGlobal(PainelEducacionalRegistroFrequenciaAgrupamentoGlobal entidade)
        {
            var retorno = await SalvarAsync(entidade);
            return retorno != 0;
        }
    }
}
