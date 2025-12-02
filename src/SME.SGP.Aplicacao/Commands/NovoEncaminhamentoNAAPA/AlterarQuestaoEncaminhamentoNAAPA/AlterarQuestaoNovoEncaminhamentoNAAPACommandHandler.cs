using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.NovoEncaminhamentoNAAPA.AlterarQuestaoEncaminhamentoNAAPA
{
    public class AlterarQuestaoNovoEncaminhamentoNAAPACommandHandler : IRequestHandler<AlterarQuestaoNovoEncaminhamentoNAAPACommand, bool>
    {
        private readonly IRepositorioQuestaoNovoEncaminhamentoNAAPA repositorioQuestaoNovoEncaminhamentoNAAPA;

        public AlterarQuestaoNovoEncaminhamentoNAAPACommandHandler(IRepositorioQuestaoNovoEncaminhamentoNAAPA repositorioQuestaoNovoEncaminhamentoNAAPA)
        {
            this.repositorioQuestaoNovoEncaminhamentoNAAPA = repositorioQuestaoNovoEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioQuestaoNovoEncaminhamentoNAAPA));
        }

        public async Task<bool> Handle(AlterarQuestaoNovoEncaminhamentoNAAPACommand request, CancellationToken cancellationToken)
        {
            await repositorioQuestaoNovoEncaminhamentoNAAPA.SalvarAsync(request.Questao);
            return true;
        }
    }
}