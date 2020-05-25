import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import shortid from 'shortid';
import { Base } from '~/componentes';
import { erro } from '~/servicos/alertas';
import ServicoConselhoClasse from '~/servicos/Paginas/ConselhoClasse/ServicoConselhoClasse';
import ComponenteSemNota from './ComponenteSemNota/ComponenteSemNota';

const Sintese = props => {
  const { ehFinal } = props;

  const dadosPrincipaisConselhoClasse = useSelector(
    store => store.conselhoClasse.dadosPrincipaisConselhoClasse
  );

  const {
    conselhoClasseId,
    fechamentoTurmaId,
    alunoCodigo,
  } = dadosPrincipaisConselhoClasse;

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

  const [dados, setDados] = useState([]);

  useEffect(() => {
    ServicoConselhoClasse.obterSintese(
      conselhoClasseId,
      fechamentoTurmaId,
      alunoCodigo
    )
      .then(resp => {
        setDados(resp.data);
      })
      .catch(e => {
        erro(e);
        setDados([]);
      });
  }, [alunoCodigo, conselhoClasseId, fechamentoTurmaId]);

  return (
    <>
      {dados && dados.length
        ? dados.map((componente, i) => {
            return (
              <div className="pl-2 pr-2" key={shortid.generate()}>
                <ComponenteSemNota
                  dados={componente.componenteSinteses}
                  nomeColunaComponente={componente.titulo}
                  corBorda={cores[i]}
                  ehFinal={ehFinal}
                />
              </div>
            );
          })
        : null}
    </>
  );
};

Sintese.propTypes = {
  ehFinal: PropTypes.oneOfType([PropTypes.bool]),
};

Sintese.defaultProps = {
  ehFinal: false,
};

export default Sintese;
