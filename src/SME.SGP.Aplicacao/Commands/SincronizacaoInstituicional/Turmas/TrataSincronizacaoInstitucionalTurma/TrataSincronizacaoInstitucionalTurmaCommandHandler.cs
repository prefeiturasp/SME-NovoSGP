using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class TrataSincronizacaoInstitucionalTurmaCommandHandler : IRequestHandler<TrataSincronizacaoInstitucionalTurmaCommand, bool>
    {
        private readonly IRepositorioTurma repositorioTurma;

        public TrataSincronizacaoInstitucionalTurmaCommandHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public Task<bool> Handle(TrataSincronizacaoInstitucionalTurmaCommand request, CancellationToken cancellationToken)
        {
            // TODO : Verificar se a turma está concluída para marcar como histórica
            var turma = request.Turma;

            if(turma.Situacao == "C")
            {
                // MArcar como histórica
            }

            // TODO : Verificar se não tiver calendário cadastrado e a turma for extinta deve excluir direto 

            // TODO : Obter o tipo de calendário pela modalidade da turma 

            // - Obter os periodos do tipo de calendário
            // - Obter o primeiro periodo e sua data inicial 
            // - Se a Data da extinção da turma for menor que a Inicial do periodo, deve-se deletar a turma do SGP 
            //   caso contrário, deve-se marcar a turma como histórica



            throw new NotImplementedException();
        }
    }
}
