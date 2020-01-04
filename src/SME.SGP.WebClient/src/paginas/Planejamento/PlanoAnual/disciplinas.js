import React, { useState, useEffect } from 'react';
import Disciplina from './disciplina';

const Disciplinas = ({ disciplinas, preSelecionadas, onChange }) => {
  const [listaDisciplinas, setListaDisciplinas] = useState(disciplinas);
  const [disciplinasSelecionadas, setDisciplinasSelecionadas] = useState([]);

  useEffect(() => {
    if (disciplinas) {
      setListaDisciplinas(disciplinas);
      setDisciplinasSelecionadas([]);
    }
  }, [disciplinas, onChange]);

  const selecionarDisciplina = (codigoComponenteCurricular, selecionada) => {
    const listaDisciplinasSelecionadas = disciplinasSelecionadas;
    const indiceDisciplina = listaDisciplinasSelecionadas.findIndex(
      c => c === codigoComponenteCurricular
    );
    if (indiceDisciplina >= 0) {
      if (!selecionada) {
        listaDisciplinasSelecionadas.splice(indiceDisciplina, 1);
      }
    } else if (selecionada) {
      listaDisciplinasSelecionadas.push(codigoComponenteCurricular);
    }
    setDisciplinasSelecionadas([...listaDisciplinasSelecionadas]);
    onChange(listaDisciplinasSelecionadas);
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
        />
      ))}
    </>
  );
};
export default Disciplinas;
