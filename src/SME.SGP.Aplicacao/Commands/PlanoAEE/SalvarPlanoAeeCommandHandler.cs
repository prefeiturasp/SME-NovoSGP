using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands
{
    public class SalvarPlanoAeeCommandHandler : IRequestHandler<SalvarPlanoAeeCommand, RetornoPlanoAEEDto>
    {

        private readonly IRepositorioPlanoAEE repositorioPlanoAEE;
        private readonly IRepositorioPlanoAEEVersao repositorioPlanoAEEVersao;

        public SalvarPlanoAeeCommandHandler(IRepositorioPlanoAEE repositorioPlanoAEE, IRepositorioPlanoAEEVersao repositorioPlanoAEEVersao)
        {
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
            this.repositorioPlanoAEEVersao = repositorioPlanoAEEVersao ?? throw new ArgumentNullException(nameof(repositorioPlanoAEEVersao));
        }

        public async Task<RetornoPlanoAEEDto> Handle(SalvarPlanoAeeCommand request, CancellationToken cancellationToken)
        {
            var plano = MapearParaEntidade(request);
            var planoId = await repositorioPlanoAEE.SalvarAsync(plano);

            // PlanoAEE
            var planoAEEEntidade = await repositorioPlanoAEE.ObterPorIdAsync(planoId);

            // Última versão plano
            int ultimaVersaoPlanoAee = await ObterUltimaVersaoPlanoAEE(plano.AlunoCodigo);

            // Salva Versao
            var planoAEEVersaoId = await SalvarPlanoAEEVersao(planoId, planoAEEEntidade, ultimaVersaoPlanoAee);

            return new RetornoPlanoAEEDto(planoId, planoAEEVersaoId);
        }

        private async Task<long> SalvarPlanoAEEVersao(long planoId, PlanoAEE planoAEEEntidade, int ultimaVersaoPlanoAee)
        {
            var planoVersaoEntidade = new PlanoAEEVersao
            {
                PlanoAEE = planoAEEEntidade,
                PlanoAEEId = planoId,
                Numero = ultimaVersaoPlanoAee
            };
            return await repositorioPlanoAEEVersao.SalvarAsync(planoVersaoEntidade);
        }

        private async Task<int> ObterUltimaVersaoPlanoAEE(string alunoCodigo)
        {
            int ultimaVersaoPlanoAee = 0;

            var maiorNumeroPlano = await repositorioPlanoAEEVersao.ObterMaiorVersaoPlanoPorAlunoCodigo(alunoCodigo);

            if (maiorNumeroPlano > 0)
                ultimaVersaoPlanoAee = maiorNumeroPlano;
            return ultimaVersaoPlanoAee + 1;
        }

        private PlanoAEE MapearParaEntidade(SalvarPlanoAeeCommand request)
            => new PlanoAEE()
            {
                TurmaId = request.TurmaId,
                Situacao = request.Situacao,
                AlunoCodigo = request.AlunoCodigo,
                AlunoNumero = request.AlunoNumero,
                AlunoNome = request.AlunoNome,
                Questoes = new System.Collections.Generic.List<PlanoAEEQuestao>()
            };
    }
}
