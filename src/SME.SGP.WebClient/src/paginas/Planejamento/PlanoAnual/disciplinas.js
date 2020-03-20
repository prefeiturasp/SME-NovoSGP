import React, { useState, useEffect } from 'react';
import Disciplina from './disciplina';

const Disciplinas = ({
  disciplinas,
  preSelecionadas,
  layoutEspecial,
  onChange,
}) => {
  const [listaDisciplinas, setListaDisciplinas] = useState(disciplinas);

  useEffect(() => {
    if (disciplinas) {
      setListaDisciplinas(disciplinas);
    }
  }, [disciplinas, onChange]);

  const selecionarDisciplina = (codigoComponenteCurricular, selecionada) => {
    if (!layoutEspecial) {
      const disciplina = selecionada ? [codigoComponenteCurricular] : [];
      disciplinas.forEach(item => {
        item.selecionada = item.codigoComponenteCurricular === codigoComponenteCurricular
          ? selecionada : false;
      });
      setListaDisciplinas([...disciplinas]);
      onChange(disciplina);
    }
  };

  useEffect(() => {
    onChange(undefined);
  }, [preSelecionadas]);

  return (
    <>
      {listaDisciplinas.map(disciplina => (
        <Disciplina
          disciplina={disciplina}
          onClick={selecionarDisciplina}
          preSelecionada={preSelecionadas.find(
            c => c == disciplina.codigoComponenteCurricular
          )}
          key={disciplina.codigoComponenteCurricular}
          layoutEspecial={layoutEspecial}
        />
      ))}
    </>
  );
};
export default Disciplinas;
