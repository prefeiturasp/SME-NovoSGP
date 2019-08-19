import React, { useState } from 'react';
import TesteEditor from '../../../componentes/testeEditor';

export default function Monica() {
  const [olar, setOlar] = useState({ texto: 'olar' });

  function clicaOlar() {
    setOlar({ ...olar, texto: 'ronaldo' });
  }

  const toolbarOptions = [
    ['bold', 'italic', 'underline'],
    [{ list: 'bullet' }, { list: 'ordered' }],
  ];

  const modules = {
    toolbar: toolbarOptions,
  };

  return (
    <div>
      <span>
        <TesteEditor
          className="form-control"
          modules={modules}
          height={135}
          text={olar.texto}
        />
      </span>
      <br />
      <button type="button" onClick={clicaOlar}>
        clica
      </button>
    </div>
  );
}
