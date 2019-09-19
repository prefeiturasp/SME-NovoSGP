import 'react-quill/dist/quill.snow.css';
import PropTypes from 'prop-types';
import React, { useState, useEffect } from 'react';
import ReactQuill from 'react-quill';

const TextEditor = React.forwardRef((props, ref) => {
  const { value, onBlur, disabled, onClick, alt, estadoAdicional } = props;

  useEffect(() => {

    if (estadoAdicional && estadoAdicional.focado) ref.current.focus();

    if (estadoAdicional && estadoAdicional.ultimoFoco)
      ref.current.setEditorSelection(
        ref.current.getEditor(),
        estadoAdicional.ultimoFoco
      );

    return () => {
      if (onBlur) {
        if (value !== ref.current.state.value) onBlur(ref.current.state.value);
      }
    };
  });

  const onBlurQuill = (rangeAnterior, origem, editor) => {

    if (onBlur && origem === "user") onBlur(ref.current.state.value);

  };

  const onClickQuill = range => {
    if (onClick) onClick(range);
  };

  return (
    <ReactQuill
      ref={ref}
      modules={modules}
      onBlur={onBlurQuill}
      alt={alt}
      defaultValue={value && value}
      onFocus={onClickQuill}
      readOnly={disabled}
      disabled={disabled}
    />
  );
});

TextEditor.propTypes = {
  onBlur: PropTypes.func,
  height: PropTypes.string,
  value: PropTypes.string,
};

TextEditor.defaultProps = {
  onBlur: () => { },
  value: '',
};

export default TextEditor;

const toolbarOptions = [
  ['bold', 'italic', 'underline'],
  [{ list: 'bullet' }, { list: 'ordered' }],
];

const modules = {
  toolbar: toolbarOptions,
  keyboard: {
    bindings: {
      tab: false,
    },
  },
};
