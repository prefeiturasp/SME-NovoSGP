import React, { forwardRef, useState } from 'react';

const InputCustom = forwardRef((props, ref) => {
    const [inputValue, setInputValue] = useState(props.value);

    const onChange = (event) => {
        setInputValue(event.target.value);
    };

    return (
        <input type="text" {...props} ref={ref} value={inputValue} onChange={onChange} />
    );
});

export default InputCustom;