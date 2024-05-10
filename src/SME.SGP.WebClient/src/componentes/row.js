import React from 'react';
import shortid from 'shortid';

const Row = props => {
  const { children, className } = props;

  return (
    <div
      {...props}
      key={shortid.generate()}
      className={`row ${className || ''}`}
    >
      {children}
    </div>
  );
};

export default Row;
