import 'react-quill/dist/quill.snow.css';
import PropTypes from 'prop-types';
import React, { useState, useEffect } from 'react';
import ReactQuill from 'react-quill';

const TextEditor = React.forwardRef((props, ref) => {
  const { value, onBlur, disabled, onClick, alt } = props;

  useEffect(() => {
    return () => {
      if (onBlur) {
        if (value !== ref.current.state.value) onBlur(ref.current.state.value);
      }
    };
  });

  const onBlurQuill = () => {
    if (onBlur) onBlur(ref.current.state.value);
  };

  const onClickQuill = () => {
    if (onClick) onClick();
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
  onBlur: () => {},
  value: '',
};

export default TextEditor;

const toolbarOptions = [
  ['bold', 'italic', 'underline'],
  [{ list: 'bullet' }, { list: 'ordered' }],
];

const modules = {
  toolbar: toolbarOptions,
};
