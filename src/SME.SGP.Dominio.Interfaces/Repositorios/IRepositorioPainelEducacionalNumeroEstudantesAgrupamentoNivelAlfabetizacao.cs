using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacao : IRepositorioBase<PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacao>
    {
        Task<IEnumerable<PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacao>> ObterNumeroAlunos(string anoLetivo, string periodo);
    }
}
