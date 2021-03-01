using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class GerarPendenciaCPEncerramentoPlanoAEECommandHandler : IRequestHandler<GerarPendenciaCPEncerramentoPlanoAEECommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration configuration;

        public GerarPendenciaCPEncerramentoPlanoAEECommandHandler(IMediator mediator, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<bool> Handle(GerarPendenciaCPEncerramentoPlanoAEECommand request, CancellationToken cancellationToken)
        {
            var planoAEE = await mediator.Send(new ObterPlanoAEEComTurmaPorIdQuery(request.PlanoAEEId));

            if (planoAEE == null)
                throw new NegocioException("Não foi possível localizar o PlanoAEE");

            if (planoAEE.Situacao == SituacaoPlanoAEE.DevolutivaCP)
            {
                var ue = await mediator.Send(new ObterUeComDrePorCodigoQuery(planoAEE.Turma.CodigoTurma));

                var funcionarios = await ObterFuncionarios(ue.CodigoUe);

                if (funcionarios == null)
                    return false;

                var usuarios = await ObterUsuariosId(funcionarios);

                var usuarioCEFAIId = await mediator.Send(new ObtemUsuarioCEFAIDaDreQuery(ue.Dre.CodigoDre));
                
                if (planoAEE.ResponsavelId.GetValueOrDefault() > 0)
                    usuarios.Add(planoAEE.ResponsavelId.GetValueOrDefault());

                var ueDre = $"{ue.TipoEscola.ShortName()} {ue.Nome} ({ue.Dre.Abreviacao})";
                var hostAplicacao = configuration["UrlFrontEnd"];
                var estudanteOuCrianca = planoAEE.Turma.ModalidadeCodigo == Modalidade.Infantil ? "da criança" : "do estudante";

                var titulo = $"Plano AEE a encerrar - {planoAEE.AlunoNome} ({planoAEE.AlunoCodigo}) - {ueDre}";
                var descricao = $"Foi solicitado o encerramento do Plano AEE {estudanteOuCrianca} {planoAEE.AlunoNome} ({planoAEE.AlunoCodigo}) da turma {planoAEE.Turma.NomeComModalidade()} da {ueDre}. <br/><a href='{hostAplicacao}relatorios/aee/plano/editar/{planoAEE.Id}'>Clique aqui para acessar o plano e registrar a devolutiva.</a> " +
                    $"<br/><br/>A pendência será resolvida automaticamente após este registro.";

                using (var transacao = unitOfWork.IniciarTransacao())
                {
                    try
                    {
                        if(usuarioCEFAIId > 0)
                        {
                            var descricaoDEFAI = $"Foi solicitado o encerramento do Plano AEE {estudanteOuCrianca} {planoAEE.AlunoNome} ({planoAEE.AlunoCodigo}) da turma {planoAEE.Turma.NomeComModalidade()} da {ueDre}. <br/><a href='{hostAplicacao}relatorios/aee/plano/editar/{planoAEE.Id}'>Clique aqui para acessar o plano e atribuir um PAAI para analisar e realizar a devolutiva.</a> " +
                                $"<br/><br/>A pendência será resolvida automaticamente após este registro.";
                            var pendenciaId = await mediator.Send(new SalvarPendenciaCommand(TipoPendencia.AEE, descricao, "", titulo));
                            var pendenciaUsuarioId = await mediator.Send(new SalvarPendenciaUsuarioCommand(pendenciaId, usuarioCEFAIId));
                            await mediator.Send(new SalvarPendenciaPlanoAEECommand(pendenciaId, planoAEE.Id));
                        }


                        foreach (var usuario in usuarios)
                        {
                            var pendenciaId = await mediator.Send(new SalvarPendenciaCommand(TipoPendencia.AEE, descricao, "", titulo));
                            var pendenciaUsuarioId = await mediator.Send(new SalvarPendenciaUsuarioCommand(pendenciaId, usuario));
                            await mediator.Send(new SalvarPendenciaPlanoAEECommand(pendenciaId, planoAEE.Id));
                        }

                        unitOfWork.PersistirTransacao();
                        return true;
                    }
                    catch (Exception e)
                    {
                        unitOfWork.Rollback();
                        throw;
                    }
                }
            }
            return false;
        }

        private async Task<List<string>> ObterFuncionarios(string codigoUe)
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
