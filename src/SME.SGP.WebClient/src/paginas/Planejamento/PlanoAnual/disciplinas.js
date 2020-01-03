import React, { useState, useEffect } from 'react';
import Disciplina from './disciplina';

const Disciplinas = ({ disciplinas, onChange }) => {
  const [lista, setLista] = useState(disciplinas);
  const [disciplinasSelecionadas, setDisciplinasSelecionadas] = useState([]);

  useEffect(() => {
    if (disciplinas) {
      setLista(disciplinas);
      setDisciplinasSelecionadas([]);
    }
  }, [disciplinas, onChange]);

  const selecionarDisciplina = (codigoComponenteCurricular, selecionada) => {
    const listaDisciplinas = disciplinasSelecionadas;
    const indiceDisciplina = listaDisciplinas.findIndex(
      c => c === codigoComponenteCurricular
    );
    if (indiceDisciplina >= 0) {
      if (!selecionada) {
        listaDisciplinas.splice(indiceDisciplina, 1);
      }
    } else if (selecionada) {
      listaDisciplinas.push(codigoComponenteCurricular);
    }
    setDisciplinasSelecionadas([...listaDisciplinas]);
    onChange(listaDisciplinas);
  };

  return (
    <>
      {lista.map(disciplina => (
        <Disciplina
          disciplina={disciplina}
          onClick={selecionarDisciplina}
          key={disciplina.codigoComponenteCurricular}
        />
      ))}
    </>
  );
};
export default Disciplinas;
