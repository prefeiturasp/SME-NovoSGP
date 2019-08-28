import React from 'react';

const Card = (props) => {
    
    const {children} = props;

  return (
    <div className="row shadow py-3 px-2 mx-2 my-4 bg-white">
        {children}
    </div>
  );

}

export default Card;