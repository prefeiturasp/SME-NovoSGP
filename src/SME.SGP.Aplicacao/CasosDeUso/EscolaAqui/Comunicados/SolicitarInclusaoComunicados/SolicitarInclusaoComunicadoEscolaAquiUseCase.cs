using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SolicitarInclusaoComunicadoEscolaAquiUseCase : ISolicitarInclusaoComunicadoEscolaAquiUseCase
    {
        private const string TODAS = "todas";
        private readonly IMediator mediator;       

        public SolicitarInclusaoComunicadoEscolaAquiUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));            
        }

        public async Task<string> Executar(ComunicadoInserirDto comunicado)
        {
            await ValidarInsercao(comunicado);

            var retorno = await mediator.Send(new InserirComunicadoCommand(comunicado.Titulo,
                                                                           comunicado.Descricao,
                                                                           comunicado.DataEnvio,
                                                                           comunicado.DataExpiracao,
                                                                           comunicado.AnoLetivo,
                                                                           comunicado.CodigoDre,
                                                                           comunicado.CodigoUe,
                                                                           comunicado.Turmas,
                                                                           comunicado.AlunosEspecificados,
                                                                           comunicado.Modalidades,
                                                                           comunicado.Semestre,
                                                                           comunicado.Alunos,
                                                                           comunicado.SeriesResumidas,
                                                                           comunicado.TipoCalendarioId,
                                                                           comunicado.EventoId));

            //TODO : Ajustar retorno quando refatorar a tela

            if(retorno)
                return "Comunicado criado com sucesso!";
            else
                return "Erro ao criar o comunicado";
        }

        private async Task ValidarInsercao(ComunicadoInserirDto comunicado)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            await ValidarAbrangenciaUsuario(comunicado, usuarioLogado);

            if (comunicado.CodigoDre == TODAS && !comunicado.CodigoUe.Equals(TODAS))
                throw new NegocioException("Não é possivel especificar uma escola quando o comunicado é para todas as DREs");

            if (comunicado.CodigoUe == TODAS && comunicado.Turmas.Any())
                throw new NegocioException("Não é possivel especificar uma turma quando o comunicado é para todas as UEs");

            if ((comunicado.Turmas == null || !comunicado.Turmas.Any()) && (comunicado.AlunosEspecificados || (comunicado.Alunos?.Any() ?? false)))
                throw new NegocioException("Não é possivel especificar alunos quando o comunicado é para todas as Turmas");
        }

        private async Task ValidarAbrangenciaUsuario(ComunicadoInserirDto comunicado, Usuario usuarioLogado)
        {
            if (comunicado.CodigoDre == TODAS && !usuarioLogado.EhPerfilSME())
                throw new NegocioException("Apenas usuários SME podem realizar envio de Comunicados para todas as DREs");

            if (comunicado.CodigoUe == TODAS && !(usuarioLogado.EhPerfilDRE() || usuarioLogado.EhPerfilSME()))
                throw new NegocioException("Apenas usuários SME e DRE podem realizar envio de Comunicados para todas as Escolas");

            if (usuarioLogado.EhPerfilDRE() && !comunicado.CodigoDre.Equals(TODAS))
                await ValidarAbrangenciaDre(comunicado, usuarioLogado);

            if (usuarioLogado.EhPerfilUE() && !comunicado.CodigoUe.Equals(TODAS))
                await ValidarAbrangenciaUE(comunicado, usuarioLogado);
        }

        private async Task ValidarAbrangenciaUE(ComunicadoInserirDto comunicado, Usuario usuarioLogado)
        {
            var abrangenciaUes = await mediator.Send(new ObterAbrangenciaUesPorLoginEPerfilQuery(comunicado.CodigoDre, usuarioLogado.Login, usuarioLogado.PerfilAtual));

            var ue = abrangenciaUes.FirstOrDefault(x => x.Codigo.Equals(comunicado.CodigoUe));
            
            if (ue == null)
                throw new NegocioException($"Usuário não possui permissão para enviar comunicados para a UE com codigo {comunicado.CodigoUe}");

            if (comunicado.Turmas != null && comunicado.Turmas.Any())
                await ValidarAbrangenciaTurma(comunicado, usuarioLogado);
        }

        private async Task ValidarAbrangenciaDre(ComunicadoInserirDto comunicado, Usuario usuarioLogado)
        {            
            var abrangenciaDres = await mediator.Send(new ObterAbrangenciaDresPorLoginEPerfilQuery(usuarioLogado.Login, usuarioLogado.PerfilAtual));

            var dre = abrangenciaDres.FirstOrDefault(x => x.Codigo.Equals(comunicado.CodigoDre));

            if (dre == null)
                throw new NegocioException($"Usuário não possui permissão para enviar comunicados para a DRE com codigo {comunicado.CodigoDre}");
        }

        private async Task ValidarAbrangenciaTurma(ComunicadoInserirDto comunicado, Usuario usuarioLogado)
        {
            var abrangencia = await mediator.Send(new ObterAbrangenciaCompactaVigenteEolPorLoginEPerfilQuery(usuarioLogado.Login, usuarioLogado.PerfilAtual));
            bool abrangenciaPermitida = abrangencia.Abrangencia.Abrangencia == Infra.Enumerados.Abrangencia.UE
                                        || abrangencia.Abrangencia.Abrangencia == Infra.Enumerados.Abrangencia.Dre
                                        || abrangencia.Abrangencia.Abrangencia == Infra.Enumerados.Abrangencia.SME;

            
            foreach (var turma in comunicado.Turmas)
            {
                var abrangenciaTurmas = await mediator.Send(new ObterAbrangenciaTurmaQuery(turma, usuarioLogado.Login, usuarioLogado.PerfilAtual, false, abrangenciaPermitida)); 

                if (abrangenciaTurmas == null)
                    throw new NegocioException($"Usuário não possui permissão para enviar comunicados para a Turma com codigo {turma}");
            }
        }        
    }
}
