using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestionarioPlanoAEEIdQuery : IRequest<long>
    {
        private static ObterQuestionarioPlanoAEEIdQuery _instance;
        public static ObterQuestionarioPlanoAEEIdQuery Instance => _instance ??= new();
    }
}
