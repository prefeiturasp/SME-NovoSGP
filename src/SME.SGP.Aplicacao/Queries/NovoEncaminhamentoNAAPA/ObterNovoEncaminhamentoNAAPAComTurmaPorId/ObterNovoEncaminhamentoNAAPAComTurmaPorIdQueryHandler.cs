using MediatR;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.NovoEncaminhamentoNAAPA.ObterNovoEncaminhamentoNAAPAComTurmaPorId
{
    public class ObterNovoEncaminhamentoNAAPAComTurmaPorIdQueryHandler : IRequestHandler<ObterNovoEncaminhamentoNAAPAComTurmaPorIdQuery, EncaminhamentoEscolar>
    {
        IRepositorioNovoEncaminhamentoNAAPA repositorioNovoEncaminhamentoNAAPA { get; }

        public ObterNovoEncaminhamentoNAAPAComTurmaPorIdQueryHandler(IRepositorioNovoEncaminhamentoNAAPA repositorioNovoEncaminhamentoNAAPA)
        {
            this.repositorioNovoEncaminhamentoNAAPA = repositorioNovoEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioNovoEncaminhamentoNAAPA));
        }

        public async Task<EncaminhamentoEscolar> Handle(ObterNovoEncaminhamentoNAAPAComTurmaPorIdQuery request, CancellationToken cancellationToken)
            => await repositorioNovoEncaminhamentoNAAPA.ObterEncaminhamentoComTurmaPorId(request.EncaminhamentoId);
    }
}