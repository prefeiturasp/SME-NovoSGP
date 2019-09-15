import 'react-quill/dist/quill.snow.css';
import PropTypes from 'prop-types';
import React, { useState, useEffect } from 'react';
import ReactQuill from 'react-quill';

const TextEditor = React.forwardRef((props, ref) => {
  const { value, onBlur, disabled, onClick, alt, stateAdicional } = props;

  useEffect(() => {

    if (stateAdicional && stateAdicional.focado)
      ref.current.focus();

    if (stateAdicional && stateAdicional.lastRange)
      ref.current.setEditorSelection(ref.current.getEditor(), stateAdicional.lastRange);

    return () => {
      if (onBlur) {
        if (value !== ref.current.state.value) onBlur(ref.current.state.value);
      }
    };
  });

  const onBlurQuill = () => {

    if (onBlur)
      onBlur(ref.current.state.value);
  };

  const onClickQuill = (range) => {

    if (onClick)
      onClick(range);
  };

  return (
    <ReactQuill
      ref={ref}
      modules={modules}
      onBlur={onBlurQuill}
      alt={alt}
      value={value || ''}
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
      tab: false
    }
  }
};
