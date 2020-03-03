import React, { useState, useEffect } from 'react';
import { useSelector } from 'react-redux';
import { Badge } from './bimestre.css';
import modalidade from '~/dtos/modalidade';

const Disciplina = ({
  disciplina,
  preSelecionada,
  layoutEspecial,
  onClick,
}) => {
  const [selecionada, setSelecionada] = useState(false);
  /** Verirfica a turma selecionada para não habilitar quando for turma de ensino médio
   *  OBS: Essa validação será retirada quando forem adicionados os objetivos de aprendizagem
   *  para Ensino Médio
   */
  const turmaSelecionada = useSelector(c => c.usuario.turmaSelecionada);

  const onClickDisciplina = () => {
    if (!layoutEspecial) {
      setSelecionada(!selecionada);
      onClick(disciplina.codigoComponenteCurricular, !selecionada);
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
      className={`badge badge-pill border text-dark bg-white font-weight-light px-2 py-1 mr-2 ${selecionada &&
        ' selecionada'}`}
      onClick={onClickDisciplina}
      disabled={turmaSelecionada.modalidade === modalidade.ENSINO_MEDIO}
    >
      {disciplina.nome}
    </Badge>
  );
};

export default Disciplina;
