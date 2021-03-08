import React, { useEffect } from 'react'
import { Helmet } from 'react-helmet'
import { inject, observer } from 'mobx-react'
import MainLayout from '../../layout/MainLayout'
import PageHeading from '../../organisms/PageHeading'
import breadcrumbs from './breadcrumbs'
import { Button } from 'antd'
import FormButtonGroup from '../../organisms/FormButtonGroup'
import FormCreator from '../../organisms/FormCreator'
import PageFormWrapper from '../../organisms/PageFormWrapper'
import EditForm from './EditForm'

const EditFormPage = props => {

  const {
    history, match,
    formsStore, buyersStore, tagsStore,
  } = props

  const formId = match.params.formId

  const handleSaveForm = () => {
    buyersStore.updateFormData(formId, history)
  }

  useEffect(() => {
    formsStore.getFormDetail(formId)
    tagsStore.getAllTags('')
    buyersStore.setFormCreationProgress(1)
    return () => {
      buyersStore.clearForm()
      tagsStore.clearTags()
      formsStore.clearAllFormsData()
    }
  }, [])

  return (
    <MainLayout>
      <Helmet>
        <title>Edit form detail | Otrafy</title>
      </Helmet>
      <PageHeading
        breadcrumbs={breadcrumbs}
        title={'Edit form detail'}>
        {
          buyersStore.formCreateProgress === 1
            ? null
            : <FormButtonGroup>
              <Button type={'danger'} ghost onClick={() => history.push(`/all-forms`)}>
                Cancel
              </Button>
              <Button type={'primary'} onClick={handleSaveForm}>
                Save form
              </Button>
            </FormButtonGroup>
        }
      </PageHeading>
      {
        buyersStore.formCreateProgress === 1
          ? <PageFormWrapper
            form={
              buyersStore.formCreateValues.name && buyersStore.formCreateValues.tags && buyersStore.formCreateValues.surveyDesigner
                ? <EditForm tagsList={tagsStore.tagsList}/>
                : null
            }
            title={'Edit form detail'}/>
          : <FormCreator/>
      }
    </MainLayout>
  )
}

export default inject('formsStore', 'buyersStore', 'tagsStore')(observer(EditFormPage))