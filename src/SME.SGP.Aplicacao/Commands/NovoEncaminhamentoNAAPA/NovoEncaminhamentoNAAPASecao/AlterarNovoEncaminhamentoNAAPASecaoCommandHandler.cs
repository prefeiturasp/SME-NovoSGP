using MediatR;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.NovoEncaminhamentoNAAPA.NovoEncaminhamentoNAAPASecao
{
    public class AlterarNovoEncaminhamentoNAAPASecaoCommandHandler : IRequestHandler<AlterarNovoEncaminhamentoNAAPASecaoCommand, AuditoriaDto>
    {
        private readonly IRepositorioNovoEncaminhamentoNAAPASecao repositorioNovoEncaminhamentoNAAPASecao;

        public AlterarNovoEncaminhamentoNAAPASecaoCommandHandler(IRepositorioNovoEncaminhamentoNAAPASecao repositorioNovoEncaminhamentoNAAPASecao)
        {
            this.repositorioNovoEncaminhamentoNAAPASecao = repositorioNovoEncaminhamentoNAAPASecao ?? throw new ArgumentNullException(nameof(repositorioNovoEncaminhamentoNAAPASecao));
        }

        public async Task<AuditoriaDto> Handle(AlterarNovoEncaminhamentoNAAPASecaoCommand request, CancellationToken cancellationToken)
        {
            await repositorioNovoEncaminhamentoNAAPASecao.SalvarAsync(request.Secao);

            return (AuditoriaDto)request.Secao;
        }
    }
}