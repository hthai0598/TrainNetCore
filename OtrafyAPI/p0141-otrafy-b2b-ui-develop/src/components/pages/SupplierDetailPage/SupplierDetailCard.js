import React, { useEffect } from 'react'
import { inject, observer } from 'mobx-react'
import { toJS } from 'mobx'
import { withRouter } from 'react-router-dom'
import supplierAvatar from '../../../assets/dummy/supplier-avatar@2x.png'
import { Icon, Tooltip } from 'antd'
import { CardWrapper } from './CustomStyled'
import NormalTag from '../../elements/NormalTag'
// Icons
import { ReactComponent as TagIcon } from '../../../assets/svg/tag-icn.svg'
import { ReactComponent as EmailIcon } from '../../../assets/svg/email-icn.svg'
import { ReactComponent as NoteIcon } from '../../../assets/svg/note-icn.svg'
import { ReactComponent as MobileIcon } from '../../../assets/svg/mobile-icn.svg'
import { ReactComponent as BriefcaseIcon } from '../../../assets/svg/briefcase-icn.svg'
import { ReactComponent as MarkerIcon } from '../../../assets/svg/marker-icn.svg'
import { ReactComponent as UserCardIcon } from '../../../assets/svg/user-card-icn.svg'

const SupplierDetailCard = ({ suppliersStore, commonStore, match }) => {

  const supplierId = match.params.supplierId
  const supplierDetail = toJS(suppliersStore.supplierDetail)

  useEffect(() => {
    suppliersStore.getSupplierDetail(supplierId)
  }, [])

  return (
    <CardWrapper>
      <div className="heading">
        <div className="info">
          <img src={supplierAvatar} alt=""/>
          <span>{supplierDetail.companyProfiles ? supplierDetail.companyProfiles.companyName : null}</span>
        </div>
        <Tooltip title={'Edit supplier info'}>
          <div className="action"
               style={{ background: commonStore.appTheme.gradientColor }}
               onClick={() => suppliersStore.toggleEditMode(true)}>
            <Icon type="edit"/>
          </div>
        </Tooltip>
      </div>
      <div className="body">
        <div className="title">
          Basic information:
        </div>
        <dl className="list">
          <dt>
            <EmailIcon className={'color-svg'} style={{ width: 11.67 }}/>
            Email:
          </dt>
          <dd>
            {supplierDetail.email}
          </dd>
          {
            suppliersStore.supplierDetailActiveTab === '1'
              ? null
              : <React.Fragment>
                <dt>
                  <MobileIcon className={'color-svg'} style={{ width: 7.58, top: 3, left: 2 }}/>
                  Phone number:
                </dt>
                <dd>
                  {supplierDetail.userProfiles ? supplierDetail.userProfiles.phone : null}
                </dd>
                <dt>
                  <UserCardIcon className={'color-svg'} style={{ width: 10.5, top: 3 }}/>
                  Persons in contact:
                </dt>
                <dd>
                  {supplierDetail.userProfiles
                    ? `${supplierDetail.userProfiles.firstName} ${supplierDetail.userProfiles.lastName}`
                    : null}
                </dd>
                <dt>
                  <BriefcaseIcon className={'color-svg'} style={{ width: 11.67 }}/>
                  Job title:
                </dt>
                <dd>
                  {supplierDetail.userProfiles
                    ? supplierDetail.userProfiles.jobTitle
                    : null}
                </dd>
                <dt>
                  <MarkerIcon className={'color-svg'} style={{ width: 9, left: 2 }}/>
                  Address:
                </dt>
                <dd>
                  {supplierDetail.companyProfiles
                    ? supplierDetail.companyProfiles.address
                    : null}
                </dd>
              </React.Fragment>
          }
          <dt>
            <NoteIcon className={'color-svg'} style={{ width: 11.67 }}/>
            Description:
          </dt>
          <dd>
            {supplierDetail.companyProfiles ? supplierDetail.companyProfiles.description : null}
          </dd>
          {
            suppliersStore.supplierDetailActiveTab === '2'
              ? null
              : <React.Fragment>
                <dt>
                  <TagIcon className={'color-svg'} style={{ width: 11.67 }}/>
                  Tags:
                </dt>
                <dd>
                  {
                    supplierDetail.tags !== undefined
                      ? <NormalTag tags={supplierDetail.tags}/>
                      : null
                  }
                </dd>
              </React.Fragment>
          }
        </dl>
      </div>
    </CardWrapper>
  )
}

export default withRouter(inject('suppliersStore', 'commonStore')(observer(SupplierDetailCard)))