using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterRecomendacoesAlunoFamiliaQuery : IRequest<IEnumerable<RecomendacoesAlunoFamiliaDto>>
    {
        private static ObterRecomendacoesAlunoFamiliaQuery _instance;
        public static ObterRecomendacoesAlunoFamiliaQuery Instance => _instance ??= new();
    }
}
