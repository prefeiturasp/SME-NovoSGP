using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRecomendacoesAlunoFamiliaQuery : IRequest<IEnumerable<RecomendacoesAlunoFamiliaDto>>
    {
        private static ObterRecomendacoesAlunoFamiliaQuery _instance;
        public static ObterRecomendacoesAlunoFamiliaQuery Instance => _instance ??= new();
    }
}
