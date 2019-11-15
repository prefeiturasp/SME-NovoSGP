using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasGrade
    {
        Task<GradeDto> ObterGradeTurma(TipoEscola tipoEscola, Modalidade modalidade, int duracao);
    }
}
