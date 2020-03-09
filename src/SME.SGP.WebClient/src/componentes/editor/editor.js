import '@ckeditor/ckeditor5-build-classic/build/translations/pt-br';

import ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import CKEditor from '@ckeditor/ckeditor5-react';
import { Field } from 'formik';
import React from 'react';
import styled from 'styled-components';

import { Base } from '../colors';
import Label from '../label';

const Campo = styled.div`
  .is-invalid {
    .ck.ck-reset.ck-editor.ck-rounded-corners > div.ck.ck-editor__main > div {
      border-color: #dc3545 !important;
    }
    .ck.ck-reset.ck-editor.ck-rounded-corners
      > div.ck.ck-editor__top.ck-reset_all
      > div
      > div.ck.ck-sticky-panel__content
      > div {
      border-color: #dc3545 !important;
    }
  }
`;

const toolbar = [
  'heading',
  'bold',
  'italic',
  'bulletedList',
  'numberedList',
  'blockQuote',
  'redo',
  'undo',
];

export default function Editor(props) {
  const {
    onChange,
    inicial,
    form,
    name,
    label,
    temErro,
    mensagemErro,
    desabilitar,
  } = props;

  const possuiErro = () => {
    return (form && form.errors[name] && form.touched[name]) || temErro;
  };

  const obterErros = () => {
    return (form && form.touched[name] && form.errors[name]) || temErro ? (
      <span style={{ color: `${Base.Vermelho}` }}>
        {(form && form.errors[name]) || mensagemErro}
      </span>
    ) : (
      ''
    );
  };

  const editorComValidacoes = () => {
    return (
      <Campo>
        <div className={form ? (possuiErro() ? 'is-invalid' : '') : ''}>
          <Field
            name={name}
            component={CKEditor}
            editor={ClassicEditor}
            disabled={desabilitar || false}
            config={{
              toolbar,
              table: { isEnabled: true },
              language: 'pt-br',
              removePlugins: [
                'Image',
                'ImageCaption',
                'ImageStyle',
                'ImageToolbar',
                'Indent',
                'IndentToolbar',
                'IndentStyle',
                'Outdent',
              ],
            }}
            data={form.values[name] || ''}
            onChange={(event, editor) => {
              const data = editor.getData();
              onChange && onChange(data);
              form.setFieldValue(name, data || '');
              form.setFieldTouched(name, true, true);
            }}
          />
        </div>
      </Campo>
    );
  };

  const editorSemValidacoes = () => {
    return (
      <CKEditor
        editor={ClassicEditor}
        config={{
          toolbar,
          table: { isEnabled: true },
          readOnly: desabilitar || false,
          language: 'pt-br',
          removePlugins: [
            'Image',
            'ImageCaption',
            'ImageStyle',
            'ImageToolbar',
            'Indent',
            'IndentToolbar',
            'IndentStyle',
            'Outdent',
          ],
        }}
        data={inicial || ''}
        onChange={(event, editor) => {
          const data = editor.getData();
          onChange && onChange(data);
        }}
      />
    );
  };

  return (
    <>
      {label ? <Label text={label} control={name} /> : ''}
      {form ? editorComValidacoes() : editorSemValidacoes()}
      {obterErros()}
    </>
  );
}
