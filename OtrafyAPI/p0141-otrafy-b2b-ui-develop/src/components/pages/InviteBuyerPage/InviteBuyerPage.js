import React, { useEffect } from 'react'
import { Helmet } from 'react-helmet'
import AuthLayout from '../../layout/AuthLayout'
import { inject, observer } from 'mobx-react'
import CreateBuyerFromInvitationForm from './CreateBuyerFromInvitationForm'

const InviteBuyerPage = ({ buyersStore, history, match }) => {

  const inviteTokenId = match.params.invitedId

  useEffect(() => {
    buyersStore.checkValidBuyerInviteToken(inviteTokenId, history)
  }, [])

  return (
    <AuthLayout>
      <Helmet>
        <title>Create Buyer password | Otrafy</title>
      </Helmet>
      <CreateBuyerFromInvitationForm/>
    </AuthLayout>
  )
}

export default inject('buyersStore')(observer(InviteBuyerPage))