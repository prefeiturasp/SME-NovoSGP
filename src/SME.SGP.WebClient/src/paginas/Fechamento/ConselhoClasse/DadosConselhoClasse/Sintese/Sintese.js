import React, { useState, useEffect } from 'react';
import { MockSintese } from './mock-sintese';
import { Base } from '~/componentes';
import ComponenteSemNota from './ComponenteSemNota/ComponenteSemNota';
import ServicoConselhoClasse from '~/servicos/Paginas/ConselhoClasse/ServicoConselhoClasse';
import { setSintese } from '~/redux/modulos/conselhoClasse/actions';
import { useDispatch } from 'react-redux';

const Sintese = props => {
  const { ehFinal, bimestreSelecionado } = props;
  const cores = [
    Base.Azul,
    Base.RoxoEventoCalendario,
    Base.Laranja,
    Base.RosaCalendario,
    Base.RoxoClaro,
    Base.VerdeBorda,
    Base.Bordo,
    Base.CinzaBadge,
    Base.Preto,
    Base.VerdeBorda,
  ];

  const [dados, setDados] = useState();

  useEffect(() => {
    ServicoConselhoClasse.obterSintese().then(resp => {
      setDados(resp);
    });
  }, [bimestreSelecionado]);

  return (
    <>
      {dados
        ? dados.map((componente, i) => {
            return (
              <ComponenteSemNota
                dados={componente.componentes}
                nomeColunaComponente={componente.titulo}
                corBorda={cores[i]}
                ehFinal={ehFinal}
              />
            );
          })
        : null}
    </>
  );
};

export default Sintese;
