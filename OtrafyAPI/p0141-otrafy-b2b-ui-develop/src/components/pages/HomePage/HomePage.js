import React, { useEffect } from 'react'
import { Helmet } from 'react-helmet'
import MainLayout from '../../layout/MainLayout'
import { inject, observer } from 'mobx-react'

const HomePage = ({ usersStore, history }) => {

  useEffect(() => {
    switch (usersStore.currentUser.role) {
      case 'administrator':
        history.push('/company-management')
        break
      case 'suppliers':
        history.push('/request-management')
        break
      case 'buyers':
        history.push('/dashboard')
        break
    }
  }, [usersStore.currentUser])

  return (
    <MainLayout>
      <Helmet>
        <title>Home | Otrafy</title>
      </Helmet>
    </MainLayout>
  )
}

export default inject('usersStore')(observer(HomePage))
