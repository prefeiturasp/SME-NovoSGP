import React from 'react';

const FormGroup = props => {
  const { children, className } = props;

  return <div className={`form-group ${className}`}>{children}</div>;
};

export default FormGroup;
