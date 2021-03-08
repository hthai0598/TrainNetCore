import React, { useEffect } from 'react'
import { Helmet } from 'react-helmet'
import MainLayout from '../../layout/MainLayout'
import PageHeading from '../../organisms/PageHeading'
import breadcrumbs from './breadcrumbs'
import PageFormWrapper from '../../organisms/PageFormWrapper'
import BuyerInvitationDetailForm from './BuyerInvitationDetailForm'
import { message } from 'antd'
import { inject, observer } from 'mobx-react'

const BuyerInvitationDetailPage = ({ usersStore, history }) => {

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
        <title>View buyer detail | Otrafy</title>
      </Helmet>
      <PageHeading
        breadcrumbs={breadcrumbs}
        title={'Update buyer detail'}/>
      <PageFormWrapper
        form={<BuyerInvitationDetailForm/>}
        title={'Update buyer detail'}/>
    </MainLayout>
  )
}

export default inject('usersStore')(observer(BuyerInvitationDetailPage))