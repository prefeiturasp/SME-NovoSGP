import React from 'react';

const Card = props => {
  const { className, children } = props;
  return (
    <div className={`ant-card ant-card-bordered ${className}`}>{children}</div>
  );
};

export default Card;
