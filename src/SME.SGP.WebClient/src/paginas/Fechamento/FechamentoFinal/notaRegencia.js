import React from 'react';
import { InputNumber } from 'antd';
import { Label } from '~/componentes';
import { CampoNumerico } from './fechamentoFinal.css';

export default function NotaRegencia({ aluno }) {
  return (
    <div className="col-teste">
      {aluno.notasConceitoFinal.map(nota => (
        <div className="input-teste">
          <Label text={nota.nomeDisciplina} />
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
      ))}
    </div>
  );
}
