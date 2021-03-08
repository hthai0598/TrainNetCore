import React from 'react'
import { Icon, Tooltip } from 'antd'
import avatar from '../../../assets/dummy/user-avatar@2x.png'
import InfoCard from '../../organisms/InfoCard'
import moment from 'moment'
import ActiveStatusIndicator from '../../elements/ActiveStatusIndicator'
import { inject, observer } from 'mobx-react'
import { withRouter } from 'react-router-dom'
import {
  CompanyDetailCardWrapper,
  CardHeader,
  CardFooter,
  List,
} from './CustomStyled'
// Icons
import { ReactComponent as EmailIcon } from '../../../assets/svg/email-icn.svg'
import { ReactComponent as MobileIcon } from '../../../assets/svg/mobile-icn.svg'
import { ReactComponent as WebsiteIcon } from '../../../assets/svg/website-icn.svg'
import { ReactComponent as CalendarIcon } from '../../../assets/svg/calendar-icn.svg'
import { ReactComponent as MarkerIcon } from '../../../assets/svg/marker-icn.svg'

const CompanyDetailCard = ({ onToggleEditMode, companiesStore, commonStore, history }) => {

  return (
    <React.Fragment>
      <CompanyDetailCardWrapper>
        <CardHeader>
          <div className="avatar">
            <img src={avatar} alt="User Avatar"/>
          </div>
          <div className="info">
            <div className="name">
              <span>{companiesStore.currentCompanyView.name}</span>
              <Tooltip title={'Edit company info'}>
                <div className="action"
                     style={{ background: commonStore.appTheme.gradientColor }}
                     onClick={onToggleEditMode}>
                  <Icon type="edit"/>
                </div>
              </Tooltip>
            </div>
            <ActiveStatusIndicator type={'button'} status={companiesStore.currentCompanyView.isActive}/>
          </div>
        </CardHeader>
        <CardFooter>
          <div style={{ color: '#000', fontSize: 14, fontWeight: 500, marginBottom: 30 }}>Contact information:</div>
          <List>
            <dt>
              <EmailIcon className={'color-svg'} style={{ top: 5 }}/> Email:
            </dt>
            <dd>
              <span>{companiesStore.currentCompanyView.email}</span>
            </dd>
            <dt>
              <MobileIcon className={'color-svg'} style={{ top: 3 }}/> Phone number:
            </dt>
            <dd>
              <span>{companiesStore.currentCompanyView.phone}</span>
            </dd>
            <dt>
              <WebsiteIcon className={'color-svg'} style={{ top: 5 }}/> Website:
            </dt>
            <dd>
              <span>{companiesStore.currentCompanyView.website}</span>
            </dd>
            <dt>
              <CalendarIcon className={'color-svg'} style={{ top: 3 }}/> Date created:
            </dt>
            <dd>
              <span>{moment(companiesStore.currentCompanyView.dateCreated).format('MMMM Do YYYY, h:mm:ss a')}</span>
            </dd>
            <dt>
              <MarkerIcon className={'color-svg'} style={{ top: 2, width: 9, left: 1 }}/> Address:
            </dt>
            <dd>
              <span>{companiesStore.currentCompanyView.address}</span>
            </dd>
          </List>
        </CardFooter>
        <InfoCard
          color={'#FF9800'}
          title={'Suppliers invited:'}
          left={companiesStore.currentCompanyView.totalSuppliersInvited}
          right={companiesStore.currentCompanyView.maxNumberSuppliersAllowed}
          leftTxt={'Suppliers invited'}
          rightTxt={'Maximum'}/>
        <InfoCard
          color={'#E91E63'}
          title={'Forms created:'}
          left={companiesStore.currentCompanyView.totalFormsCreated}
          right={companiesStore.currentCompanyView.maxNumberFormsAllowed}
          leftTxt={'Forms created'}
          rightTxt={'Maximum'}
        />
      </CompanyDetailCardWrapper>
    </React.Fragment>
  )
}

export default inject('commonStore', 'companiesStore')(observer(CompanyDetailCard))