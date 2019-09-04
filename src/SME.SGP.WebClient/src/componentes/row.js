import React from 'react';

const Row = (props) => {

    const { children, className } = props;

    return (
        <div {...props} className={`row ${className}`}>
            {children}
        </div>
    );
}

export default Row;
