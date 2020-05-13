import React, { useState, useEffect } from 'react';
import Disciplina from './disciplina';
import { Loader } from '~/componentes';

const Disciplinas = ({
  disciplinas,
  preSelecionadas,
  layoutEspecial,
  onChange,
}) => {
  const [listaDisciplinas, setListaDisciplinas] = useState(disciplinas);
  const [carregando, setCarregando] = useState(true);

  useEffect(() => {
    if (disciplinas) {
      setListaDisciplinas(disciplinas);
      setCarregando(false);
    }
  }, [disciplinas, onChange]);

  const selecionarDisciplina = (codigoComponenteCurricular, selecionada) => {
    if (!layoutEspecial) {
      const disciplina = selecionada ? [codigoComponenteCurricular] : [];
      disciplinas.forEach(item => {
        item.selecionada =
          item.codigoComponenteCurricular === codigoComponenteCurricular
            ? selecionada
            : false;
      });
      setListaDisciplinas([...disciplinas]);
      onChange(disciplina);
    }
  };

  useEffect(() => {
    onChange(undefined);
  }, [preSelecionadas]);

  return (
    <Loader loading={carregando}>
      {listaDisciplinas.map(disciplina => (
        <Disciplina
          disciplina={disciplina}
          onClick={selecionarDisciplina}
          preSelecionada={preSelecionadas.find(
            c => String(c) === String(disciplina.codigoComponenteCurricular)
          )}
          key={disciplina.codigoComponenteCurricular}
          layoutEspecial={layoutEspecial}
        />
      ))}
    </Loader>
  );
};

export default Disciplinas;
