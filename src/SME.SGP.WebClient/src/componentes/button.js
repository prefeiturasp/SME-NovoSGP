import React from 'react';
import PropTypes from 'prop-types';
import styled from 'styled-components';
import shortid from 'shortid';
import { Base, Active, Hover } from './colors';
import Remover from '../recursos/Remover.svg';

const Button = React.forwardRef((props, ref) => {
  const {
    type,
    style,
    color,
    border,
    steady,
    remove,
    className,
    onClick,
    disabled,
    icon,
    padding,
    height,
    lineHeight,
    width,
    fontSize,
    indice,
    label,
    hidden,
    id,
    customRadius,
    iconType,
  } = props;

  const Icon = styled.i``;

  const Remove = styled(Icon)`
    background: ${Base.Roxo} url(${Remover}) center !important;
    border: 2px solid ${Base.Branco} !important;
    box-sizing: border-box !important;
    height: 15px !important;
    right: -5px !important;
    top: -5px !important;
    width: 15px !important;
  `;

  const Btn = styled.button`
    display: flex;
    position: relative;
    background: ${border ? 'transparent' : Active[color]} !important;
    text-align: center;
    ${
      border
        ? `border-color: ${Active[color]} !important; color: ${Active[color]} !important;`
        : `border: 0 none !important;`
    };
    ${customRadius || ''};
    font-weight: bold !important;
    ${width ? `width: ${width};` : ''}
    ${fontSize && `font-size: ${fontSize} !important;`}
    ${padding && `padding: ${padding} !important;`}
    height: ${height} !important;
    ${lineHeight && `line-height: ${lineHeight}`}
    &:hover {
      background: ${border ? Active[color] : Hover[color]} !important;
      color: ${!steady ? Base.Branco : 'initial'} !important;
    }
    &[disabled] {
      background: transparent !important;
      border: 1px solid ${Base.CinzaDesabilitado} !important;
      color: ${Base.CinzaDesabilitado} !important;
      cursor: unset !important;
    }
  `;

  return (
    <Btn
      hidden={hidden}
      type={type}
      className={`btn btn-${style} ${className} position-relative d-flex justify-content-center align-items-center ${
        padding ? '' : 'py-2 px-3'
      } ${fontSize ? '' : 'fonte-14'}`}
      onClick={onClick}
      disabled={disabled}
      data-indice={indice}
      id={id}
      ref={ref}
    >
      {icon ? (
        <Icon className={`${iconType || 'fa'} fa-${icon} mr-2 py-1`} />
      ) : null}
      {label}
      {remove ? (
        <Remove
          aria-label="Remover"
          className="d-block rounded-circle position-absolute"
        />
      ) : null}
    </Btn>
  );
});

Button.propTypes = {
  type: PropTypes.string,
  style: PropTypes.string,
  color: PropTypes.string,
  border: PropTypes.bool,
  steady: PropTypes.bool,
  remove: PropTypes.bool,
  className: PropTypes.string,
  onClick: PropTypes.func,
  disabled: PropTypes.bool,
  icon: PropTypes.string,
  padding: PropTypes.string,
  height: PropTypes.string,
  lineHeight: PropTypes.string,
  width: PropTypes.string,
  fontSize: PropTypes.string,
  indice: PropTypes.oneOfType([PropTypes.string, PropTypes.number]),
  label: PropTypes.string,
  hidden: PropTypes.bool,
  id: PropTypes.string,
  customRadius: PropTypes.string,
  iconType: PropTypes.string,
};

Button.defaultProps = {
  type: 'button',
  style: 'primary',
  color: Base.Roxo,
  border: false,
  steady: false,
  remove: false,
  className: '',
  onClick: () => {},
  disabled: false,
  icon: '',
  padding: '',
  height: '38px',
  lineHeight: 'inherit',
  width: '',
  fontSize: '',
  indice: '',
  label: '',
  hidden: false,
  id: shortid.generate(),
  customRadius: '',
  iconType: '',
};

export default Button;
