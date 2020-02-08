import React, { useState } from 'react';
import { MaisMenos } from './fechamentoFinal.css';
import CampoNumero from '~/componentes/campoNumero';

export default function ConceitoFinal({
  ehRegencia,
  aluno,
  onClickConceitoRegencia,
}) {
  const [alunoAtual, setAlunoAtual] = useState(aluno);
  const [regenciaExpandida, setRegenciaExpandida] = useState(true);

  const onClickRegencia = () => {
    const expandida = !regenciaExpandida;
    setRegenciaExpandida(expandida);
    onClickConceitoRegencia(expandida);
  };

  return (
    <>
      {ehRegencia ? (
        <MaisMenos
          className={`fas fa-${regenciaExpandida ? 'minus' : 'plus'}-circle`}
          onClick={() => {
            onClickRegencia();
          }}
        />
      ) : (
        alunoAtual.notasConceitoFinal.map(nota => (
          <CampoNumero
            value={nota.notaConceito}
            min={0}
            max={10}
            step={0.5}
            placeholder="Nota"
          />
        ))
      )}
    </>
  );
}
