import React from 'react'
import styled from 'styled-components'
import { Icon, Dropdown } from 'antd'

const DisplayText = styled.div`
  display: inline-block;
  background: #F7F9FA;
  border-radius: 99px;
  color: #919699;
  font-size: 12px;
  padding: 6px 15px;
  &:hover {
    cursor: pointer;
  }
`

const CustomDropdown = ({ pageSize, total, dropdownMenu }) => {

  return (
    <Dropdown
      trigger={['click']}
      placement="topLeft"
      overlay={dropdownMenu}>
      <DisplayText>
        Showing <strong>{pageSize}</strong> of <strong>{total}</strong> items
        <Icon type="caret-down" style={{ color: '#46A41D', marginLeft: '15px' }}/>
      </DisplayText>
    </Dropdown>
  )
}

export default CustomDropdown