using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class EnviarNotificacaoCriacaoPlanoAEECommandHandler : IRequestHandler<EnviarNotificacaoCriacaoPlanoAEECommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IConfiguration configuration;

        public EnviarNotificacaoCriacaoPlanoAEECommandHandler(IMediator mediator, IConfiguration configuration)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<bool> Handle(EnviarNotificacaoCriacaoPlanoAEECommand request, CancellationToken cancellationToken)
        {
            var planoAEE = await mediator.Send(new ObterPlanoAEEComTurmaUeEDrePorIdQuery(request.PlanoAEEId));

            if (planoAEE == null)
                throw new NegocioException($"O PlanoAEE [{request.PlanoAEEId}] não foi localizado.");

            var ueDre = $"{planoAEE.Turma.Ue.TipoEscola.ShortName()} {planoAEE.Turma.Ue.Nome} ({planoAEE.Turma.Ue.Dre.Abreviacao})";
            var hostAplicacao = configuration["UrlFrontEnd"];
            var estudanteOuCrianca = planoAEE.Turma.ModalidadeCodigo == Modalidade.EducacaoInfantil ? "da criança" : "do estudante";

            var titulo = $"Novo plano AEE - {planoAEE.AlunoNome} ({planoAEE.AlunoCodigo}) - {ueDre}";
            var descricao = $@"O Plano AEE {estudanteOuCrianca} {planoAEE.AlunoNome} ({planoAEE.AlunoCodigo}) da turma {planoAEE.Turma.NomeComModalidade()} da {ueDre} foi cadastrado no sistema.<br/>
                            <a href='{hostAplicacao}aee/plano/editar/{planoAEE.Id}'>Clique aqui</a> para acessar o plano AEE. ";

            var usuariosIds = await ObterUsuarios(planoAEE.Turma.Ue.CodigoUe, planoAEE.Turma.Ue.Dre.CodigoDre);

            if (usuariosIds.Any())
                await mediator.Send(new GerarNotificacaoPlanoAEECommand(planoAEE.Id, usuariosIds, titulo, descricao, NotificacaoPlanoAEETipo.PlanoCriado, NotificacaoCategoria.Aviso));

            return true;
        }

        private async Task<IEnumerable<long>> ObterUsuarios(string ueCodigo, string dreCodigo)
        {
            var coordenadoresUe = await ObterCoordenadoresUe(ueCodigo);

            var usuariosIds = await ObterUsuariosId(coordenadoresUe);
            var coordenadorCEFAI = await mediator.Send(new ObtemUsuarioCEFAIDaDreQuery(dreCodigo));

            if (coordenadorCEFAI != 0)
                usuariosIds.Add(coordenadorCEFAI);

            return usuariosIds;
        }

        private async Task<List<string>> ObterCoordenadoresUe(string codigoUe)
        {
            var funcionariosCP = await mediator.Send(new ObterFuncionariosPorUeECargoQuery(codigoUe, (int)Cargo.CP));
            if (funcionariosCP.Any())
                return funcionariosCP.Select(f => f.CodigoRF).ToList();

            var funcionariosAD = await mediator.Send(new ObterFuncionariosPorUeECargoQuery(codigoUe, (int)Cargo.AD));
            if (funcionariosAD.Any())
                return funcionariosAD.Select(f => f.CodigoRF).ToList();

            var funcionariosDiretor = await mediator.Send(new ObterFuncionariosPorUeECargoQuery(codigoUe, (int)Cargo.Diretor));
            if (funcionariosDiretor.Any())
                return funcionariosDiretor.Select(f => f.CodigoRF).ToList();

            return null;
        }

        private async Task<List<long>> ObterUsuariosId(List<string> funcionarios)
        {
            List<long> usuarios = new List<long>();
            foreach (var functionario in funcionarios)
            {
                var usuario = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(functionario));
                usuarios.Add(usuario);
            }
            return usuarios;
        }
    }
}
