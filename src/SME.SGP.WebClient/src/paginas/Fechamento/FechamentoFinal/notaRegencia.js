import React, { useState, useEffect } from 'react';
import { InputNumber } from 'antd';
import { Label, SelectComponent } from '~/componentes';
import { CampoNumerico } from './fechamentoFinal.css';
import ServicoNotaConceito from '~/servicos/Paginas/DiarioClasse/ServicoNotaConceito';

export default function NotaRegencia({ aluno, ehNota, listaConceitos }) {
  return (
    <div className="col-teste">
      {aluno.notasConceitoFinal.map(nota =>
        ehNota ? (
          <div className="input-regencia">
            <Label text={nota.disciplina} className="break-word" />
            <InputNumber
              classNameCampo="col-conceito-regencia"
              min={1}
              max={10}
              defaultValue={nota.notaConceito}
              onChange={value => {
                nota.notaConceito = value;
              }}
            />
          </div>
        ) : (
          <SelectComponent
            id="disciplinasId"
            name="disciplinasId"
            label="Componente curricular"
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
      )}
    </div>
  );
}
