import React from 'react';
import type from 'prop-types';

// Estilos
import { TagEstilo } from './styles';

function Tag({ tipo, children, inverted }) {
  return (
    <TagEstilo className={inverted && 'inverted'} tipo={tipo}>
      {children}
    </TagEstilo>
  );
}

Tag.propTypes = {
  tipo: type.oneOf([
    'basico',
    'informativo1',
    'informativo2',
    'erro',
    'alerta',
    'atencao',
    'cancelar',
    'sucesso',
  ]),
  children: type.oneOfType([type.element, type.string]),
  invertido: type.bool,
};

Tag.defaultProps = {
  tipo: 'basico',
  children: () => null,
  invertido: false,
};

export default Tag;
