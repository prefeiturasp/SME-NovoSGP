import React from 'react';
<<<<<<< HEAD
import type from 'prop-types';
=======
import t from 'prop-types';

// Ant
import { Tag as AntTag } from 'antd';
>>>>>>> 1d7bad66a617c18e28055710b061c8afbe92c1ce

// Estilos
import { TagEstilo } from './styles';

<<<<<<< HEAD
function Tag({ tipo, children, inverted }) {
  return (
    <TagEstilo className={inverted && 'inverted'} tipo={tipo}>
      {children}
=======
function Tag({ tipo, tamanho, fluido, centralizado, children, ...rest }) {
  return (
    <TagEstilo
      className={`${tamanho} ${fluido && 'fluido'} ${centralizado &&
        'centralizado'}`}
      tipo={tipo}
    >
      <AntTag {...rest}>{children}</AntTag>
>>>>>>> 1d7bad66a617c18e28055710b061c8afbe92c1ce
    </TagEstilo>
  );
}

Tag.propTypes = {
<<<<<<< HEAD
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
=======
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
>>>>>>> 1d7bad66a617c18e28055710b061c8afbe92c1ce
};

Tag.defaultProps = {
  tipo: 'basico',
<<<<<<< HEAD
  children: () => null,
  invertido: false,
=======
  tamanho: 'pequeno',
  children: null,
  fluido: false,
  centralizado: false,
>>>>>>> 1d7bad66a617c18e28055710b061c8afbe92c1ce
};

export default Tag;
