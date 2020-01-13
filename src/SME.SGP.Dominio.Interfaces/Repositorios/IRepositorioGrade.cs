using SME.SGP.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioGrade : IRepositorioBase<Grade>
    {
        Task<Grade> ObterGradeTurma(TipoEscola tipoEscola, Modalidade modalidade, int duracao);
        Task<int> ObterHorasComponente(long grade, int componenteCurricular, int ano);
    }
}
