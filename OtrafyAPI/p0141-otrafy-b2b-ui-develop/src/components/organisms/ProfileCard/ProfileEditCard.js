import React from 'react'
import PropTypes from 'prop-types'
import { Button } from 'antd'
import {
  CardWrapper,
  CardBody,
  CardHeader,
  Avatar,
  AvatarWrapper,
  UserInfoTop,
} from './CustomStyled'
// Icons
import avatar from '../../../assets/imgs/tt_avatar_small.jpg'
import logout from '../../../assets/icons/logout-icn@2x.png'
import FormButtonGroup from '../FormButtonGroup'

const ProfileEditCard = ({ info, onLogout, onToggleEdit, form }) => {

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
        <FormButtonGroup>
          <Button form="user-info-update-form" type='primary'
            key="submit" htmlType="submit">
            Save changes
          </Button>
          <Button onClick={() => onToggleEdit(false)}>
            Cancel
          </Button>
        </FormButtonGroup>
      </CardHeader>
      <CardBody>
        {form}
      </CardBody>
    </CardWrapper>
  )
}

ProfileEditCard.propTypes = {
  info: PropTypes.object,
}

export default ProfileEditCard