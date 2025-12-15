using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.NovoEncaminhamentoNAAPA;
using SME.SGP.Aplicacao.Queries.NovoEncaminhamentoNAAPA.ExisteNovoEncaminhamentoNAAPAAtivoParaAluno;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.NovoEncaminhamentoNAAPA
{
    public class ExisteNovoEncaminhamentoNAAPAAtivoParaAlunoUseCase : AbstractUseCase, IExisteNovoEncaminhamentoNAAPAAtivoParaAlunoUseCase
    {
        public ExisteNovoEncaminhamentoNAAPAAtivoParaAlunoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public Task<bool> Executar(string param)
        {
            return mediator.Send(new ExisteNovoEncaminhamentoNAAPAAtivoParaAlunoQuery(param));
        }
    }
}