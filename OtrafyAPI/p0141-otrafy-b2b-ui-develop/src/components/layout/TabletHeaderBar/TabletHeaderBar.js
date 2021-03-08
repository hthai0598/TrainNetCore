import React from 'react'
import {
  HeaderBarWrapper, LogoWrapper, DrawerToggler, ToolsboxWrapper, AvatarWrapper,
} from './CustomTabletHeaderBarStyled'
import { inject, observer } from 'mobx-react'
import { withRouter } from 'react-router-dom'
import { Link } from 'react-router-dom'
import { Dropdown, Menu, Icon } from 'antd'
// Icons
import logo from '../../../assets/icons/otrafy-logo-tablet@2x.png'
import toggler from '../../../assets/icons/drawer-toggler-icn@2x.png'
import bellIcon from '../../../assets/sidebar/bell@2x.png'
import defaultAvatar from '../../../assets/imgs/tt_avatar_small.jpg'

const TabletHeaderBar = ({ history, commonStore, usersStore }) => {

  const handleLogout = () => {
    usersStore.userLogout()
      .then(() => history.push('/login'))
  }

  const menu = (
    <Menu>
      <Menu.Item key={'my-profile'}>
        <Link to={'/my-profile'}>
          <Icon type="user" style={{
            marginRight: 5,
          }}/> My profile
        </Link>
      </Menu.Item>
      <Menu.Item key={'logout'}>
        <div onClick={handleLogout}>
          <Icon type="logout" style={{
            marginRight: 5,
          }}/> Logout
        </div>
      </Menu.Item>
    </Menu>
  )

  return (
    <HeaderBarWrapper theme={commonStore.appTheme}>
      <DrawerToggler onClick={() => commonStore.toggleDrawer(true)}>
        <img src={toggler} alt="" width={36}/>
      </DrawerToggler>
      <LogoWrapper to={'/'}>
        <img src={logo} alt="Otrafy" width={131}/>
      </LogoWrapper>
      <ToolsboxWrapper>
        <Link to='/notifications'>
          <img src={bellIcon} alt="Notifications" height={21.41} className={'anticon'}/>
        </Link>
        <Dropdown overlay={menu} trigger={['click']} placement={'bottomRight'}>
          <AvatarWrapper>
            <img src={defaultAvatar} alt=""/>
          </AvatarWrapper>
        </Dropdown>
      </ToolsboxWrapper>
    </HeaderBarWrapper>
  )
}

export default withRouter(inject('commonStore', 'usersStore')(observer(TabletHeaderBar)))