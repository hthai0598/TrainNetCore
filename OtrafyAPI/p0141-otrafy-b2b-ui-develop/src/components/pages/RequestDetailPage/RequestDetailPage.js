import React, { useEffect } from 'react'
import { Helmet } from 'react-helmet'
import MainLayout from '../../layout/MainLayout'
import PageHeading from '../../organisms/PageHeading'
import breadcrumbs from './breadcrumbs'
import {
  PageWrapper,
} from './RequestDetailPageStyled'
import CommentsBlock from '../../organisms/CommentsBlock'
import RequestViewer from '../../organisms/RequestViewer'
import { Button, message } from 'antd'
import { inject, observer } from 'mobx-react'
import { toJS } from 'mobx'

const RequestDetailPage = ({ formsRequestsStore, usersStore, history }) => {

  const { currentPage, currentForm } = formsRequestsStore.survey

  useEffect(() => {
    const role = usersStore.currentUser.role
    if (role) {
      switch (role) {
        case 'suppliers':
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
        <title>Request detail | Otrafy</title>
      </Helmet>
      <PageHeading
        breadcrumbs={breadcrumbs}
        title={'Request detail'}>
        <Button type={'danger'} ghost onClick={() => history.push('/request-management')}>
          Cancel
        </Button>
        <span style={{ width: 10, display: 'inline-block' }}/>
        <Button
          key="submit" htmlType="submit" type={'primary'}
          form={
            formsRequestsStore.survey.surveyData.length > 0
              ? formsRequestsStore.survey.surveyData[currentForm].formId
              : null
          }>
          Save changes
        </Button>
      </PageHeading>
      <PageWrapper>
        <RequestViewer/>
        <CommentsBlock/>
      </PageWrapper>
    </MainLayout>
  )
}

export default inject('formsRequestsStore', 'usersStore')(observer(RequestDetailPage))