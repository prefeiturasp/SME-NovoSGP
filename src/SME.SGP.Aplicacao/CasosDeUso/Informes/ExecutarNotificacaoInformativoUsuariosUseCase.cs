using MediatR;
using Minio.DataModel;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarNotificacaoInformativoUsuariosUseCase : AbstractUseCase, IExecutarNotificacaoInformativoUsuariosUseCase
    {
        public ExecutarNotificacaoInformativoUsuariosUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var param = mensagem.ObterObjetoMensagem<string>();
            if (string.IsNullOrEmpty(param)) return false;
            var informativoId = long.Parse(param);

            var informativo = await mediator.Send(new ObterInformesPorIdQuery(informativoId));
            if (informativo.EhNulo())
                throw new NegocioException("Não foi possível encontrar o Informativo informado");

            var guidPerfis = await ObterPerfisPorCodigos(informativo.Perfis.Select(perfil => perfil.CodigoPerfil));
            var usuarios = await ObterUsuarios(informativo, guidPerfis);
            foreach (var usuario in usuarios)
            {
                var notificacaoInformativoUsuario = new NotificacaoInformativoUsuarioFiltro()
                {
                    InformativoId = informativoId,
                    UsuarioRf = usuario,
                    Titulo = informativo.Titulo,
                    Mensagem = informativo.Texto,
                    DreCodigo = informativo.Dre?.CodigoDre,
                    UeCodigo = informativo.Ue?.CodigoUe
                };
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaNotificacaoInformativoUsuario, notificacaoInformativoUsuario, Guid.NewGuid(), null));  
            }               
            return true;
        }

        private async Task<string[]> ObterUsuarios(Informativo informativo, string[] guidPerfis)
        {
            var usuarios = new List<string>();
            if (informativo.Ue?.CodigoUe is null 
                && informativo.Dre?.CodigoDre is null)
            {
                var dres = await mediator.Send(ObterTodasDresQuery.Instance);
                var tasks = dres
                     .AsParallel()
                     .WithDegreeOfParallelism(4)
                     .Select(async dre =>
                     {
                         usuarios.AddRange(await mediator.Send(new ObterRfsUsuariosPorPerfisDreUeQuery(informativo.Ue?.CodigoUe, dre.CodigoDre, guidPerfis)));
                     });

                await Task.WhenAll(tasks);
                    
            }
            else
                usuarios.AddRange(await mediator.Send(new ObterRfsUsuariosPorPerfisDreUeQuery(informativo.Ue?.CodigoUe, informativo.Dre?.CodigoDre, guidPerfis)));

            return usuarios.Distinct().ToArray();
        }

        private async Task<string[]> ObterPerfisPorCodigos(IEnumerable<long> codigosPerfis)
        {
            var perfis = await mediator.Send(new ObterGruposDeUsuariosQuery((int)TipoPerfil.SME));
            return perfis.Where(perfil => codigosPerfis.Contains(perfil.Id)).Select(perfil => perfil.GuidPerfil.ToString()).ToArray();
        }
    }
}