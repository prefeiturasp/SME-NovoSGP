import React, { useState, useEffect } from 'react';
import Disciplina from './disciplina';

const Disciplinas = ({
  disciplinas,
  preSelecionadas,
  layoutEspecial,
  onChange,
}) => {
  const [listaDisciplinas, setListaDisciplinas] = useState(disciplinas);
  const [disciplinasSelecionadas, setDisciplinasSelecionadas] = useState([]);

  useEffect(() => {
    if (disciplinas) {
      setListaDisciplinas(disciplinas);
    }
  }, [disciplinas, onChange]);

  const selecionarDisciplina = (codigoComponenteCurricular, selecionada) => {
    if (!layoutEspecial) {
      const indiceDisciplina = disciplinasSelecionadas.findIndex(
        c => c === codigoComponenteCurricular
      );
      if (indiceDisciplina >= 0) {
        if (!selecionada) {
          disciplinasSelecionadas.splice(indiceDisciplina, 1);
        }
      } else if (selecionada) {
        disciplinasSelecionadas.push(codigoComponenteCurricular);
      }
      setDisciplinasSelecionadas([...disciplinasSelecionadas]);
      onChange(disciplinasSelecionadas);
    }
  };

  useEffect(() => {
    setDisciplinasSelecionadas(preSelecionadas);
    onChange(preSelecionadas);
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
