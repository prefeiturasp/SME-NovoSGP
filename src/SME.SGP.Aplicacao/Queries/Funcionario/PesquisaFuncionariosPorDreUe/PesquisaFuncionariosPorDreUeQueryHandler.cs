using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PesquisaFuncionariosPorDreUeQueryHandler : IRequestHandler<PesquisaFuncionariosPorDreUeQuery, IEnumerable<UsuarioEolRetornoDto>>
    {
        private readonly IServicoEol servicoEOL;
        private readonly IMediator mediator;

        public PesquisaFuncionariosPorDreUeQueryHandler(IServicoEol servicoEOL, IMediator mediator)
        {
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<UsuarioEolRetornoDto>> Handle(PesquisaFuncionariosPorDreUeQuery request, CancellationToken cancellationToken)
        {
            ValidaFiltros(request);

            var filtroCodigos = await ObterCodigosFiltros(request.CodigoDRE, request.CodigoUE, request.CodigoTurma);

            var usuario = request.Usuario ?? await mediator.Send(new ObterUsuarioLogadoQuery());

            FiltroFuncionarioDto filtro = new FiltroFuncionarioDto()
            {
                CodigoDRE = filtroCodigos.codigoDRE,
                CodigoUE = filtroCodigos.codigoUE,
                CodigoRF = request.CodigoRF,
                NomeServidor = request.Nome,
            };

            return await CarregarUsuarioId(await servicoEOL.ObterFuncionariosPorDre(usuario.PerfilAtual, filtro));
        }

        private async Task<IEnumerable<UsuarioEolRetornoDto>> CarregarUsuarioId(IEnumerable<UsuarioEolRetornoDto> funcionarios)
        {
            var rfs = funcionarios.Select(a => a.CodigoRf)
                .Distinct()
                .ToList();

            var usuarios = await mediator.Send(new ObterUsuariosPorCodigosRfQuery(rfs));

            if (usuarios != null)
                foreach(var funcionario in funcionarios)
                {
                    funcionario.UsuarioId = usuarios.FirstOrDefault(a => a.CodigoRf == funcionario.CodigoRf)?.Id ?? 0;
                }

            return funcionarios;
        }

        private async Task<(string codigoDRE, string codigoUE)> ObterCodigosFiltros(string codigoDRE, string codigoUE, string codigoTurma)
        {
            if (!string.IsNullOrEmpty(codigoTurma))
            {
                var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(codigoTurma));
                return (turma.Ue.Dre.CodigoDre, turma.Ue.CodigoUe);
            }

            if (string.IsNullOrEmpty(codigoDRE))
            {
                var ue = await mediator.Send(new ObterUeComDrePorCodigoQuery(codigoUE));
                return (ue.Dre.CodigoDre, ue.CodigoUe);
            }

            return (codigoDRE, codigoUE);
        }

        private void ValidaFiltros(PesquisaFuncionariosPorDreUeQuery request)
        {
            //if (string.IsNullOrEmpty(request.CodigoRF) && string.IsNullOrEmpty(request.Nome))
            //    throw new NegocioException("O código RF ou nome do servidor deve ser informado.");

            if (string.IsNullOrEmpty(request.CodigoTurma) && string.IsNullOrEmpty(request.CodigoDRE) && string.IsNullOrEmpty(request.CodigoUE))
                throw new NegocioException("O código DRE/UE ou código da turma deve ser informado para pesquisa de servidores");
        }
    }
}
