import React from 'react';
import PropTypes from 'prop-types';

const Button = props => {
  const { type, style, className, onClick, icon, label } = props;

  return (
    <button
      type={type}
      className={`btn btn-${style} ${className}`}
      onClick={onClick}
    >
      <i className={`fa fa-${icon}`} />
      {label}
    </button>
  );
};

Button.propTypes = {
  type: PropTypes.string,
  style: PropTypes.string,
  className: PropTypes.string,
  onClick: PropTypes.string,
  icon: PropTypes.string,
  label: PropTypes.string,
};

Button.defaultProps = {
  type: 'button',
  style: 'primary',
};

export default Button;
