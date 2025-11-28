using MediatR;
using SME.SGP.Aplicacao.Queries.NovoEncaminhamentoNAAPA.ObterHistoricosDeAlteracoesNovoEncaminhamentoNAAPA;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.NovoEncaminhamentoNAAPA.RegistrarHistoricoDeAlteracaoNovoEncaminhamentoNAAPA
{
    public class RegistrarHistoricoDeAlteracaoNovoEncaminhamentoNAAPACommandHandler : IRequestHandler<RegistrarHistoricoDeAlteracaoNovoEncaminhamentoNAAPACommand, long>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioNovoEncaminhamentoNAAPAHistoricoAlteracoes repositorioNovoEncaminhamentoNAAPAHistoricoAlteracoes;

        public RegistrarHistoricoDeAlteracaoNovoEncaminhamentoNAAPACommandHandler(IMediator mediator, IRepositorioNovoEncaminhamentoNAAPAHistoricoAlteracoes repositorioNovoEncaminhamentoNAAPAHistoricoAlteracoes)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioNovoEncaminhamentoNAAPAHistoricoAlteracoes = repositorioNovoEncaminhamentoNAAPAHistoricoAlteracoes ?? throw new ArgumentNullException(nameof(repositorioNovoEncaminhamentoNAAPAHistoricoAlteracoes));
        }

        public async Task<long> Handle(RegistrarHistoricoDeAlteracaoNovoEncaminhamentoNAAPACommand request, CancellationToken cancellationToken)
        {
            var historicoAlteracao = await mediator.Send(new ObterHistoricosDeAlteracoesNovoEncaminhamentoNAAPAQuery(request.NovoEncaminhamentoNAAPASecaoAlterado, request.NovoEncaminhamentoNAAPASecaoExistente, request.TipoHistoricoAlteracoes));

            if (historicoAlteracao.NaoEhNulo())
                return await repositorioNovoEncaminhamentoNAAPAHistoricoAlteracoes.SalvarAsync(historicoAlteracao);

            return 0;
        }
    }
}