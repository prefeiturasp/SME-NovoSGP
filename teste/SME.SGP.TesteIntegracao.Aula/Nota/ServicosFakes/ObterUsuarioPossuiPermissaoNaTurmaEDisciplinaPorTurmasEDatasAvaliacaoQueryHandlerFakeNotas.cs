using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Nota.ServicosFakes
{
    public class ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaPorTurmasEDatasAvaliacaoQueryHandlerFakeNotas : IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaPorTurmasEDatasAvaliacaoQuery, IEnumerable<UsuarioPossuiAtribuicaoEolDto>>
        {
            private const string TURMA_CODIGO_1 = "1";

            public ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaPorTurmasEDatasAvaliacaoQueryHandlerFakeNotas()
            {}

            public Task<IEnumerable<UsuarioPossuiAtribuicaoEolDto>> Handle(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaPorTurmasEDatasAvaliacaoQuery request, CancellationToken cancellationToken)
            {
                var usariosAtribuicao = new List<UsuarioPossuiAtribuicaoEolDto>()
                {
                    new UsuarioPossuiAtribuicaoEolDto() 
                    {
                        CodigoTurma = TURMA_CODIGO_1,
                        DataAtribuicaoAula = DateTime.Now,
                        DataDisponibilizacaoAulas = DateTime.Now
                    },
                };

                return Task.FromResult<IEnumerable<UsuarioPossuiAtribuicaoEolDto>>(usariosAtribuicao);
            }
        }
    }
