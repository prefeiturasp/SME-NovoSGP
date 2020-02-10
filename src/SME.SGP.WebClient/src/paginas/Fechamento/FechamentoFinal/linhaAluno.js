import React, { useState } from 'react';
import ConceitoFinal from './conceitoFinal';
import NotaRegencia from './notaRegencia';

export default function LinhaAluno({
  aluno,
  ehRegencia,
  ehNota,
  listaConceitos,
  disciplinaSelecionada,
}) {
  const [regenciaExpandida, setRegenciaExpandida] = useState(true);

  return (
    <>
      <tr>
        <td className="sticky-col col-numero-chamada">{aluno.numeroChamada}</td>
        <td className="sticky-col col-nome-aluno">{aluno.nome}</td>
        <td className="sticky-col col-nota-conceito">
          {aluno.notasConceitoBimestre
            .filter(n => n.disciplinaCodigo == disciplinaSelecionada)
            .map(c => (
              <div className="input-notas">{c.notaConceito}</div>
            ))}
        </td>
        <td className="sticky-col">{aluno.totalFaltas}</td>
        <td className="sticky-col">{aluno.totalAusenciasCompensadas}</td>
        <td className="sticky-col">{aluno.frequencia}%</td>
        <td className="sticky-col col-conceito-final">
          <ConceitoFinal
            aluno={aluno}
            ehRegencia={ehRegencia}
            ehNota={ehNota}
            onClickConceitoRegencia={expandida => {
              setRegenciaExpandida(expandida);
            }}
          />
        </td>
      </tr>
      {ehRegencia && regenciaExpandida && (
        <tr className="linha-conceito-regencia">
          <td colSpan="7" className="elevation-3">
            <NotaRegencia aluno={aluno} />
          </td>
        </tr>
      )}
    </>
  );
}
