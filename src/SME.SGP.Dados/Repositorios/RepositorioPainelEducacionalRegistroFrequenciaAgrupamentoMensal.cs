using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPainelEducacionalRegistroFrequenciaAgrupamentoMensal : RepositorioBase<PainelEducacionalRegistroFrequenciaAgrupamentoMensal>, IRepositorioPainelEducacionalRegistroFrequenciaAgrupamentoMensal
    {
        public RepositorioPainelEducacionalRegistroFrequenciaAgrupamentoMensal(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task ExcluirFrequenciaMensal(long frequenciaId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SalvarFrequenciaMensal(PainelEducacionalRegistroFrequenciaAgrupamentoMensal entidade)
        {
            var retorno = await SalvarAsync(entidade);
            return retorno != 0;
        }
    }
}
