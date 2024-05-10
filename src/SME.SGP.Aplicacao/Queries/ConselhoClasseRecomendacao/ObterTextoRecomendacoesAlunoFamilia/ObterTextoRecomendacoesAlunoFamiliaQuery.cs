using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterTextoRecomendacoesAlunoFamiliaQuery : IRequest<(string recomendacoesAluno, string recomendacoesFamilia)>
    {
    }
}
