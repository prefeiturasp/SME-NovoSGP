using MediatR;
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

namespace SME.SGP.Aplicacao.Commands.NovoEncaminhamentoNAAPA.RegistrarNovoEncaminhamentoNAAPASecao
{
    public class RegistrarNovoEncaminhamentoNAAPASecaoCommandHandler : IRequestHandler<RegistrarNovoEncaminhamentoNAAPASecaoCommand, EncaminhamentoNAAPASecao>
    {
        private readonly IRepositorioNovoEncaminhamentoNAAPASecao repositorioNovoEncaminhamentoNAAPASecao;

        public RegistrarNovoEncaminhamentoNAAPASecaoCommandHandler(IRepositorioNovoEncaminhamentoNAAPASecao repositorioNovoEncaminhamentoNAAPASecao)
        {
            this.repositorioNovoEncaminhamentoNAAPASecao = repositorioNovoEncaminhamentoNAAPASecao ?? throw new ArgumentNullException(nameof(repositorioNovoEncaminhamentoNAAPASecao));
        }

        public async Task<EncaminhamentoNAAPASecao> Handle(RegistrarNovoEncaminhamentoNAAPASecaoCommand request, CancellationToken cancellationToken)
        {
            var secao = MapearParaEntidade(request);
            await repositorioNovoEncaminhamentoNAAPASecao.SalvarAsync(secao);
            return secao;
        }

        private EncaminhamentoNAAPASecao MapearParaEntidade(RegistrarNovoEncaminhamentoNAAPASecaoCommand request)
            => new()
            {
                SecaoEncaminhamentoNAAPAId = request.SecaoId,
                Concluido = request.Concluido,
                EncaminhamentoNAAPAId = request.EncaminhamentoNAAPAId
            };
    }
}