import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { Loader, SelectComponent } from '~/componentes';
import modalidade from '~/dtos/modalidade';
import { AbrangenciaServico, api, erros } from '~/servicos';

const DadosComunicadosLeitura = props => {
  const { codigoDre, codigoUe } = props;

  const [exibirLoader, setExibirLoader] = useState(false);

  const [listaAnosLetivo, setListaAnosLetivo] = useState([]);
  const [anoLetivo, setAnoLetivo] = useState();

  const [carregandoModalidades, setCarregandoModalidades] = useState(false);
  const [listaModalidades, setListaModalidades] = useState([]);
  const [modalidadeId, setModalidadeId] = useState();

  const [carregandoSemestres, setCarregandoSemestres] = useState(false);
  const [listaSemestres, setListaSemestres] = useState([]);
  const [semestre, setSemestre] = useState();

  const [listaGrupo] = useState([
    { valor: 1, desc: 'Grupo 01' },
    { valor: 2, desc: 'Grupo 02' },
  ]);
  const [grupo, setGrupo] = useState();

  const obterAnosLetivos = useCallback(async () => {
    setExibirLoader(true);
    const anosLetivo = await AbrangenciaServico.buscarTodosAnosLetivos()
      .catch(e => erros(e))
      .finally(() => setExibirLoader(false));

    if (anosLetivo?.data?.length) {
      const a = [];
      anosLetivo.data.forEach(ano => {
        a.push({ desc: ano, valor: ano });
      });
      setAnoLetivo(a[0].valor);
      setListaAnosLetivo(a);
    } else {
      setListaAnosLetivo([]);
    }
  }, []);

  useEffect(() => {
    obterAnosLetivos();
  }, [obterAnosLetivos]);

  // TODO Validar se vai manter este endpoint!
  const obterModalidades = async (ue, ano) => {
    if (ue && ano) {
      setCarregandoModalidades(true);
      const { data } = await api.get(`/v1/ues/${ue}/modalidades?ano=${ano}`);
      if (data) {
        const lista = data.map(item => ({
          desc: item.nome,
          valor: String(item.id),
        }));

        if (lista && lista.length && lista.length === 1) {
          setModalidadeId(lista[0].valor);
        }
        setListaModalidades(lista);
      }
      setCarregandoModalidades(false);
    }
  };

  useEffect(() => {
    if (anoLetivo && codigoUe) {
      obterModalidades(codigoUe, anoLetivo);
    } else {
      setModalidadeId();
      setListaModalidades([]);
    }
  }, [anoLetivo, codigoUe]);

  const obterSemestres = async (
    modalidadeSelecionada,
    anoLetivoSelecionado
  ) => {
    setCarregandoSemestres(true);
    const retorno = await api.get(
      `v1/abrangencias/false/semestres?anoLetivo=${anoLetivoSelecionado}&modalidade=${modalidadeSelecionada ||
        0}`
    );
    if (retorno && retorno.data) {
      const lista = retorno.data.map(periodo => {
        return { desc: periodo, valor: periodo };
      });

      if (lista && lista.length && lista.length === 1) {
        setSemestre(lista[0].valor);
      }
      setListaSemestres(lista);
    }
    setCarregandoSemestres(false);
  };

  useEffect(() => {
    if (
      modalidadeId &&
      anoLetivo &&
      String(modalidadeId) === String(modalidade.EJA)
    ) {
      obterSemestres(modalidadeId, anoLetivo);
    } else {
      setSemestre();
      setListaSemestres([]);
    }
  }, [modalidadeId, anoLetivo]);

  return (
    <Loader loading={exibirLoader}>
      <div className="row">
        <div className="col-sm-12 col-md-6 col-lg-3 col-xl-2 mb-2">
          <SelectComponent
            id="select-ano-letivo"
            label="Ano Letivo"
            lista={listaAnosLetivo}
            valueOption="valor"
            valueText="desc"
            disabled={listaAnosLetivo?.length === 1}
            onChange={setAnoLetivo}
            valueSelect={anoLetivo}
            placeholder="Selecione o ano"
          />
        </div>
        <div className="col-sm-12 col-md-6 col-lg-3 col-xl-2 mb-2">
          <SelectComponent
            id="select-grupo"
            label="Grupo"
            lista={listaGrupo}
            valueOption="valor"
            valueText="desc"
            onChange={setGrupo}
            valueSelect={grupo}
            placeholder="Selecione o grupo"
          />
        </div>
        <div className="col-sm-12 col-md-6 col-lg-3 col-xl-3  mb-2">
          <Loader loading={carregandoModalidades} tip="">
            <SelectComponent
              id="select-modalidade"
              label="Modalidade"
              lista={listaModalidades}
              valueOption="valor"
              valueText="desc"
              onChange={setModalidadeId}
              valueSelect={modalidadeId}
              placeholder="Modalidade"
            />
          </Loader>
        </div>
        <div className="col-sm-12 col-md-6 col-lg-3 col-xl-3 mb-2">
          <Loader loading={carregandoSemestres} tip="">
            <SelectComponent
              id="select-semestre"
              lista={listaSemestres}
              valueOption="valor"
              valueText="desc"
              label="Semestre"
              disabled={
                !modalidadeId ||
                (listaSemestres && listaSemestres.length === 1) ||
                String(modalidadeId) !== String(modalidade.EJA)
              }
              valueSelect={semestre}
              onChange={setSemestre}
              placeholder="Semestre"
            />
          </Loader>
        </div>
      </div>
    </Loader>
  );
};

DadosComunicadosLeitura.propTypes = {
  codigoDre: PropTypes.oneOfType([PropTypes.string]),
  codigoUe: PropTypes.oneOfType([PropTypes.string]),
};

DadosComunicadosLeitura.defaultProps = {
  codigoDre: '',
  codigoUe: '',
};

export default DadosComunicadosLeitura;
