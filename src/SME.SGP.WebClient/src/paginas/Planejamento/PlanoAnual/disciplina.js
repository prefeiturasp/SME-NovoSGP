import React, { useState, useEffect } from 'react';
import { Badge } from './bimestre.css';

const Disciplina = ({
  disciplina,
  preSelecionada,
  layoutEspecial,
  onClick,
}) => {
  const [selecionada, setSelecionada] = useState(false);

  const onClickDisciplina = () => {
    if (!layoutEspecial) {
      setSelecionada(!selecionada);
      onClick(disciplina.codigoComponenteCurricular, !disciplina.selecionada);
    }
  };

  useEffect(() => {
    setSelecionada(false);
  }, [disciplina]);

  useEffect(() => {
    setSelecionada(!!preSelecionada);
  }, [preSelecionada]);

  return (
    <Badge
      role="button"
      id={disciplina.codigoComponenteCurricular}
      data-index={disciplina.codigoComponenteCurricular}
      alt={disciplina.nome}
      key={disciplina.codigoComponenteCurricular}
      className={`badge badge-pill border text-dark bg-white font-weight-light px-2 py-1 mr-2 ${disciplina.selecionada &&
        ' selecionada'}`}
      onClick={onClickDisciplina}
    >
      {disciplina.nome}
    </Badge>
  );
};

export default Disciplina;
