import React, { useEffect } from 'react'
import { Helmet } from 'react-helmet'
import MainLayout from '../../layout/MainLayout'
import PageHeading from '../../organisms/PageHeading'
import breadcrumbs from './breadcrumbs'
import { Button, Popconfirm, Icon, message } from 'antd'
import { inject, observer } from 'mobx-react'
import PageFormWrapper from '../../organisms/PageFormWrapper'
import CreateNewForm from './CreateNewForm'
import FormButtonGroup from '../../organisms/FormButtonGroup'
import FormCreator from '../../organisms/FormCreator'

const CreateNewFormPage = ({ history, buyersStore, tagsStore, formsStore, usersStore }) => {

  const handleCancelCreateForm = () => {
    history.push('/all-forms')
    formsStore.clearAllFormsData()
  }

  useEffect(() => {
    buyersStore.setFormCreationProgress(1)
    tagsStore.getAllTags('')
  }, [])

  useEffect(() => {
    buyersStore.clearForm()
    tagsStore.clearTags()
    formsStore.clearAllFormsData()
    return () => {
      buyersStore.clearForm()
      tagsStore.clearTags()
      formsStore.clearAllFormsData()
    }
  }, [])

  useEffect(() => {
    const role = usersStore.currentUser.role
    if (role) {
      switch (role) {
        case 'buyers':
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
        <title>Create new form | Otrafy</title>
      </Helmet>
      <PageHeading title={'Create new form'} breadcrumbs={breadcrumbs}>
        {
          buyersStore.formCreateProgress === 1
            ? null
            : <FormButtonGroup>
              <Popconfirm
                placement={'bottomRight'}
                onConfirm={handleCancelCreateForm}
                okType={'danger'} okText={'Confirm'}
                icon={<Icon type="question-circle-o" style={{ color: 'red' }}/>}
                title={`All form data will be cleared. This can't be undone`}>
                <Button type={'danger'} ghost>
                  Cancel
                </Button>
              </Popconfirm>
              <Button type={'primary'} onClick={() => buyersStore.createFormData(history)}>
                Create form
              </Button>
            </FormButtonGroup>
        }
      </PageHeading>
      {
        buyersStore.formCreateProgress === 1
          ? <PageFormWrapper
            form={<CreateNewForm tagsList={tagsStore.tagsList}/>}
            title={'Create new form'}/>
          : <FormCreator/>
      }
    </MainLayout>
  )
}

export default inject('buyersStore', 'tagsStore', 'formsStore', 'usersStore')(observer(CreateNewFormPage))