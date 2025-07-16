using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarExclusaoAulaComFrequenciaUseCase : INotificarExclusaoAulaComFrequenciaUseCase
    {
        private readonly IMediator mediator;

        public NotificarExclusaoAulaComFrequenciaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var dadosMensagem = mensagemRabbit.ObterObjetoMensagem<NotificarExclusaoAulasComFrequenciaDto>();
            var turma = dadosMensagem.Turma;
            var datasAulas = dadosMensagem.DatasAulas;

            var titulares = await ObterRfsTitulares(turma.CodigoTurma);
            if (titulares.NaoEhNulo())
            {
                var mensagem = ConstruirMensagem(turma, datasAulas);

                var listaRfs = ObterListaRfs(titulares);

                await NotificarTitulares(listaRfs, mensagem, turma);

                return true;
            }

            return false;
        }

        private async Task<IEnumerable<string>> ObterRfsTitulares(string codigoTurma)
        {
            var listaTitulares = await mediator.Send(new ObterProfessoresTitularesDisciplinasEolQuery(codigoTurma));
            return listaTitulares?.Select(x => x.ProfessorRf);
        }

        private static string ConstruirMensagem(Turma turma, IEnumerable<DateTime> datasAulas)
        {
            var mensagem = new StringBuilder($"As seguintes aulas da turma {turma.ModalidadeCodigo.ShortName()} - {turma.Nome} da {turma.Ue.TipoEscola.ShortName()} {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao}) não foram excluídas pela rotina automática pois já possuem registros de frequência.<br>");

            foreach (var data in datasAulas)
                mensagem.AppendLine($"<br>{data:dd/MM/yyyy}");

            mensagem.AppendLine("<br><br>As aulas deverão ser excluídas manualmente.");
            return mensagem.ToString();
        }

        private static IEnumerable<string> ObterListaRfs(IEnumerable<string> titulares)
        {
            if (titulares.Count() == 1)
                return titulares.First().Split(',').Select(rf => rf.Trim());

            return titulares.Select(rf => rf.Trim());
        }

        private async Task NotificarTitulares(IEnumerable<string> rfs, string mensagem, Turma turma)
        {
            foreach (var rf in rfs)
            {
                if (!string.IsNullOrEmpty(rf))
                {
                    var usuario = await mediator.Send(new ObterUsuarioPorRfQuery(rf));
                    if (usuario.NaoEhNulo())
                    {
                        await mediator.Send(new NotificarUsuarioCommand(
                            $"Problemas na exclusão de aulas da turma {turma.ModalidadeCodigo.ShortName()} - {turma.Nome}",
                            mensagem,
                            rf,
                            NotificacaoCategoria.Aviso,
                            NotificacaoTipo.Calendario));
                    }
                }
            }
        }

    }
}
