import React, { useEffect } from 'react';

import { useDispatch } from 'react-redux';

import api from '~/servicos/api';

import { salvarVersao } from '~/redux/modulos/sistema/actions';

export default function VersaoSistema() {
  const dispatch = useDispatch();
  useEffect(() => {
    async function buscarVersao() {
      try {
        const { data, status } = await api.get('v1/versoes');
        if (data && status === 200) {
          dispatch(salvarVersao(data));
        }
      } catch (error) {
        dispatch(salvarVersao(1));
      }
    }

    buscarVersao();
  }, [dispatch]);
  return null;
}
