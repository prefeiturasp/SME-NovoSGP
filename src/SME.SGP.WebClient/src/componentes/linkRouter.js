import React from 'react';
import { Link } from 'react-router-dom';

const LinkRouter = (props) => {

    const { isactive, children, to, className } = props;

    return (
        <>
            {
                isactive ? <Link to={to}>{children}</Link> : <div className={className}>{children}</div>
            }
        </>
    )

};

export default LinkRouter;