import React, { useEffect } from 'react'
import { inject, observer } from 'mobx-react'
import { withRouter } from 'react-router-dom'
import {
  RequestViewerWrapper, RequestViewerHeading,
} from './RequestViewerStyled'
import RequestForm from './RequestForm'
import StatusTag from '../../elements/StatusTag'

const RequestViewer = props => {

  const {
    history, match,
    formsRequestsStore, commonStore,
  } = props

  const formRequestId = match.params.formRequestId

  const checkAndShowStatusTag = () => {
    switch (formsRequestsStore.formRequestDetail.status) {
      case 1:
        return <StatusTag mainColor={'yellow'}>Pending</StatusTag>
      case 2:
        return <StatusTag mainColor={'green'}>Completed</StatusTag>
      case 3:
        return <StatusTag mainColor={'red'}>Rejected</StatusTag>
      case 4:
        return <StatusTag mainColor={'blue'}>Approved</StatusTag>
    }
  }

  useEffect(() => {
    formsRequestsStore.getFormRequestDetailById(formRequestId)
    return () => {
      formsRequestsStore.clearOnLeave()
    }
  }, [])

  return (
    <RequestViewerWrapper>
      <RequestViewerHeading theme={commonStore.appTheme}>
        <h2>{formsRequestsStore.formRequestDetail.title}</h2>
        {checkAndShowStatusTag()}
      </RequestViewerHeading>
      {
        formsRequestsStore.survey.surveyData.length > 0
          ? <RequestForm/>
          : null
      }
    </RequestViewerWrapper>
  )
}

export default withRouter(inject('formsRequestsStore', 'commonStore')(observer(RequestViewer)))