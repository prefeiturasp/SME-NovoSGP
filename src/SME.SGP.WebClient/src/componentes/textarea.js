import React from 'react';

const Textarea = ({ rows, value, name, id, onChange }) => {
  const emiteValor = event => {
    onChange(event.target.value);
  };

  return (
    <textarea
      className="form-control"
      rows={rows || 5}
      onChange={emiteValor}
      name={name}
      id={id || name}
      value={value}
    ></textarea>
  );
};
export default Textarea;
