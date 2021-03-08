import React, { useEffect, useState } from 'react'
import PropTypes from 'prop-types'
import { Link } from 'react-router-dom'
import { Menu } from 'antd'
import { inject, observer } from 'mobx-react'
import { withRouter } from 'react-router-dom'
// Icons
import companyManagementIcon from '../../../assets/sidebar/company-management@2x.png'
import repostAnalyticsIcon from '../../../assets/sidebar/report-analytics@2x.png'
import dashboardIcon from '../../../assets/sidebar/dashboard@2x.png'
import suppliersIcon from '../../../assets/sidebar/suppliers@2x.png'
import formsIcon from '../../../assets/sidebar/forms@2x.png'
import supportIcon from '../../../assets/sidebar/support@2x.png'

const NavigationMenu = props => {

  const {
    match,
    commonStore, usersStore,
  } = props

  const menu = match.path.split('/')[1]

  const [menuKeyPath, setMenuKeyPath] = useState([])

  const onClickLinkHandler = () => {
    commonStore.toggleDrawer(false)
  }

  useEffect(() => {
    setMenuKeyPath([menu])
  }, [])

  const content = () => {
    switch (usersStore.currentUser.role) {
      case `administrator`:
        return (
          <Menu
            selectedKeys={menuKeyPath}
            mode='inline'
            inlineCollapsed={commonStore.isSidebarCollapse}>
            <Menu.Item key={'company-management'}>
              <Link to={'/company-management'} onClick={onClickLinkHandler}>
                <img src={companyManagementIcon} alt="Company management" height={19.5} className={'anticon'}/>
                <span>Company</span>
              </Link>
            </Menu.Item>
            <Menu.Item key={'reports-analytics'}>
              <Link to={'/reports-analytics'} onClick={onClickLinkHandler}>
                <img src={repostAnalyticsIcon} alt="Report & Analytics" height={19.5} className={'anticon'}/>
                <span>Report & Analytics</span>
              </Link>
            </Menu.Item>
          </Menu>
        )
      case `suppliers`:
        return (
          <Menu
            selectedKeys={menuKeyPath}
            mode={'inline'}
            inlineCollapsed={commonStore.isSidebarCollapse}>
            <Menu.Item key={'request-management'}>
              <Link to={'/request-management'} onClick={onClickLinkHandler}>
                <img src={formsIcon} alt="Request management" height={21.67} className={'anticon'}/>
                <span>Request management</span>
              </Link>
            </Menu.Item>
            <Menu.Item key={'support'}>
              <Link to={'/support'} onClick={onClickLinkHandler}>
                <img src={supportIcon} alt="Supports" height={24.47} className={'anticon'}/>
                <span>Support</span>
              </Link>
            </Menu.Item>
          </Menu>
        )
      case `buyers`:
        return (
          <Menu
            selectedKeys={menuKeyPath}
            mode={'inline'}
            inlineCollapsed={commonStore.isSidebarCollapse}>
            <Menu.Item key={'dashboard'}>
              <Link to={'/dashboard'} onClick={onClickLinkHandler}>
                <img src={dashboardIcon} alt="Dashboard" height={21.67} className={'anticon'}/>
                <span>Dashboard</span>
              </Link>
            </Menu.Item>
            <Menu.Item key={'suppliers-management'}>
              <Link to={'/suppliers-management'} onClick={onClickLinkHandler}>
                <img src={suppliersIcon} alt="Suppliers" height={17.33} className={'anticon'}/>
                <span>Suppliers</span>
              </Link>
            </Menu.Item>
            <Menu.Item key="all-forms">
              <Link to={'/all-forms'} onClick={onClickLinkHandler}>
                <img src={formsIcon} alt="Forms" height={21.67} className={'anticon'}/>
                <span>All forms</span>
              </Link>
            </Menu.Item>
            <Menu.Item key={'reports-analytics'}>
              <Link to={'/reports-analytics'} onClick={onClickLinkHandler}>
                <img src={repostAnalyticsIcon} alt="Report & Analytics" height={19.5} className={'anticon'}/>
                <span>Report & Analytics</span>
              </Link>
            </Menu.Item>
            <Menu.Item key={'support'}>
              <Link to={'/support'} onClick={onClickLinkHandler}>
                <img src={supportIcon} alt="Supports" height={24.47} className={'anticon'}/>
                <span>Support</span>
              </Link>
            </Menu.Item>
          </Menu>
        )
    }
  }

  return (
    <React.Fragment>
      {content()}
    </React.Fragment>
  )
}

NavigationMenu.propTyes = {
  activeMenu: PropTypes.string.isRequired,
  isCollapsed: PropTypes.bool.isRequired,
}

export default withRouter(inject('usersStore', 'commonStore')(observer(NavigationMenu)))