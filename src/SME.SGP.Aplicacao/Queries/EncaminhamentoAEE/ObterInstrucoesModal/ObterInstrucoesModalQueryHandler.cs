using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterInstrucoesModalQueryHandler : IRequestHandler<ObterInstrucoesModalQuery, string>
    {
        public async Task<string> Handle(ObterInstrucoesModalQuery request, CancellationToken cancellationToken)
        {
            var instruoesModal = @"<b>O preenchimento deste encaminhamento é exclusivo para o público da Educação Especial. Segundo as normativas do município, o Atendimento Educação Especializado (AEE) é um serviço a ser ofertado exclusivamente para Estudantes/Crianças nos seguintes casos:</b> <br/><br/><p>
                                      1 - Com Deficiência <br/>
                                      2 - Com Transtornos Globais do Desenvolvimento (TGD) <br/>
                                      3 - Com Altas Habilidades / Superdotação</p>";

            return instruoesModal;
        }
    }
}
