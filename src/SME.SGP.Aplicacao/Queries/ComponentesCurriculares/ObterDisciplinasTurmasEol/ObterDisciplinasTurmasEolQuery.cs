using MediatR;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDisciplinasTurmasEolQuery : IRequest<IEnumerable<DisciplinaResposta>>
    {
        public ObterDisciplinasTurmasEolQuery(string[] codigosDeTurmas, bool? adicionarComponentesPlanejamento = null)
        {
            CodigosDeTurmas = codigosDeTurmas;
            AdicionarComponentesPlanejamento = adicionarComponentesPlanejamento;
        }

        public string[] CodigosDeTurmas { get; set; }
        public bool? AdicionarComponentesPlanejamento { get; private set; }
    }
}
