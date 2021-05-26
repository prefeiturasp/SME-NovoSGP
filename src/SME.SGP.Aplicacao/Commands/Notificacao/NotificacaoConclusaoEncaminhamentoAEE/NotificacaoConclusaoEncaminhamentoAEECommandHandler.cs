using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificacaoConclusaoEncaminhamentoAEECommandHandler : IRequestHandler<NotificacaoConclusaoEncaminhamentoAEECommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IConfiguration configuration;

        public NotificacaoConclusaoEncaminhamentoAEECommandHandler(IMediator mediator, IConfiguration configuration)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<bool> Handle(NotificacaoConclusaoEncaminhamentoAEECommand request, CancellationToken cancellationToken)
        {

            var encaminhamentoAEE = await mediator.Send(new ObterEncaminhamentoAEEPorIdQuery(request.EncaminhamentoAEEId));
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(encaminhamentoAEE.TurmaId));

            var funcionarios = await ObterFuncionarios(turma.Ue.CodigoUe);

            if (funcionarios == null)
                return false;

            funcionarios.Add(encaminhamentoAEE.CriadoRF);

            var usuarios = await ObterUsuariosId(funcionarios);
            var situacao = encaminhamentoAEE.Situacao == Dominio.Enumerados.SituacaoAEE.Deferido ? "deferiu" : "indeferiu";
            var ueDre = $"{turma.Ue.TipoEscola.ShortName()} {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao})";
            var hostAplicacao = configuration["UrlFrontEnd"];
            var estudanteOuCrianca = turma.ModalidadeCodigo == Modalidade.InfantilPreEscola ? "da criança" : "do estudante";

            var titulo = $"Encaminhamento AEE - {encaminhamentoAEE.AlunoNome} ({encaminhamentoAEE.AlunoCodigo}) - {ueDre}";
            var mensagem = $"O usuário {request.UsuarioNome} ({request.UsuarioRF}) <b>{situacao}</b> o encaminhamento {estudanteOuCrianca} {encaminhamentoAEE.AlunoNome} ({encaminhamentoAEE.AlunoCodigo}) " +
                $"da turma {turma.ModalidadeCodigo.ShortName()}-{turma.Nome} da {ueDre}.<br/><br/>" +
                $"<a href='{hostAplicacao}aee/encaminhamento/editar/{encaminhamentoAEE.Id}'>Clique aqui para acessar o encaminhamento.</a>";


            

            await mediator.Send(new EnviarNotificacaoUsuariosCommand(titulo,
                                                                     mensagem,
                                                                     NotificacaoCategoria.Alerta,
                                                                     NotificacaoTipo.AEE,
                                                                     usuarios)); ;

            return true;
        }

        private async Task<List<long>> ObterUsuariosId(List<string> funcionarios)
        {
            List<long> usuarios = new List<long>();
            foreach(var functionario in funcionarios)
            {
                var usuario = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(functionario));
                usuarios.Add(usuario);
            }
            return usuarios;
        }

        private async Task<List<string>> ObterFuncionarios(string codigoUe)
        {

            var funcionariosCP = await mediator.Send(new ObterFuncionariosPorUeECargoQuery(codigoUe, (int)Cargo.CP));
            if(funcionariosCP.Any())
                return funcionariosCP.Select(f => f.CodigoRF).ToList();

            var funcionariosAD = await mediator.Send(new ObterFuncionariosPorUeECargoQuery(codigoUe, (int)Cargo.AD));
            if (funcionariosAD.Any())
                return funcionariosAD.Select(f => f.CodigoRF).ToList();

            var funcionariosDiretor = await mediator.Send(new ObterFuncionariosPorUeECargoQuery(codigoUe, (int)Cargo.Diretor));
            if (funcionariosDiretor.Any())
                return funcionariosDiretor.Select(f => f.CodigoRF).ToList();

            return null;
        }
    }
}
