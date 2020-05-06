import React from 'react';
import shortid from 'shortid';
import t from 'prop-types';

// Componentes
import { Colors } from '~/componentes';

// Estilos
import { Botao } from '../styles';

function LabelAulaEvento({ dadosEvento }) {
  return (
    <Botao
      id={shortid.generate()}
      label={dadosEvento.ehAula ? 'Aula' : dadosEvento.tipoEvento}
      color={(dadosEvento.ehAula && Colors.Roxo) || Colors.CinzaBotao}
      className="w-100"
      height={dadosEvento.ehAula ? '38px' : 'auto'}
      border
      steady
    />
  );
}

LabelAulaEvento.propTypes = {
  dadosEvento: t.oneOfType([t.any]),
};

LabelAulaEvento.defaultProps = {
  dadosEvento: {},
};

export default LabelAulaEvento;
