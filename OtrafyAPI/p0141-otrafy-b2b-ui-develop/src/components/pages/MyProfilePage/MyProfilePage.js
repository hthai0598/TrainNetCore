import React, { useEffect, useState } from 'react'
import MainLayout from '../../layout/MainLayout'
import PageHeading from '../../organisms/PageHeading/PageHeading'
import breadcrumbs from './breadcrumbs'
import { inject, observer } from 'mobx-react'
import ProfileCard from '../../organisms/ProfileCard/ProfileCard'
import ProfileEditCard from '../../organisms/ProfileCard/ProfileEditCard'
import { Wrapper } from './CustomStyled'
import UserInfoForm from './UserInfoForm'
import usersStore from '../../../stores/usersStore'

const MyProfilePage = ({ usersStore, history }) => {

  const handleLogout = () => {
    usersStore.userLogout()
      .then(() => {
        history.push('/login')
      })
  }

  const handleUpdateInfo = values => {
    const submitValue = {
      firstName: values.firstName,
      lastName: values.lastName,
      email: values.email,
      phone: values.phone || null,
      jobTitle: values.jobTitle,
      companyName: values.companyName || null,
      address: values.address || null,
      description: values.description || null,
    }
    usersStore.updateUserInfo(submitValue)
  }

  const toggleEditMode = state => {
    usersStore.toggleEditMode(state)
  }

  useEffect(() => {
    usersStore.toggleEditMode(false)
  }, [])

  return (
    <MainLayout>
      <PageHeading
        breadcrumbs={breadcrumbs}
        title={'My profile'}>
      </PageHeading>
      <Wrapper>
        {
          usersStore.editMode
            ? <ProfileEditCard
              form={<UserInfoForm
                onSubmit={values => handleUpdateInfo(values)}
                info={usersStore.currentUser}/>}
              onToggleEdit={state => toggleEditMode(state)}
              info={usersStore.currentUser}/>
            : <ProfileCard
              onToggleEdit={state => toggleEditMode(state)}
              onLogout={handleLogout}
              info={usersStore.currentUser}/>
        }
      </Wrapper>
    </MainLayout>
  )
}

export default inject('usersStore')(observer(MyProfilePage))