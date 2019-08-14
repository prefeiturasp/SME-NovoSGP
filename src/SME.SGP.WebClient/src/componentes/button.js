import React from 'react';
import PropTypes from 'prop-types';
import styled from 'styled-components';

const Button = props => {
  const {
    type,
    style,
    color,
    border,
    className,
    onClick,
    disabled,
    icon,
    label,
  } = props;

  const Icon = styled.i``;

  const Btn = styled.button`
    background: ${border ? 'transparent' : color} !important;
    ${border
      ? `border-color: ${color} !important; color: ${color} !important;`
      : `border: 0 none !important;`};
  `;

  return (
    <Btn
      type={type}
      className={`btn btn-${style} ${className}`}
      onClick={onClick}
      disabled={disabled}
    >
      {icon ? <Icon className={`fa fa-${icon} mr-2`} /> : null}
      {label}
    </Btn>
  );
};

Button.propTypes = {
  type: PropTypes.string,
  style: PropTypes.string,
  color: PropTypes.string,
  border: PropTypes.bool,
  className: PropTypes.string,
  onClick: PropTypes.string,
  disabled: PropTypes.bool,
  icon: PropTypes.string,
  label: PropTypes.string,
};

Button.defaultProps = {
  type: 'button',
  style: 'primary',
  border: false,
  className: '',
  disabled: false,
};

export default Button;
