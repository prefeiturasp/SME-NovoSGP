import React from 'react';
import t from 'prop-types';

// Ant
import { Tag as AntTag } from 'antd';

// Estilos
import { TagEstilo } from './styles';

function Tag({ tipo, tamanho, fluido, centralizado, children, ...rest }) {
  return (
    <TagEstilo
      className={`${tamanho} ${fluido && 'fluido'} ${centralizado &&
        'centralizado'}`}
      tipo={tipo}
    >
      <AntTag {...rest}>{children}</AntTag>
    </TagEstilo>
  );
}

Tag.propTypes = {
  tipo: t.oneOf([
    'basico',
    'informativo1',
    'informativo2',
    'cancelar',
    'erro',
    'alerta',
    'sucesso',
    'atencao',
  ]),
  tamanho: t.oneOf(['pequeno', 'medio', 'grande']),
  children: t.oneOfType([t.string, t.element]),
  fluido: t.bool,
  centralizado: t.bool,
};

Tag.defaultProps = {
  tipo: 'basico',
  tamanho: 'pequeno',
  children: null,
  fluido: false,
  centralizado: false,
};

export default Tag;
