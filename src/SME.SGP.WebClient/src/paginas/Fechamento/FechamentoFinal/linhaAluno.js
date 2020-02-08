import React, { useState } from 'react';
import ConceitoFinal from './conceitoFinal';
import CampoNumero from '~/componentes/campoNumero';
import { InputNumber } from 'antd';
import { CampoNumerico } from './fechamentoFinal.css';
import { Label } from '~/componentes';
import NotaRegencia from './notaRegencia';

export default function LinhaAluno({
  aluno,
  ehRegencia,
  disciplinaSelecionada,
}) {
  const [regenciaExpandida, setRegenciaExpandida] = useState(true);

  return (
    <>
      <tr>
        <td
          className="sticky-col col-numero-chamada"
          rowSpan={ehRegencia && regenciaExpandida ? '2' : '1'}
        >
          {aluno.numeroChamada}
        </td>
        <td
          className="sticky-col col-nome-aluno"
          rowSpan={ehRegencia && regenciaExpandida ? '2' : '1'}
        >
          {aluno.nome}
        </td>
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
            onClickConceitoRegencia={expandida => {
              setRegenciaExpandida(expandida);
            }}
          />
        </td>
      </tr>
      {ehRegencia && regenciaExpandida && (
        <tr className="linha-conceito-regencia">
          <td colSpan="5" className="elevation-3">
            <NotaRegencia aluno={aluno} />
            {/* <table className="tb-conceito-regencia">
              <thead>
                {aluno.notasConceitoFinal.map(nota => (
                  <th>
                    <Label text={nota.nomeDisciplina} />
                  </th>
                ))}
              </thead>
              <tbody>
                {aluno.notasConceitoFinal.map(nota => (
                  <td>
                    <InputNumber
                      classNameCampo="col-conceito-regencia"
                      min={1}
                      max={10}
                      defaultValue={nota.notaConceito}
                      onChange={value => {
                        nota.notaConceito = value;
                      }}
                    />
                  </td>
                ))}
              </tbody>
            </table> */}
            {/* {aluno.notasConceitoFinal.map(nota => (
              <div className="input-reg">
                <Label text={nota.nomeDisciplina} />
                <InputNumber
                  label={nota.nomeDisciplina}
                  classNameCampo="col-conceito-regencia"
                  min={1}
                  max={10}
                  defaultValue={nota.notaConceito}
                  onChange={value => {
                    nota.notaConceito = value;
                  }}
                />
              </div>
            ))} */}
            {/* <CampoNumerico></CampoNumerico> */}
          </td>
        </tr>
      )}
    </>
  );
}
