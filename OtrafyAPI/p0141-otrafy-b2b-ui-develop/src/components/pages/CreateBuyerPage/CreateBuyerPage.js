import React, { useEffect } from 'react'
import { Helmet } from 'react-helmet'
import MainLayout from '../../layout/MainLayout'
import PageHeading from '../../organisms/PageHeading/PageHeading'
import PageFormWrapper from '../../organisms/PageFormWrapper'
import breadcrumbs from './breadcrumbs'
import CreateBuyerForm from './CreateBuyerForm'
import { inject, observer } from 'mobx-react'

const CreateBuyerPage = ({ usersStore, history }) => {

  useEffect(() => {
    const role = usersStore.currentUser.role
    if (role) {
      switch (role) {
        case 'administrator':
          break
        default:
          message.error(`You dont have permission to view this page, please contact admin for more information`)
          usersStore.userLogout()
            .then(() => history.push('/'))
      }
    }
  }, [usersStore.currentUser])

  return (
    <MainLayout>
      <Helmet>
        <title>Create new buyer | Otrafy</title>
      </Helmet>
      <PageHeading
        breadcrumbs={breadcrumbs}
        title={'Create new buyer'}/>
      <PageFormWrapper
        form={<CreateBuyerForm/>}
        title={'Create new buyer'}/>
    </MainLayout>
  )
}

export default inject('usersStore')(observer(CreateBuyerPage))