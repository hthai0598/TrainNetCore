import React from 'react'
import PropTypes from 'prop-types'
import { Button } from 'antd'
import {
  CardWrapper, CardBodyContentCol, CardBody, CardHeader,
  Avatar, AvatarWrapper,
  UserInfoTop,
  List,
} from './CustomStyled'
// Icons
import avatar from '../../../assets/imgs/tt_avatar_small.jpg'
import logout from '../../../assets/icons/logout-icn@2x.png'
import { ReactComponent as MobileIcon } from '../../../assets/svg/mobile-icn.svg'
import { ReactComponent as EmailIcon } from '../../../assets/svg/email-icn.svg'
import { ReactComponent as UserIcon } from '../../../assets/svg/user-card-icn.svg'
import { ReactComponent as JobIcon } from '../../../assets/svg/briefcase-icn.svg'
import { ReactComponent as BuildingIcon } from '../../../assets/svg/building-icn.svg'
import { ReactComponent as MarkerIcon } from '../../../assets/svg/marker-icn.svg'
import { ReactComponent as NoteIcon } from '../../../assets/svg/note-icn.svg'

const ProfileCard = ({ info, onLogout, onToggleEdit }) => {

  return (
    <CardWrapper>
      <CardHeader>
        <AvatarWrapper>
          <Avatar src={avatar} alt="Avatar"/>
          <UserInfoTop>
            <div className={'name-row'}>
              {info.userProfiles ? info.userProfiles.firstName : null} {info.userProfiles ? info.userProfiles.lastName : null}
            </div>
            <div className={'action-row'} onClick={onLogout}>
              Logout
              <img src={logout} alt="Logout"/>
            </div>
          </UserInfoTop>
        </AvatarWrapper>
        <Button onClick={() => onToggleEdit(true)} type={'primary'}>
          Edit information
        </Button>
      </CardHeader>
      <CardBody>
        <CardBodyContentCol>
          <div className={'title'}>Personal information:</div>
          <List>
            <dt>
              <UserIcon className={'color-svg'} style={{ top: 3 }}/>
              Fullname:
            </dt>
            <dd>
              {info.userProfiles ? info.userProfiles.firstName : null} {info.userProfiles ? info.userProfiles.lastName : null}
            </dd>
            <dt>
              <EmailIcon className={'color-svg'} style={{ top: 5 }}/>
              Email:
            </dt>
            <dd>
              {info.email}
            </dd>
            <dt>
              <MobileIcon className={'color-svg'} style={{ top: 3 }}/>
              Phone number:
            </dt>
            <dd>{info.userProfiles ? info.userProfiles.phone : null}</dd>
            <dt>
              <JobIcon className={'color-svg'} style={{ top: 4 }}/>
              Job title:
            </dt>
            <dd>{info.userProfiles ? info.userProfiles.jobTitle : null}</dd>
          </List>
        </CardBodyContentCol>
        <CardBodyContentCol>
          <div className={'title'}>Company information:</div>
          <List>
            <dt>
              <BuildingIcon className={'color-svg'} style={{ top: 4 }}/>
              Name:
            </dt>
            <dd>
              {info.companyProfiles ? info.companyProfiles.companyName : null}
            </dd>
            <dt>
              <MarkerIcon className={'color-svg'} style={{ top: 3.5, width: 9 }}/>
              Address:
            </dt>
            <dd>
              {info.companyProfiles ? info.companyProfiles.address : null}
            </dd>
            <dt>
              <NoteIcon className={'color-svg'} style={{ top: 4.5 }}/>
              Description:
            </dt>
            <dd>
              {info.companyProfiles ? info.companyProfiles.description : null}
            </dd>
          </List>
        </CardBodyContentCol>
      </CardBody>
    </CardWrapper>
  )
}

ProfileCard.propTypes = {
  info: PropTypes.object,
}

export default ProfileCard