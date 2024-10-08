﻿using MediatR;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class CalcularFrequenciaPorTurmaCommand : IRequest<bool>
    {
        public IEnumerable<string> Alunos { get; set; }
        public DateTime DataAula { get; set; }
        public string TurmaId { get; set; }
        public string DisciplinaId { get; set; }
        public bool ConsideraTodosAlunos { get; set; }
        public (string rf, string perfil) UsuarioConsiderado { get; set; }

        public CalcularFrequenciaPorTurmaCommand(IEnumerable<string> alunos, DateTime dataAula, string turmaId, string disciplinaId, bool consideraTodosAlunos = false, (string rf, string perfil) usuarioConsiderado = default)
        {
            Alunos = alunos;
            DataAula = dataAula;
            TurmaId = turmaId;
            DisciplinaId = disciplinaId;
            ConsideraTodosAlunos = consideraTodosAlunos;
            UsuarioConsiderado = usuarioConsiderado;
        }
    }
}