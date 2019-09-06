import React from 'react';
import PropTypes from 'prop-types';
import styled from 'styled-components';
import { Base, Active, Hover } from './colors';
import Remover from '../recursos/Remover.svg';

const Button = props => {
  const {
    type,
    style,
    color,
    border,
    bold,
    steady,
    remove,
    className,
    onClick,
    disabled,
    icon,
    label,
    hidden,
    id,
  } = props;

  const Icon = styled.i``;

  const Remove = styled(Icon)`
    background: ${Base.Roxo} url(${Remover}) center;
    border: 2px solid ${Base.Branco};
    box-sizing: border-box;
    height: 15px;
    right: -5px;
    top: -5px;
    width: 15px;
  `;

  const Btn = styled.button`
    background: ${border ? 'transparent' : Active[color]} !important;
    ${border
      ? `border-color: ${Active[color]} !important; color: ${Active[color]} !important;`
      : `border: 0 none !important;`};
    display: flex;
    font-weight: ${bold ? 'bold' : 'normal'} !important;
    height: 38px;
    &:hover {
      background: ${Hover[color]} !important;
      color: ${!steady ? Base.Branco : 'initial'} !important;
    }
    &[disabled] {
      background: transparent !important;
      border-color: ${Base.CinzaDesabilitado} !important;
      color: ${Base.CinzaDesabilitado} !important;
    }
  `;

  return (
    <Btn
      hidden={hidden}
      type={type}
      className={`btn btn-${style} ${className} position-relative py-2 px-3 fonte-14`}
      onClick={onClick}
      disabled={disabled}
      id={id}
    >
      {icon ? <Icon className={`fa fa-${icon} mr-2`} /> : null}
      {label}
      {remove ? (
        <Remove
          aria-label="Remover"
          className="d-block rounded-circle position-absolute"
        />
      ) : null}
    </Btn>
  );
};

Button.propTypes = {
  type: PropTypes.string,
  style: PropTypes.string,
  color: PropTypes.string,
  border: PropTypes.bool,
  bold: PropTypes.bool,
  steady: PropTypes.bool,
  remove: PropTypes.bool,
  className: PropTypes.string,
  onClick: PropTypes.func,
  disabled: PropTypes.bool,
  icon: PropTypes.string,
  label: PropTypes.string,
  hidden: PropTypes.bool,
};

Button.defaultProps = {
  type: 'button',
  style: 'primary',
  color: Base.Roxo,
  border: false,
  bold: false,
  steady: false,
  remove: false,
  className: '',
  onClick: () => {},
  disabled: false,
  icon: '',
  label: '',
  hidden: false,
};

export default Button;
