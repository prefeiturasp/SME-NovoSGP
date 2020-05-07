import React from 'react';
import shortid from 'shortid';

// Estilos
import { DiasDaSemanaWrapper } from './styles';

const Dias = [
  'Domingo',
  'Segunda',
  'Terça',
  'Quarta',
  'Quinta',
  'Sexta',
  'Sábado',
];

function DiasDaSemana() {
  return (
    <DiasDaSemanaWrapper>
      {Dias.map(dia => (
        <div key={shortid.generate()} className="col">
          <div className="text-muted text-center fonte-12">{dia}</div>
        </div>
      ))}
    </DiasDaSemanaWrapper>
  );
}

export default DiasDaSemana;
