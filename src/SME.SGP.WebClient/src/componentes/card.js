import React from 'react';

const Card = props => {
  const { children, className, mtop, mx } = props;

  return (
    <div
      className={`row shadow py-3 px-2 ${mx ? mx : 'mx-2'} ${mtop ||
        ''} bg-white ${className || ''}`}
    >
      {children}
    </div>
  );
};

export default Card;
