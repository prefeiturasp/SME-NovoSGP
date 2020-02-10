using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IComandosProcessoExecutando
    {
        Task Incluir(ProcessoExecutando processo);
        void Excluir(ProcessoExecutando processo);

        Task IncluirCalculoFrequencia(string turmaId, string disciplinaId, int bimestre);
        Task ExcluirCalculoFrequencia(string turmaId, string disciplinaId, int bimestre);
    }
}
