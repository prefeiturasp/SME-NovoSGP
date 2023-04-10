using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public interface IRepositorioRegistroFrequenciaAluno : IRepositorioBase<RegistroFrequenciaAluno>
    {
        Task RemoverPorRegistroFrequenciaIdENumeroAula(long registroFrequenciaId, int numeroAula, string codigoAluno);
        Task RemoverPorRegistroFrequenciaId(long registroFrequenciaId, string[] alunosComFrequenciaRegistrada);
        Task<bool> InserirVarios(IEnumerable<RegistroFrequenciaAluno> registros);
        Task<bool> InserirVariosComLog(IEnumerable<RegistroFrequenciaAluno> registros);
        Task ExcluirVarios(List<long> idsParaExcluir);
        Task AlterarRegistroAdicionandoAula(long registroFrequenciaId, long aulaId);
        Task ExcluirRegistroFrequenciaAlunoPorAulaECodigosAlunos(long aulaId, string[] codigosAlunos);
    }
}
