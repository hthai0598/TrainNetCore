import React from 'react'
import filterIcon from '../../../assets/icons/filter-icn@2x.png'
import {
  AdvanceFilterTriggerButton,
} from './CustomStyled'

const AdvanceFilterTrigger = ({ active, onClick, theme }) => {
  return (
    <AdvanceFilterTriggerButton
      onClick={onClick}
      className={theme.name}
      style={{
        borderColor: active ? theme.solidColor : '#E3E5E5',
      }}>
      <img src={filterIcon} alt="Filter" style={{
        height: 15,
        marginRight: 10,
      }}/>
      Filter
    </AdvanceFilterTriggerButton>
  )
}

export default AdvanceFilterTrigger