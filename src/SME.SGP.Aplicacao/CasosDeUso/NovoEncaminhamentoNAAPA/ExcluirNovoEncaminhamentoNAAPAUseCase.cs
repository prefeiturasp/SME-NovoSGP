using MediatR;
using SME.SGP.Aplicacao.Commands.NovoEncaminhamentoNAAPA.ExcluirNovoEncaminhamentoNAAPA;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.NovoEncaminhamentoNAAPA;
using SME.SGP.Aplicacao.Queries.NovoEncaminhamentoNAAPA.ExisteEncaminhamentoNAAPAAtivo;
using SME.SGP.Aplicacao.Queries.NovoEncaminhamentoNAAPA.ObterNovoEncaminhamentoNAAPAPorId;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.NovoEncaminhamentoNAAPA
{
    public class ExcluirNovoEncaminhamentoNAAPAUseCase : AbstractUseCase, IExcluirNovoEncaminhamentoNAAPAUseCase
    {
        public ExcluirNovoEncaminhamentoNAAPAUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(long encaminhamentoNAAPAId)
        {
            var existeEncaminhamentoAtivo = await mediator.Send(new ExisteEncaminhamentoNAAPAAtivoQuery(encaminhamentoNAAPAId));

            var encaminhamentoNAAPA = await mediator.Send(new ObterNovoEncaminhamentoNAAPAPorIdQuery(encaminhamentoNAAPAId));

            if (!existeEncaminhamentoAtivo)
                throw new NegocioException(MensagemNegocioNovoEncaminhamentoNAAPA.ENCAMINHAMENTO_NAO_EXISTE_OU_ESTA_EXCLUIDO);

            if (encaminhamentoNAAPA.EhNulo())
                throw new NegocioException(MensagemNegocioNovoEncaminhamentoNAAPA.ENCAMINHAMENTO_NAO_ENCONTRADO);

            if (encaminhamentoNAAPA.Situacao != SituacaoNovoEncaminhamentoNAAPA.AguardandoAtendimento)
                throw new NegocioException(MensagemNegocioNovoEncaminhamentoNAAPA.ENCAMINHAMENTO_NAO_PODE_SER_EXCLUIDO_NESSA_SITUACAO);

            return (await mediator.Send(new ExcluirNovoEncaminhamentoNAAPACommand(encaminhamentoNAAPAId)));
        }
    }
}