using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
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

            var listaTitulares = await mediator.Send(new ObterProfessoresTitularesDisciplinasEolQuery(turma.CodigoTurma));
            var titulares = listaTitulares?.Select(x => x.ProfessorRf);
            if (titulares.NaoEhNulo())
            {
                var mensagem = new StringBuilder($"As seguintes aulas da turma {turma.ModalidadeCodigo.ShortName()} - {turma.Nome} da {turma.Ue.TipoEscola.ShortName()} {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao}) não foram excluídas pela rotina automática pois já possuem registros de frequência.<br>");
                foreach (var data in datasAulas)
                {
                    mensagem.AppendLine($"<br>{data:dd/MM/yyyy}");
                }
                mensagem.AppendLine("<br><br>As aulas deverão ser excluídas manualmente.");

                if (titulares.Count() == 1)
                    titulares = titulares.FirstOrDefault().Split(',');

                foreach (var titular in titulares)
                {
                    var codigoRf = titular.Trim();
                    if (!string.IsNullOrEmpty(codigoRf))
                    {
                        var usuario = await mediator.Send(new ObterUsuarioPorRfQuery(codigoRf));
                        if (usuario.NaoEhNulo())
                            await mediator.Send(new NotificarUsuarioCommand($"Problemas na exclusão de aulas da turma {turma.ModalidadeCodigo.ShortName()} - {turma.Nome}",
                                                                            mensagem.ToString(),
                                                                            codigoRf,
                                                                            NotificacaoCategoria.Aviso,
                                                                            NotificacaoTipo.Calendario));
                    }
                }
                return true;
            }
            return false;
        }
    }
}
