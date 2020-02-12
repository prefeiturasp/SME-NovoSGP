import React, { useState, useEffect } from 'react';
import { MaisMenos } from './fechamentoFinal.css';
import CampoNumero from '~/componentes/campoNumero';
import ServicoNotaConceito from '~/servicos/Paginas/DiarioClasse/ServicoNotaConceito';
import { erros } from '~/servicos/alertas';
import { SelectComponent } from '~/componentes';

export default function ConceitoFinal({
  ehRegencia,
  ehNota,
  aluno,
  onClickConceitoRegencia,
  listaConceitos,
}) {
  const [alunoAtual, setAlunoAtual] = useState(aluno);
  const [regenciaExpandida, setRegenciaExpandida] = useState(false);

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
        alunoAtual.notasConceitoFinal.map(nota =>
          ehNota ? (
            <CampoNumero
              value={nota.notaConceito}
              min={0}
              max={10}
              step={0.5}
              placeholder="Nota"
            />
          ) : (
            <SelectComponent
              id="disciplinasId"
              name="disciplinasId"
              lista={listaConceitos}
              valueOption="id"
              valueText="nome"
              placeholder="Selecione um conceito"
              valueSelect={nota.notaConceito}
              // onChange={conceito => {
              //   debugger;
              //   nota.notaConceito = conceito;
              // }}
            />
          )
        )
      )}
    </>
  );
}
