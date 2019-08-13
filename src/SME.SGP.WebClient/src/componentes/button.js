import React from 'react';
import PropTypes from 'prop-types';
import styled from 'styled-components';

const Button = props => {
  const {
    type,
    style,
    color,
    className,
    onClick,
    disabled,
    icon,
    label,
  } = props;

  const Icone = styled.i``;

  return (
    <button
      type={type}
      className={`btn btn-${style}-${color} ${className}`}
      onClick={onClick}
      disabled={disabled}
    >
      {icon ? <Icone className={`fa fa-${icon} mr-2`} /> : null}
      {label}
    </button>
  );
};

Button.propTypes = {
  type: PropTypes.string,
  style: PropTypes.string,
  color: PropTypes.string,
  className: PropTypes.string,
  onClick: PropTypes.string,
  disabled: PropTypes.bool,
  icon: PropTypes.string,
  label: PropTypes.string,
};

Button.defaultProps = {
  type: 'button',
  style: 'outline',
  color: 'primary',
  disabled: false,
};

export default Button;
