import React, { useState, useEffect } from 'react'
import { Link } from 'react-router-dom'
import logo from '../../../assets/imgs/otrafy-logo@2x.png'
import { inject, observer } from 'mobx-react'
import { apiUrl } from '../../../config'
import { Dropdown, Menu, Icon } from 'antd'
import { withRouter } from 'react-router-dom'
import defaultAvatar from '../../../assets/imgs/tt_avatar_small.jpg'
import NavigationMenu from '../NavigationMenu'
import arrow from '../../../assets/icons/sidebar-arrow-white@2x.png'
import {
  AppSidebar,
  LogoWrapper,
  UtilityMenu,
  UserMenu,
  Avatar,
  CollapseButtonWrapper,
} from './CustomStyled'
// Icons
import bellIcon from '../../../assets/sidebar/bell@2x.png'

const Sidebar = ({ usersStore, commonStore, history, location }) => {

  const [menuKeyPath, setMenuKeyPath] = useState([])

  const handleLogout = () => {
    usersStore.userLogout()
      .then(() => history.push('/login'))
  }

  useEffect(() => {
    setMenuKeyPath([location.pathname.replace('/', '')])
  }, [])

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

  const CollapseButton = () => {
    return (
      <CollapseButtonWrapper onClick={() => commonStore.toggleCollapseSidebar(!commonStore.isSidebarCollapse)}>
        {
          commonStore.isSidebarCollapse
            ? <img src={arrow} alt="" height={7}/>
            : <img src={arrow} alt="" height={7} style={{ transform: 'rotate(180deg)' }}/>
        }
      </CollapseButtonWrapper>
    )
  }

  const style = {
    background: commonStore.appTheme.gradientColor,
    boxShadow: commonStore.appTheme.shadowColor,
  }

  return (
    <AppSidebar style={style}>
      <LogoWrapper to={'/'}>
        <img src={logo} alt="Otrafy"/>
      </LogoWrapper>
      <CollapseButton/>
      {
        usersStore.currentUser
          ? <NavigationMenu/>
          : null
      }
      <UtilityMenu>
        <Menu
          selectedKeys={menuKeyPath}
          mode='inline'
          inlineCollapsed={commonStore.isSidebarCollapse}>
          <Menu.Item key={'notifications'}>
            <Link to='/notifications'>
              <img src={bellIcon} alt="Notifications" height={21.41} className={'anticon'}/>
              <span>Notifications</span>
            </Link>
          </Menu.Item>
        </Menu>
        <Dropdown overlay={menu} overlayClassName={'user-avatar-menu'} placement="topCenter">
          <UserMenu>
            <Avatar className={'user-menu-zone'}>
              <img src={defaultAvatar} alt=""/>
              <div style={{
                maxWidth: commonStore.isSidebarCollapse ? 0 : 'unset',
                opacity: commonStore.isSidebarCollapse ? 0 : 1,
              }}>
                {
                  usersStore.currentUser.userProfiles ?
                    `${usersStore.currentUser.userProfiles.firstName} ${usersStore.currentUser.userProfiles.lastName}`
                    : null
                }
              </div>
            </Avatar>
          </UserMenu>
        </Dropdown>
      </UtilityMenu>
    </AppSidebar>
  )
}

export default withRouter(inject('usersStore', 'commonStore')(observer(Sidebar)))