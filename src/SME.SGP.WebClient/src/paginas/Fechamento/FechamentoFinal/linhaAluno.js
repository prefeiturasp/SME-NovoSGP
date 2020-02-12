import React, { useState } from 'react';
import { Tooltip } from 'antd';
import ConceitoFinal from './conceitoFinal';
import NotaRegencia from './notaRegencia';
import { Info } from './fechamentoFinal.css';

export default function LinhaAluno({
  aluno,
  ehRegencia,
  ehNota,
  listaConceitos,
  disciplinaSelecionada,
  onChange,
}) {
  const [regenciaExpandida, setRegenciaExpandida] = useState(false);

  return (
    <>
      <tr>
        <td className="sticky-col col-numero-chamada">
          {aluno.numeroChamada}
          {aluno.temInformacao ? (
            <Tooltip title={aluno.informacao} placement="top">
              <Info className="fas fa-circle" />
            </Tooltip>
          ) : (
            ''
          )}
        </td>
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
            onChange={onChange}
          />
        </td>
      </tr>
      {ehRegencia && regenciaExpandida && (
        <tr className="linha-conceito-regencia">
          <td colSpan="7">
            <NotaRegencia
              aluno={aluno}
              ehNota={ehNota}
              listaConceitos={listaConceitos}
              onChange={onChange}
            />
          </td>
        </tr>
      )}
    </>
  );
}
