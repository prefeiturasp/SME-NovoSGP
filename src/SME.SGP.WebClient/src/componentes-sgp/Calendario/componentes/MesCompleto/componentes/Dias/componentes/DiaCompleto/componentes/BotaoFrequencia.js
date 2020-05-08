import React from 'react';
import shortid from 'shortid';
import t from 'prop-types';

// Componentes
import { Colors } from '~/componentes';

// Estilos
import { Botao } from '../styles';

function BotaoFrequencia({ onClickFrequencia }) {
  return (
    <div className="pr-0 d-flex align-items-center px-2 p-x-md-3">
      <Botao
        id={shortid.generate()}
        label="Frequência"
        color={Colors.Roxo}
        className="w-100 position-relative btn-sm"
        onClick={onClickFrequencia}
        height="24px"
        padding="0 1rem"
        border
      />
    </div>
  );
}

BotaoFrequencia.propTypes = {
  onClickFrequencia: t.func.isRequired,
};

export default BotaoFrequencia;
