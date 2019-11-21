using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasAula
    {
        AulaConsultaDto BuscarPorId(long id);
        Task<int> ObterQuantidadeAulasTurmaSemana(string turma, string disciplina, string semana);
    }
}
